using System;
using System.Collections.Generic;

namespace CoreDMS.Model
{
    public partial class Files
    {
        public Files()
        {
            FileTag = new HashSet<FileTag>();
        }

        public string Id { get; set; }
        public string Filename { get; set; }
        public string Hash { get; set; }
        public string DocumentDate { get; set; }
        public long State { get; set; }
        public string Location { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }

        public ICollection<FileTag> FileTag { get; set; }
        public ICollection<DocumentFileFile> DocumentFileFile { get; set; }
    }
}
