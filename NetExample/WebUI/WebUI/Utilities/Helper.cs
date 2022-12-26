using static NuGet.Packaging.PackagingConstants;

namespace WebUI.Utilities
{
    public static class Helper
    {
        public static bool DeleteFile(params string[] path)
        {
            
            var resultpath = String.Empty;
            foreach (var item in path)
            {
                resultpath = Path.Combine(resultpath, item);
            }
            if (File.Exists(resultpath))
            {
               File.Delete(resultpath);
                return true;
            }

            return false;
        }

        public static string CreatePath(string wwwroot,params string[] path) {
            var resultpath = wwwroot;
            foreach (var item in path)
            {
                resultpath = Path.Combine(resultpath, item);
            }
            return resultpath;
        }
    } 
}
