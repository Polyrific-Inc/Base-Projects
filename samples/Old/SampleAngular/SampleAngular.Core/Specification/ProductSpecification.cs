using Polyrific.Project.Core;
using SampleAngular.Core.Entities;

namespace SampleAngular.Core.Specification
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification() : base(x => true)
        {

        }
    }
}
