namespace IIOT_OPC.Shared.Extensions
{
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public static class Utils
    {
        public static string FileToProjectDirectory(string fileName)
        {
            return Path.Combine(TryGetSolutionFolder(), fileName);
        }

        public static string TryGetSolutionFolder()
        {
            string currentPath = GetAssemblyLocation();
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());

            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory.FullName.ToString();
        }

        public static string GetAssemblyLocation()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static string FullPath(string path, string fileName)
        {
            return Path.Combine(path, fileName);
        }
    }
}