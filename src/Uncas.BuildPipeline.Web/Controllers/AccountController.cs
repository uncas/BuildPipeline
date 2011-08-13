namespace Uncas.BuildPipeline.Web.Controllers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;
    using Uncas.BuildPipeline.Web.Models;

    /// <summary>
    /// Handles login etc.
    /// </summary>
    [HandleError]
    public class AccountController : Controller
    {
        public IFormsAuthenticationService FormsService { get; set; }
        
        public IMembershipService MembershipService { get; set; }

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }

                ModelState.AddModelError(
                    string.Empty,
                    "The current password is incorrect or the new password is invalid.");
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOn()
        {
            return View();
        }

        [SuppressMessage(
            "Microsoft.Design",
            "CA1054:UriParametersShouldNotBeStrings",
            MessageId = "1#",
            Justification = "Action parameter.")]
        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (model != null && ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "The user name or password provided is incorrect.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Register()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus =
                    MembershipService.CreateUser(model.UserName, model.Password, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, AccountValidation.ErrorCodeToString(createStatus));
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null)
            {
                FormsService = new FormsAuthenticationService();
            }

            if (MembershipService == null)
            {
                MembershipService = new AccountMembershipService();
            }

            base.Initialize(requestContext);
        }
    }
}