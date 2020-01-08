using Polyrific.Project.Core;
using SampleMvc.Core.Entities;

namespace SampleMvc.Core.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification() : base(x => true)
        {

        }
    }
}
