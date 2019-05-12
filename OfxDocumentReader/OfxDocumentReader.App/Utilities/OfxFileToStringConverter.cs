using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OfxDocumentReader.App.Utilities
{
    public static class OfxFileToStringConverter
    {
        public static string Convert(IFormFile file)
        {
            string fileContent = string.Empty;

            // full path to file in temp location
            string filePath = Path.GetTempFileName();

            if (file.Length > 0)
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyToAsync(stream);
                }

                using (StreamReader reader = new StreamReader(file.OpenReadStream()))
                {
                    fileContent = reader.ReadToEnd();
                }
            }

            return fileContent;
        }
    }
}
