using System;

namespace Api.Dto
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public double Rating { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
    }
}