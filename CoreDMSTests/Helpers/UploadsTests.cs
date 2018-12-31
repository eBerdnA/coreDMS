using CoreDMS;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;

namespace CoreDMSTests.Helpers
{
    public class UploadsTests
    {
        bool isWindows;
        public UploadsTests()
        {
            isWindows = System.Runtime.InteropServices.RuntimeInformation
                                               .IsOSPlatform(OSPlatform.Windows);
        }
        [Fact]
        public void CreateLocationPathTest()
        {
            if (isWindows)
            {
                string uploadPath = @"C:\CoreDMS\uploads\";
                string filename = "test1.pdf";

                string filePath = CoreDMS.Helpers.Uploads.CreateLocationPath(uploadPath, filename);

                Assert.DoesNotContain(@"\\", filePath);
            }
            else
            {
                string uploadPath = @"/CoreDMS/uploads\";
                string filename = "test1.pdf";

                string filePath = CoreDMS.Helpers.Uploads.CreateLocationPath(uploadPath, filename);

                Assert.DoesNotContain(@"//", filePath);
            }
        }

        [Fact]
        /// <summary>
        /// Checks whether the path does contain the DirectorySeparatorChar twice
        /// </summary>
        public void CreateProcessedLocationDoubleBackSlashTest()
        {
            if (isWindows)
            {
                DateTime processTime = DateTime.UtcNow;
                string fileProcessTime = processTime.ToString(Constants.FileProcessTimeDateFormat);
                string processedPath = @"C:\CoreDMS\processed\";
                string filename = "test1.pdf";
                string targetFileName = CoreDMS.Helpers.Uploads.CreateProcessedLocation(processedPath, fileProcessTime, filename);

                Assert.DoesNotContain(@"\\", targetFileName);
            }
            else
            {
                DateTime processTime = DateTime.UtcNow;
                string fileProcessTime = processTime.ToString(Constants.FileProcessTimeDateFormat);
                string processedPath = @"/CoreDMS/processed/";
                string filename = "test1.pdf";
                string targetFileName = CoreDMS.Helpers.Uploads.CreateProcessedLocation(processedPath, fileProcessTime, filename);

                Assert.DoesNotContain(@"//", targetFileName);
            }
        }

        [Fact]
        public void CreateProcessedLocationTest()
        {
            if (isWindows)
            {
                DateTime processTime = DateTime.UtcNow;
                string fileProcessTime = processTime.ToString(Constants.FileProcessTimeDateFormat);
                string processedPath = @"C:\CoreDMS\processed\";
                string filename = "test1.pdf";
                string targetFileName = CoreDMS.Helpers.Uploads.CreateProcessedLocation(processedPath, fileProcessTime, filename);

                Assert.Equal(processedPath + fileProcessTime + "_" + filename, targetFileName);
            }
            else
            {
                DateTime processTime = DateTime.UtcNow;
                string fileProcessTime = processTime.ToString(Constants.FileProcessTimeDateFormat);
                string processedPath = @"/CoreDMS/processed/";
                string filename = "test1.pdf";
                string targetFileName = CoreDMS.Helpers.Uploads.CreateProcessedLocation(processedPath, fileProcessTime, filename);

                Assert.Equal(processedPath + fileProcessTime + "_" + filename, targetFileName);
            }
        }
    }
}
