﻿using Microsoft.AspNetCore.Mvc;

namespace NPAU.Controllers
{
    [Area("Admin")]
    public class TermController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}