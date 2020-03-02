using System;
using System.Collections.Generic;
using System.Text;

namespace RegalLuton.Policy.Renewals.FileService.Interfaces
{
    public interface IFileHandler
    {
        bool FileExists(string fullPath);
        bool FileExists(string folder, string fileName);
        string[] ReadAllLines(string fullPath);
        void Write(string folder, string fileName, string contents);
        string GetLetterTemplate();
    }
}