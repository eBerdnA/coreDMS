using System;
using System.Collections.Generic;

namespace CoreDMS.Model
{
    public partial class UploadError
    {
        public long Id { get; set; }
        public long UploadId { get; set; }
        public string Error { get; set; }
        public string CreatedAt { get; set; }
        public Upload Upload { get; set; }
    }
}
