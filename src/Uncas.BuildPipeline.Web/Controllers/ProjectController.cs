using System.Web.Mvc;
using Uncas.BuildPipeline.Repositories;

namespace Uncas.BuildPipeline.Web.Controllers
{
    public class ProjectController : BaseController
    {
        private readonly IProjectReadStore _projectReadStore;

        public ProjectController(IProjectReadStore projectReadStore)
        {
            _projectReadStore = projectReadStore;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(_projectReadStore.GetProjects());
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(_projectReadStore.GetProjectById(id));
        }

        [HttpPost]
        public ActionResult Edit(ProjectReadModel model)
        {
            _projectReadStore.Update(model);
            return RedirectToAction("Index");
        }
    }
}