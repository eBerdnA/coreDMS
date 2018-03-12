using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreDMS.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoreDMS.Controllers
{
    public class SearchController : Controller
    {
        DMSContext _dmsContext;
        private readonly IViewRenderService _viewRenderService;

        public SearchController(DMSContext dMSContext, IViewRenderService viewRenderService)
        {
            _dmsContext = dMSContext;
            _viewRenderService = viewRenderService;
        }

        public IActionResult Index()
        {
            var tags = _dmsContext.Tag.Include(t => t.FileTag).OrderBy(t => t.Name).ToList();
            return View(tags);
        }

        [HttpPost("/search/find")]
        public async Task<string> Search([FromBody] List<Tag> tags)
        {
            string[] tagArr = tags.Select(t => t.name).ToArray();
            var tagsResult2 = (from t in _dmsContext.Tag
                               where tagArr.Contains(t.Name)
                               join filetag in _dmsContext.FileTag
                                 on t.Id equals filetag.TagId
                               join file in _dmsContext.Files
                                 on filetag.FileId equals file.Id
                               group filetag.FileId by filetag.FileId into grouped
                               where grouped.Count() == tagArr.Length
                               select grouped).ToArray();

            List<string> fileIds = new List<string>();
            foreach (IGrouping<string, string> item in tagsResult2)
            {
                fileIds.Add(item.Key);
            }

            var files = (from f in _dmsContext.Files
                         where fileIds.Contains(f.Id)
                         select f).ToList();

            var result = await _viewRenderService.RenderToStringAsync("FilesPartial", files);
            return result;
        }

        [HttpPost("/search/hash")]
        public async Task<string> Hash([FromBody] HashSearch hash)
        {
            List<string> tagNames = new List<string>();
            var file = _dmsContext.Files.Where(f => f.Hash.Equals(hash.hash, StringComparison.OrdinalIgnoreCase)).ToList();
            var result = await _viewRenderService.RenderToStringAsync("FilesPartial", file);
            return result;
        }
    }

    public class Tag
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class HashSearch
    {
        public string hash { get; set; }
    }
}
