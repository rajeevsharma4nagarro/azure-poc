using Microsoft.AspNetCore.Mvc;

namespace SCD.Services.ProductAPI.Utility.IUtility
{
    public interface IUploadFilesToBlob
    {
        Task<string> UploadToBlob(IFormFile file);
    }
}
