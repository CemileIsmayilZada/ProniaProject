using Core.Entities;
using System.Linq;

namespace WebUI.Utilities
{
    public static class Extensions
    {

        public static bool CheckFileSize(this IFormFile file, int kbyte)
        {
            return file.Length / 1024 < kbyte;
        }
        public static bool CheckFileFormat(this IFormFile file, string fileFormat)
        {
            return file.ContentType.Contains(fileFormat);
        }
        public static async Task<string> CopyFileAsync(this IFormFile file, string wwwroot, params string[] folders)
        {
            try
            {
                var fileName = Guid.NewGuid() + file.FileName;
                var resultpath = wwwroot;
                foreach (var folder in folders)
                {
                    resultpath = Path.Combine(resultpath, folder);
                }
                using (FileStream fs = new FileStream(resultpath, FileMode.Create))
                {
                    await file.CopyToAsync(fs);
                }
                return resultpath;
            }
            catch (Exception)
            {
                throw;
            }


        }
    }
}
