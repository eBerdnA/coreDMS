using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CoreDMS.Model;
using CoreDMS.Services;
using CoreDMS.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoreDMS.Controllers
{
    public class UploadController : Controller
    {
        DMSContext _dmsContext;
        private readonly IViewRenderService _viewRenderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISettings _settings;
        private IHostingEnvironment _environment;
        string uploadPath = string.Empty;
        string processedPath = string.Empty;

        public UploadController(DMSContext dMSContext, IViewRenderService viewRenderService,
            IHttpContextAccessor httpContextAccessor, ISettings settings, IHostingEnvironment environment)
        {
            _dmsContext = dMSContext;
            _viewRenderService = viewRenderService;
            _httpContextAccessor = httpContextAccessor;
            _settings = settings;
            _environment = environment;
            uploadPath = settings.GetUploadsDirectory();
            processedPath = settings.GetProcessedDirectory();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/upload/upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file.Length > 0)
            {
                var filename = Helpers.Uploads.CreateLocationPath(uploadPath, file.FileName);
                try
                {                    
                    using (var fileStream = new FileStream(filename, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    _dmsContext.Upload.Add(new Upload
                    {
                        FileName = file.FileName,
                        Path = filename,
                        CreatedAt = DateTime.UtcNow.ToString(Constants.DocumentDateFormat)
                    });
                    _dmsContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    System.IO.File.Delete(filename);
                    return BadRequest($"error - {ex.Message}");
                }
            }
            return Ok();
        }

        public IActionResult Uploads()
        {
            UploadsViewModel model = new UploadsViewModel();
            var files = _dmsContext.Upload.ToList();
            List<string> filenames = new List<string>();
            model.UploadedFiles = files;
            return View(model);
        }

        public IActionResult Process(int id)
        {
            Upload upload = _dmsContext.Upload
                            .Include(u => u.UploadError)
                            .Where(u => u.Id == id).First();
            string fileHash = string.Empty;
            using (FileStream fs = new FileStream(upload.Path, FileMode.Open))
            using (BufferedStream bs = new BufferedStream(fs))
            {
                using (SHA1Managed sha1 = new SHA1Managed())
                {
                    byte[] hash = sha1.ComputeHash(bs);
                    StringBuilder formatted = new StringBuilder(2 * hash.Length);
                    foreach (byte b in hash)
                    {
                        formatted.AppendFormat("{0:X2}", b);
                    }
                    fileHash = formatted.ToString();
                }                
            }
            // check filehash to avoid archiving duplicates
            var checkHash = _dmsContext.Files.Where(f => f.Hash == fileHash).FirstOrDefault();
            if (checkHash != null)
            {
                _dmsContext.UploadError.Add(new UploadError
                {
                    UploadId = upload.Id,
                    Error = "Hash collision",
                    CreatedAt = DateTime.UtcNow.ToString(Constants.DocumentDateFormat)
                });
                _dmsContext.SaveChanges();
                return BadRequest("Hash collision");
            }
            _dmsContext.Database.BeginTransaction();
            var stateNew = _dmsContext.FileStates.Where(f => f.Name == "new").FirstOrDefault();
            DateTime processTime = DateTime.UtcNow;
            string fileProcessTime = processTime.ToString(Constants.FileProcessTimeDateFormat);
            try
            {
                string targetFileName = Helpers.Uploads.CreateProcessedLocation(processedPath, fileProcessTime, upload.FileName);
                System.IO.File.Move(upload.Path, targetFileName);
                _dmsContext.Files.Add(new Files
                {
                    Id = Guid.NewGuid().ToString(),
                    Filename = upload.FileName,
                    Title = upload.FileName,
                    CreatedAt = processTime.ToString(Constants.DocumentDateFormat),
                    UpdatedAt = processTime.ToString(Constants.DocumentDateFormat),
                    Hash = fileHash,
                    Location = targetFileName,
                    State = stateNew.Id
                });
                _dmsContext.Remove(upload);
                _dmsContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _dmsContext.Database.RollbackTransaction();
                return BadRequest(ex.Message);
            }
            _dmsContext.Database.CommitTransaction();
            return Ok();
        }

        public IActionResult Delete(int id)
        {
            _dmsContext.Database.BeginTransaction();
            try
            {
                var upload = _dmsContext.Upload
                                .Include(u => u.UploadError)
                                .Where(u => u.Id == id).FirstOrDefault();
                _dmsContext.Remove(upload);
                System.IO.File.Delete(upload.Path);
                _dmsContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _dmsContext.Database.RollbackTransaction();
                return BadRequest(ex.Message);
            }
            _dmsContext.Database.CommitTransaction();
            return Ok();
        }


        public IActionResult Errors()
        {
            var errors = _dmsContext.UploadError.ToList();
            return View(errors);
        }
    }
}
