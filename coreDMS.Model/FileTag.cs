using System;
using System.Collections.Generic;

namespace CoreDMS.Model
{
    public partial class FileTag
    {
        public long Id { get; set; }
        public long TagId { get; set; }
        public string FileId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        public Files File { get; set; }
        public Tag Tag { get; set; }
    }
}
