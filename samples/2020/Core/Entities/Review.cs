using Polyrific.Project.Core;

namespace Core.Entities
{
    public class Review : BaseEntity
    {
        public double Rating { get; set; }

        public override void UpdateValueFrom(BaseEntity source)
        {
            Rating = ((Review)source).Rating;
        }
    }
}
