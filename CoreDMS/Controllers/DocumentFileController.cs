using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CoreDMS.Model;
using CoreDMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CoreDMS.Controllers
{
    public class DocumentFileController : Controller
    {
        DMSContext _dmsContext;
        ISettings _settings;
        Microsoft.Extensions.Logging.ILogger _logger;
        public DocumentFileController(DMSContext dMSContext, ISettings settings, ILogger<HomeController> logger)
        {
            _dmsContext = dMSContext;
            _settings = settings;
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<DocumentFiles> documentFiles = _dmsContext.DocumentFiles.ToList();
            return View(documentFiles);
        }

        public IActionResult Create()
        {
            DocumentFiles documentFile = new DocumentFiles();
            return View(documentFile);
        }

        [HttpPost]
        public IActionResult Create([FromForm]DocumentFiles _documentFile)
        {
            DocumentFiles documentFile = new DocumentFiles
            {
                Title = _documentFile.Title,
                Note = _documentFile.Note,
                CreatedAt = DateTime.UtcNow.ToString(Constants.DocumentDateFormat),
                UpdatedAt = DateTime.UtcNow.ToString(Constants.DocumentDateFormat)
            };
            _dmsContext.Add(documentFile);
            _dmsContext.SaveChanges();
            return RedirectToAction("Create");
        }

        public IActionResult Detail(string id)
        {
            int? documentFileId = null;
            try
            {
                documentFileId = Convert.ToInt32(id);
            }
            catch {}
            if (documentFileId == null | documentFileId == 0)
            {
                return NotFound();
            }
            var documentFile = _dmsContext.DocumentFiles.Where(d => d.Id == documentFileId).FirstOrDefault();
            return View(documentFile);
        }

        [HttpPost]
        public IActionResult Detail([FromForm]DocumentFiles _documentFile)
        {
            var documentFile = _dmsContext.DocumentFiles.Where(d => d.Id == Convert.ToInt32(_documentFile.Id)).FirstOrDefault();
            documentFile.Title = _documentFile.Title;
            documentFile.Note = _documentFile.Note;
            documentFile.UpdatedAt = DateTime.UtcNow.ToString(Constants.DocumentDateFormat);
            _dmsContext.SaveChanges();
            return RedirectToAction("Detail", "DocumentFile", new { id = _documentFile.Id});
        }

        public class AddingDocumentFile
        {
            public string documentid { get; set;}
            public string fileid{ get; set; }
        }

        [HttpPost("/remove/documentfileid")]
        public IActionResult RemoveDocumentFile([FromBody] AddingDocumentFile values)
        {
            var checkResult = _dmsContext.DocumentFileFiles.Where(d => d.FileId == values.fileid && d.DocumentFileId == Convert.ToInt32(values.documentid)).FirstOrDefault();
            if (checkResult == null)
            {
                return NotFound();
            }
            _dmsContext.Remove(checkResult);
            try
            {
                _dmsContext.SaveChanges();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }

        [HttpPost("/add/documentfileid")]
        public IActionResult AddDocumentFile([FromBody] AddingDocumentFile values)
        {
            int Documentid = Convert.ToInt32(values.documentid);
            var checkResult = _dmsContext.DocumentFileFiles.Where(d => d.DocumentFileId == Documentid && d.FileId == values.fileid).FirstOrDefault();            
            if (checkResult != null)
            {
                return StatusCode(500,"Document already in file");
            }
            DocumentFileFile dff = new DocumentFileFile();
            dff.FileId = values.fileid;
            dff.DocumentFileId = Convert.ToInt32(values.documentid);
            dff.CreatedAt = DateTime.UtcNow.ToString(Constants.DocumentDateFormat);
            dff.UpdatedAt = DateTime.UtcNow.ToString(Constants.DocumentDateFormat);
            _dmsContext.Add(dff);
            try
            {
                _dmsContext.SaveChanges();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }

        [HttpPost("/add/fileid")]
        public string AddFileId([FromBody] PostData hash)
        {
            DocumentFileFile dff = new DocumentFileFile();
            dff.FileId = hash.documentId;
            dff.DocumentFileId = Convert.ToInt32(hash.documentFileId);
            dff.CreatedAt = DateTime.UtcNow.ToString(Constants.DocumentDateFormat);
            dff.UpdatedAt = DateTime.UtcNow.ToString(Constants.DocumentDateFormat);
            _dmsContext.Add(dff);
            _dmsContext.SaveChanges();
            return "done";
        }

        [HttpPost("/get/documentfileid")]
        public IActionResult GetDocumentFile([FromBody]int documentFileId)
        {
            if (documentFileId == null | documentFileId == 0)
            {
                return NotFound("DocumentFileId not found");
            }
            var documentFile = _dmsContext.DocumentFiles
                .Where(dff => dff.Id == documentFileId)
                .Include(df => df.DocumentFileFiles)
                .ThenInclude(dff => dff.File)
                .FirstOrDefault();

            List<Files> fileIds = new List<Files>();
            foreach(var file in documentFile.DocumentFileFiles)
            {
                
                fileIds.Add(new Files{
                    Filename = file.File.Filename,
                    Hash = file.File.Hash,
                    CreatedAt = file.File.CreatedAt,
                    UpdatedAt = file.File.UpdatedAt,
                    DocumentDate = file.File.DocumentDate,
                    Id = file.File.Id
                });
            }
            return Json(fileIds);
        }

        
    }

    public class PostData
    {
        public string documentFileId { get; set; }
        public string documentId { get; set; }
    }
}