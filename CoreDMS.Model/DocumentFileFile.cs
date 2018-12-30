using System;
using System.Collections.Generic;

namespace CoreDMS.Model
{
    public partial class DocumentFileFile
    {
        public int Id { get; set; }
        public int DocumentFileId { get; set; }
        public string FileId { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        public Files File { get; set; }
        public DocumentFiles DocumentFile { get; set; }
    }
}
