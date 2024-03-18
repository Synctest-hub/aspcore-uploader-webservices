using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Inputs;
using System.Net.Http.Headers;

namespace SyncfusionCoreUploader.Controllers
{
    public class UploaderController : Controller
    {
        [DisableRequestSizeLimit]
        public void Save(IList<IFormFile> UploadFiles)
        {
            try
            {
                var saveLocation = @"E:\"; // replace with the path where you want to save the files
                foreach (var file in UploadFiles)
                {
                    var filePath = Path.Combine(saveLocation, file.FileName);

                    if (System.IO.File.Exists(filePath))
                    {
                        throw new Exception($"File {file.FileName} already exists.");
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusCode = 300;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload";
                Response.Headers.Add("Access-Control-Expose-Headers", "Error");
                Response.Headers.Add("Error", "File already exists");

            }
        }
        // to delete uploaded chunk-file from server
        public void Remove(string UploadFiles)
        {
            try
            {
                var saveLocation = @"E:\";
                var filename = Path.Combine(saveLocation, UploadFiles);
                if (System.IO.File.Exists(filename))
                {
                    System.IO.File.Delete(filename);
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 200;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File removed successfully";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
