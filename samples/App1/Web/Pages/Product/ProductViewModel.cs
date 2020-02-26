using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Product
{
    public class ProductViewModel : BaseViewModel
    {
        [Required]
        public string Name { get; set; }
        public override string DisplayName => Name;
    }
}
