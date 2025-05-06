using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Common.Services.Attachments
{
    public class AttachmentServices : IAttachmentServices
    {
        private readonly List<string> AllowedExtentions = new List<string>() { ".jpg", ".png", ".jpeg" };
        private const int FileMaximumSiz = 2_097_152;
        public string UplodImage(IFormFile File, string FolderName)
        {
            var fileExtention = Path.GetExtension(File.FileName);
            if (!AllowedExtentions.Contains(fileExtention))
                throw new Exception("Invalid File Extention");
            if (File.Length > FileMaximumSiz)
                throw new Exception("Invalid File Size, Over our Range!!");

            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", FolderName);
            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            var FileName = $"{Guid.NewGuid()}_{File.FileName}";
            var FilPath = Path.Combine(FolderPath, FileName);
            using var fs = new FileStream(FilPath, FileMode.Create);
            File.CopyTo(fs);
            return FileName;

        }
        public bool DeleteImage(string FilePath)
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
                return true;
            }
            return false;
        }

    }
}
