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
    public class TagController : Controller
    {
        DMSContext _dmsContext;
        public TagController(DMSContext dMSContext)
        {
            _dmsContext = dMSContext;
        }

        public IActionResult Index()
        {
            var tags = _dmsContext.Tag.Include(t => t.FileTag).OrderByDescending(t => t.FileTag.Count).ToList();
            return View(tags);
        }

        public IActionResult Detail(string id)
        {
            var files = _dmsContext
                .Files
                .Include(file => file.FileTag)
                    .ThenInclude(filetag => filetag.File)
                .Where(f => f.FileTag.Any(ft => ft.Tag.Name == id))
                .OrderByDescending(file => file.DocumentDate)
                .ToList();
            ViewData["tagName"] = id;
            return View(files);
        }
    }
}
