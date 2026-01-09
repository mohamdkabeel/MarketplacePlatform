using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Application.IServices.FileServices
{
    public interface IFileServices
    {
        Task<string> UploadProductImageAsync(IFormFile formFile);
        Task DeleteImageAsync(string fileurl);
    }
}
