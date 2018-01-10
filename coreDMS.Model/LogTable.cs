using System;
using System.Collections.Generic;

namespace CoreDMS.Model
{
    public partial class LogTable
    {
        public long Id { get; set; }
        public long ScriptOrder { get; set; }
        public string ScriptName { get; set; }
        public string CreatedAt { get; set; }
    }
}
