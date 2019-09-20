namespace Polyrific.Project.Core
{
    public class SignInResult
    {
        public bool Succeeded { get; set; }

        public bool IsLockedOut { get; set; }

        public bool IsNotAllowed { get; set; }

        public bool RequiresTwoFactor { get; set; }
    }
}
