using Core.Entities;
using Microsoft.Extensions.Logging;
using Polyrific.Project.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services
{
    public class ReviewService : BaseService<Review>, IReviewService
    {
        public ReviewService(IRepository<Review> ReviewRepository, ILogger<ReviewService> logger,IEventStorage eventStorage)
            : base(ReviewRepository, logger, eventStorage)
        {
        }
    }
}
