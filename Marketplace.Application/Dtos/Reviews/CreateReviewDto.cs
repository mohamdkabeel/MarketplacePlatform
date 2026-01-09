using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Application.Dtos.Reviews
{
    public class CreateReviewDto
    {
        public int ProductId { get; set; }
        public int Rating { get; set; }   // 1-5
        public string Comment { get; set; } = string.Empty;
    }

}
