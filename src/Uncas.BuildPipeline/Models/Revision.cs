namespace Uncas.BuildPipeline.Models
{
    public class Revision
    {
        public static string Short(string revision)
        {
            return revision.Substring(0, 10);
        }
    }
}