using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        public IActionResult Index(string fileid, int state, string tags, string documentdate)
        {
            var file = _dmsContext.Files
                .Include(f => f.FileTag)
                    .ThenInclude(ft => ft.Tag)
                .Where(f => f.Id == fileid).FirstOrDefault();
            file.State = state;
            file.UpdatedAt = DateTime.UtcNow.ToString(Constants.DocumentDateFormat);
            DateTime dt;
            DateTime.TryParse(documentdate, null, DateTimeStyles.AssumeUniversal, out dt);
            file.DocumentDate = dt.ToUniversalTime().ToString(Constants.DocumentDateFormat);

            List<string> splittedTags = SplitTags(tags);

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
            foreach (var tag in tags.Split(","))
            {
                splittedTags.Add(tag.Trim());

            }
            return splittedTags;
        }

        [HttpGet("/file/{id}")]
        public IActionResult Index(string id)
        {
            Files file = _dmsContext.Files
                .Include(f => f.FileTag)
                    .ThenInclude(filetag => filetag.Tag)
                .Where(f => f.Id == id)
                .FirstOrDefault();
            SingleFileViewModel model = new SingleFileViewModel(_settings)
            {
                File = file
            };
            StringBuilder sb = new StringBuilder();
            foreach (FileTag tag in file.FileTag)
            {
                sb.Append(tag.Tag.Name + ", ");
            }
            sb.Remove(sb.Length - 2, 2);
            model.Tags = sb.ToString();

            List<FileStates> states = _dmsContext.FileStates.ToList();
            model.FileStates = new List<SelectListItem>();
            foreach (FileStates state in states)
            {
                model.FileStates.Add(new SelectListItem { Text = state.Name, Value = state.Id.ToString() });
            }
            model.FileDate = DateTime.Parse(file.DocumentDate).ToString("yyyy-MM-dd");
            return View(model);
        }
    }

    
}
