using System;
using System.Collections.Generic;
using System.Text;

namespace CoreDMS.Model
{
    public partial class DocumentFiles
    {
        public DocumentFiles()
        {
            DocumentFileFiles = new HashSet<DocumentFileFile>();
        }
        public int? Id { get; set; }
        public string Title { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public string Note { get; set; }
        
        public ICollection<DocumentFileFile> DocumentFileFiles { get; set; }
    }
}
