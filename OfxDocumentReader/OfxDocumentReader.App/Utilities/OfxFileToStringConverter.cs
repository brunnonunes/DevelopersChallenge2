using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace OfxDocumentReader.App.Utilities
{
    public static class OfxFileToStringConverter
    {
        /// <summary>
        /// Converts the OFX file into one string.
        /// </summary>
        /// <param name="file">OFX file.</param>
        /// <returns></returns>
        public static string Convert(IFormFile file)
        {
            string fileContent = string.Empty;

            try
            {
                // Get the full path to file in temp location
                string filePath = Path.GetTempFileName();

                if (file.Length > 0)
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        // Copy the file to this file stream
                        file.CopyToAsync(stream);
                    }

                    using (StreamReader reader = new StreamReader(file.OpenReadStream()))
                    {
                        // Reads the content of the file
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return fileContent;
        }
    }
}
