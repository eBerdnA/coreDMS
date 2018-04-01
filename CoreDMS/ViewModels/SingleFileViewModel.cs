using CoreDMS.Model;
using CoreDMS.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CoreDMS.ViewModels
{
    public class SingleFileViewModel : BaseViewModel
    {
        public SingleFileViewModel(ISettings settings) : base(settings)
        {
        }

        public Files File { get; set; }
        public string FileDate { get; set; }
        public string Tags { get; set; }
        public List<SelectListItem> FileStates { set; get; }
    }
}
