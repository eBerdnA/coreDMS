using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreDMS.Model;
using CoreDMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CoreDMS.Controllers
{
    public class HomeController : Controller
    {
        DMSContext _dmsContext;
        ISettings _settings;
        public HomeController(DMSContext dMSContext, ISettings settings)
        {
            _dmsContext = dMSContext;
            _settings = settings;
        }
        public IActionResult Index()
        {
            var idFileState = _dmsContext.FileStates.Where(s => s.Name == "new").FirstOrDefault();
            var files = _dmsContext.Files.Where(f => f.State == idFileState.Id).ToList();
            @ViewData["Sitetitle"] = "Home";
            return View(files);
        }

        public IActionResult All()
        {
            var files = _dmsContext.Files.ToList();
            @ViewData["Sitetitle"] = "All";
            return View("Index", files);
        }
    }
}
