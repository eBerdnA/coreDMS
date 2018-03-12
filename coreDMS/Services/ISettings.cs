using System;
using System.Collections.Generic;
using System.Text;

namespace CoreDMS.Services
{
    public interface ISettings
    {
        string GetProcessedDirectory();
        string GetUploadsDirectory();
    }
}
