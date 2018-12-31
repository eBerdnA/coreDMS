using System.IO;

namespace CoreDMS.DBCreation
{
    public class SqlFile
    {
        public SqlFile(string filePath)
        {
            FileName = filePath.Substring(filePath.LastIndexOf(Path.DirectorySeparatorChar)+1);
            FilePath = filePath;
            Order = int.Parse(FileName.Substring(0, 3));
        }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int Order { get; set; }
    }
}
