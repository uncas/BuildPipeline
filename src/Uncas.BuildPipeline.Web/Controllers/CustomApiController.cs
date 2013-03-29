using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Uncas.BuildPipeline.Web.Controllers
{
    public class CustomApiController : Controller
    {
        public static readonly string PackageFolder =
            ConfigurationManager.AppSettings["DeploymentPackageFolder"];

        [HttpPost]
        public ActionResult Packages(string id)
        {
            HttpFileCollectionBase files = Request.Files;
            if (files.Count == 0)
                throw new InvalidOperationException("No file...");
            HttpPostedFileBase file = files[0];
            if (file == null)
                throw new InvalidOperationException("No file...");
            if (!Directory.Exists(PackageFolder))
                Directory.CreateDirectory(PackageFolder);
            string newFilePath = Path.Combine(PackageFolder, file.FileName);
            file.SaveAs(newFilePath);
            return Content("OK " + file.FileName);
        }
    }
}