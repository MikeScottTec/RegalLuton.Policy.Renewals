using System.IO;
using System.Reflection;
using RegalLuton.Policy.Renewals.FileService.Interfaces;

namespace RegalLuton.Policy.Renewals.FileService.Implementation
{
    public class FileHandler : IFileHandler
    {
        public bool FileExists(string fullPath)
        {
            return File.Exists(fullPath);
        }

        public bool FileExists(string folder, string fileName)
        {
            string fullPath = FullPath(folder, fileName);
            return File.Exists(fullPath);
        }

        public string[] ReadAllLines(string fullPath)
        {
            return File.ReadAllLines(fullPath);
        }

        public void Write(string folder, string fileName, string contents)
        {
            string fullPath = FullPath(folder, fileName);
            File.WriteAllText(fullPath, contents);
        }

        public string GetLetterTemplate()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("RegalLuton.Policy.Renewals.FileService.Resources.template.txt"))
            {
                TextReader tr = new StreamReader(stream);
                return tr.ReadToEnd();
            }
        }

        private string FullPath(string folder, string fileName)
        {
            return Path.Combine(folder, fileName);
        }
    }
}