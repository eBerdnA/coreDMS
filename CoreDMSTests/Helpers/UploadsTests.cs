using CoreDMS;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CoreDMSTests.Helpers
{
    public class UploadsTests
    {
        [Fact]
        public void CreateLocationPathTest()
        {
            string uploadPath = @"C:\CoreDMS\uploads\";
            string filename = "test1.pdf";

            string filePath = CoreDMS.Helpers.Uploads.CreateLocationPath(uploadPath, filename);

            Assert.DoesNotContain(@"\\", filePath);
        }

        [Fact]
        public void CreateProcessedLocationDoubleBackSlashTest()
        {
            DateTime processTime = DateTime.UtcNow;
            string fileProcessTime = processTime.ToString(Constants.FileProcessTimeDateFormat);
            string processedPath = @"C:\CoreDMS\processed\";
            string filename = "test1.pdf";
            string targetFileName = CoreDMS.Helpers.Uploads.CreateProcessedLocation(processedPath, fileProcessTime, filename);

            Assert.DoesNotContain(@"\\", targetFileName);
        }

        [Fact]
        public void CreateProcessedLocationTest()
        {
            DateTime processTime = DateTime.UtcNow;
            string fileProcessTime = processTime.ToString(Constants.FileProcessTimeDateFormat);
            string processedPath = @"C:\CoreDMS\processed\";
            string filename = "test1.pdf";
            string targetFileName = CoreDMS.Helpers.Uploads.CreateProcessedLocation(processedPath, fileProcessTime, filename);

            Assert.Equal(processedPath + fileProcessTime + "_" + filename, targetFileName);
        }
    }
}
