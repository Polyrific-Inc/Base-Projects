using Polyrific.Project.Core;

namespace SampleAngular.Core.Entities
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
