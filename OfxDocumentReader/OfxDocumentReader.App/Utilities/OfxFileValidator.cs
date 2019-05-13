using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace OfxDocumentReader.App.Utilities
{
    /// <summary>
    /// Validates ContentType for OFX files 
    /// </summary>
    public class OfxFileValidator
    {
        public OfxFileValidator() { }

        public static bool Validate(List<IFormFile> files)
        {
            foreach (IFormFile file in files)
            {
                if (file.ContentType != "application/octet-stream")
                {
                    return false;
                }
            }

            // Implement other possible validations

            return true;
        }
    }
}
