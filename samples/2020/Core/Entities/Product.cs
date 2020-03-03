using Polyrific.Project.Core;

namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public override void UpdateValueFrom(BaseEntity source)
        {
            Name = ((Product)source).Name;
        }
    }
}