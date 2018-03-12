using CoreDMS.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreDMS.ViewModels
{
    public class BaseViewModel
    {
        ISettings _settings;
        public BaseViewModel(ISettings settings)
        {
            _settings = settings;
        }
    }
}
