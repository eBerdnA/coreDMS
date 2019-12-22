using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CoreDMS.Model;
using CoreDMS.Services;
using CoreDMS.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoreDMS.Controllers
{
    public class FileController : Controller
    {
        DMSContext _dmsContext;
        private readonly IViewRenderService _viewRenderService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISettings _settings;

        public FileController(DMSContext dMSContext, IViewRenderService viewRenderService, IHttpContextAccessor httpContextAccessor, ISettings settings)
        {
            _dmsContext = dMSContext;
            _viewRenderService = viewRenderService;
            _httpContextAccessor = httpContextAccessor;
            _settings = settings;
        }

        [HttpPost("/file/{id}")]
        public IActionResult Index(string fileid, int state, string tags, string documentdate, string note)
        {
            var file = _dmsContext.Files
                .Include(f => f.FileTag)
                    .ThenInclude(ft => ft.Tag)
                .Where(f => f.Id == fileid).FirstOrDefault();
            file.State = state;
            file.UpdatedAt = DateTime.UtcNow.ToString(Constants.DocumentDateFormat);
            if (documentdate != null)
            {
                DateTime dt;
                DateTime.TryParse(documentdate, null, DateTimeStyles.AssumeUniversal, out dt);
                file.DocumentDate = dt.ToUniversalTime().ToString(Constants.DocumentDateFormat); 
            }

            List<string> splittedTags = SplitTags(tags);
            file.Note = note;
            _dmsContext.Database.BeginTransaction();
            try
            {
                UpdateFileTags(_dmsContext, splittedTags.ToArray(), file);                    
                _dmsContext.SaveChanges();
                _dmsContext.Database.CommitTransaction();                
            }
            catch (Exception ex)
            {
                _dmsContext.Database.RollbackTransaction();
                return Redirect($"/file/{fileid}").Danger(Response, $"Saving not succesfull - {ex.Message}");
            }
            return Redirect($"/file/{fileid}").Success(Response, "Saving succesfull");
        }

        private void UpdateFileTags(DMSContext dMSContext, string[] tags, Files fileToUpdate)
        {
            if (tags != null)
            {
                var assignedTags = (from t in _dmsContext.Tag
                                    where tags.Contains(t.Name)
                                    select t).ToList();
                if (tags.Count() != assignedTags.Count())
                {
                    foreach (var tag in tags)
                    {
                        var lookupTag = _dmsContext.Tag.Where(t => t.Name == tag).FirstOrDefault();
                        if (lookupTag == null)
                        {
                            var addedTag = _dmsContext.Tag.Add(new Model.Tag
                            {
                                Name = tag,
                                CreatedAt = DateTime.UtcNow.ToString(),
                                UpdatedAt = DateTime.UtcNow.ToString(),
                            });
                        }
                    }
                    _dmsContext.SaveChanges();
                    assignedTags = (from t in _dmsContext.Tag
                                    where tags.Contains(t.Name)
                                    select t).ToList();
                }

                var selectedTagsHS = new HashSet<long>(assignedTags.Select(c => c.Id));
                var instructorCourses = new HashSet<long>
                    (fileToUpdate.FileTag.Select(c => c.Tag.Id));
                foreach (var tag in dMSContext.Tag)
                {
                    if (selectedTagsHS.Contains(tag.Id))
                    {
                        if (!instructorCourses.Contains(tag.Id))
                        {
                            fileToUpdate.FileTag.Add(new FileTag
                            {
                                FileId = fileToUpdate.Id,
                                TagId = tag.Id,
                                CreatedAt = DateTime.UtcNow.ToString(),
                                UpdatedAt = DateTime.UtcNow.ToString()
                            });
                        }
                    }
                    else
                    {

                        if (instructorCourses.Contains(tag.Id))
                        {
                            FileTag courseToRemove = fileToUpdate.FileTag.SingleOrDefault(i => i.TagId == tag.Id);
                            dMSContext.Remove(courseToRemove);
                        }
                    }
                }
            }
            else
            {
                fileToUpdate.FileTag = new List<FileTag>();
            }
        }

        private List<string> SplitTags(string tags)
        {
            var splittedTags = new List<string>();
            if (!string.IsNullOrEmpty(tags))
            {
                foreach (var tag in tags.Split(","))
                {
                    splittedTags.Add(tag.Trim());
                }
            }
            return splittedTags;
        }

        [HttpGet("/file/{id}")]
        public IActionResult Index(string id)
        {
            try
            {
                new Guid(id);
            }
            catch
            {
                return StatusCode(500, "invalid file id");
            }
            Files file = _dmsContext.Files
                .Include(f => f.FileTag)
                    .ThenInclude(filetag => filetag.Tag)
                .Where(f => f.Id == id)
                .FirstOrDefault();
            SingleFileViewModel model = new SingleFileViewModel(_settings)
            {
                File = file
            };
            if (model.Tags == null)
            {
                model.Tags = string.Empty;
            }
            model.Tags = BuildTagString(file.FileTag);
            List<FileStates> states = _dmsContext.FileStates.ToList();
            model.FileStates = new List<SelectListItem>();
            foreach (FileStates state in states)
            {
                model.FileStates.Add(new SelectListItem { Text = state.Name, Value = state.Id.ToString() });
            }
            if (file.DocumentDate != null && file.DocumentDate.Length > 1)
            {
                model.FileDate = DateTime.Parse(file.DocumentDate).ToString(Constants.DateFormatDateTimePicker);
            }
            return View(model);
        }

        private static string BuildTagString(ICollection<FileTag> fileTags)
        {
            StringBuilder sb = new StringBuilder();
            foreach (FileTag tag in fileTags)
            {
                sb.Append(tag.Tag.Name + ", ");
            }
            if (sb.Length > 1)
            {
                sb.Remove(sb.Length - 2, 2);
            }
            return sb.ToString();
        }

        [HttpGet("/file/get/{id}")]
        public FileContentResult Get(string id)
        {
            var file = _dmsContext.Files.Where(f => f.Id == id).FirstOrDefault();
            var memory = new MemoryStream();
            using (var stream = new FileStream(file.Location, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            // https://stackoverflow.com/a/38909848/715348
            System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
            {
                FileName = file.Filename,
                Inline = true  // false = prompt the user for downloading;  true = browser to try to show the file inline
            };

            Response.Headers.Add("Content-Disposition", cd.ToString());
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            return File(System.IO.File.ReadAllBytes(file.Location), GetContentType(file.Location));
        }

        [HttpGet("/file/tags/{id}")]
        public JsonResult Tags(string id)
        {
            TagViewModel model = new TagViewModel();
            Files file = _dmsContext.Files
                .Include(f => f.FileTag)
                    .ThenInclude(filetag => filetag.Tag)
                .Where(f => f.Id == id)
                .FirstOrDefault();
            model.tags = BuildTagString(file.FileTag);
            model.title = file.Title;
            model.location = file.Location;
            return Json(model);
        }

        [HttpPost("/file/delete/{id}")]
        public IActionResult Delete(string id)
        {
            _dmsContext.Database.BeginTransaction();
            try
            {
                var file = _dmsContext.Files
                            .Include(f => f.FileTag)
                            .Where(f => f.Id == id).FirstOrDefault();
                _dmsContext.Remove(file);
                System.IO.File.Delete(file.Location);
                _dmsContext.SaveChanges();
            }
            catch (Exception)
            {
                _dmsContext.Database.RollbackTransaction();
                return BadRequest();
            }
            _dmsContext.Database.CommitTransaction();
            return Ok();
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".css", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
    }

    
}
