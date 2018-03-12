using System;
using System.Collections.Generic;

namespace CoreDMS.Model
{
    public partial class Upload
    {
        public Upload()
        {
            UploadError = new HashSet<UploadError>();
        }
        public long Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string CreatedAt { get; set; }
        public ICollection<UploadError> UploadError { get; set; }
    }
}
