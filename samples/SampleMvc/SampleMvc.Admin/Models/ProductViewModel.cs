using Polyrific.Project.Mvc.Models;
using System;

namespace SampleMvc.Admin.Models
{
    public class ProductViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}
