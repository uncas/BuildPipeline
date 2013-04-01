using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Uncas.BuildPipeline.Utilities;

namespace Uncas.BuildPipeline.Web.Controllers
{
    public class CustomApiController : Controller
    {
        [HttpPost]
        public ActionResult Packages(string id)
        {
            string packageFolder = DeploymentUtility.PackageFolder;
            HttpFileCollectionBase files = Request.Files;
            if (files.Count == 0)
                throw new InvalidOperationException("No file...");
            HttpPostedFileBase file = files[0];
            if (file == null)
                throw new InvalidOperationException("No file...");
            if (!Directory.Exists(packageFolder))
                Directory.CreateDirectory(packageFolder);
            string newFilePath = Path.Combine(packageFolder, file.FileName);
            file.SaveAs(newFilePath);
            return Content("OK " + file.FileName);
        }
    }
}