using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoreDMS.Helpers
{
    public static class Uploads
    {
        public static string CreateLocationPath(string uploadPath, string filename)
        {
            return Path.Combine(uploadPath, filename);
        }

        public static string CreateProcessedLocation(string processedPath, string fileProcessTime, string filename)
        {
            return Path.Combine(processedPath, fileProcessTime + "_" + filename);
        }
    }
}
