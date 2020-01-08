using Polyrific.Project.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleMvc.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public void SetEntity(Product product)
        {
            this.Name = product.Name;
        }
    }
}
