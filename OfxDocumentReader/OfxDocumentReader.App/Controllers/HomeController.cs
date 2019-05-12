using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> UploadFiles(List<IFormFile> files)
        {
            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            List<string> fileContent = new List<string>();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    using (var reader = new StreamReader(formFile.OpenReadStream()))
                    {
                        fileContent.AddRange(reader.ReadToEnd().Split("\r\n").ToList());
                    }
                }
            }

            return Ok();
        }
    }
}
