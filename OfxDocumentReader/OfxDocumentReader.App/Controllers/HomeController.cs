using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using OfxDocumentReader.App.Models;

namespace OfxDocumentReader.App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new List<TransactionViewModel>());
        }
    }
}
