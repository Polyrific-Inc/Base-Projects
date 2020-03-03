using Polyrific.Project.Core;

namespace Core.Entities
{
    public class Product : BaseEntity<Product>
    {
        public string Name { get; set; }

        public override void UpdateValueFrom(Product source)
        {
            Name = source.Name;
        }
    }
}