using System;
using System.Collections.Generic;

namespace CoreDMS.Model
{
    public partial class Tag
    {
        public Tag()
        {
            FileTag = new HashSet<FileTag>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }

        public ICollection<FileTag> FileTag { get; set; }
    }
}
