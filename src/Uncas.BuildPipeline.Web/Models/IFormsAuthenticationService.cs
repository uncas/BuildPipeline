namespace Uncas.BuildPipeline.Web.Models
{
    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        
        void SignOut();
    }
}