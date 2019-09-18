///OpenCatapultModelId:111
using System;
using System.Collections.Generic;

namespace SampleMvc.Admin.Models
{
    public class ProductViewModel : BaseViewModel
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
    }
}
