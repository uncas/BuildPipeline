namespace Uncas.BuildPipeline.Repositories
{
    public class ProjectReadModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string GitRemoteUrl { get; set; }
        public string GithubUrl { get; set; }
        public string DeploymentScript { get; set; }
    }
}