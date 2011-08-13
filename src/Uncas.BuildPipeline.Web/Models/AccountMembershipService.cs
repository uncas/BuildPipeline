namespace Uncas.BuildPipeline.Web.Models
{
    using System;
    using System.Web.Security;

    /// <summary>
    /// Handles membership.
    /// </summary>
    public class AccountMembershipService : IMembershipService
    {
        private readonly MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength
        {
            get { return _provider.MinRequiredPasswordLength; }
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "userName");
            }

            if (string.IsNullOrEmpty(oldPassword))
            {
                throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            }
            
            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentException("Value cannot be null or empty.", "newPassword");
            }

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "userName");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Value cannot be null or empty.", "password");
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Value cannot be null or empty.", "email");
            }

            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public bool ValidateUser(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "userName");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Value cannot be null or empty.", "password");
            }

            return _provider.ValidateUser(userName, password);
        }
    }
}