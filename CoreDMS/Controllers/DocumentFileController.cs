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
            int documentFileId = Convert.ToInt32(id);
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
    }
}