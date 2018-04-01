using CoreDMS.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreDMS.Services
{
    public class Settings : ISettings
    {
        private readonly string UploadDirectory = string.Empty;
        private readonly string ProcessedDirectory = string.Empty;

        public Settings(string uploadDirectory, string processedDirectory)
        {
            UploadDirectory = uploadDirectory;
            ProcessedDirectory = processedDirectory;
        }

        public string GetProcessedDirectory()
        {
            return ProcessedDirectory;
        }

        public string GetUploadsDirectory()
        {
            return UploadDirectory;
        }
    }
}
