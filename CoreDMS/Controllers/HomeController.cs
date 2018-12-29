﻿using System;
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
    public class HomeController : Controller
    {
        DMSContext _dmsContext;
        ISettings _settings;
        Microsoft.Extensions.Logging.ILogger _logger;
        public HomeController(DMSContext dMSContext, ISettings settings, ILogger<HomeController> logger)
        {
            _dmsContext = dMSContext;
            _settings = settings;
            _logger = logger;
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

        public IActionResult About()
        {            
            @ViewData["Sitetitle"] = "About";
            Version version = typeof(Program).Assembly.GetName().Version;
            @ViewData["Version"] = version;
            _logger.LogInformation($"version: {version}");
            return View();
        }
    }
}
