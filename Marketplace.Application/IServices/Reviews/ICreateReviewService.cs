using Marketplace.Application.Dtos.Reviews;
using Marketplace.Core.Entites.review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Application.IServices.Reviews
{
    public interface IReviewService
    {
        Task<Review> AddReviewAsync(string userId, CreateReviewDto dto);
        Task<IEnumerable<Review>> GetReviewsByProductAsync(int productId);
    }

}
