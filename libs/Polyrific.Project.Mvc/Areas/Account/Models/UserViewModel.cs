using Polyrific.Project.Mvc.Models;

namespace Polyrific.Project.Mvc.Areas.Account.Models
{
    public class UserViewModel : BaseViewModel
    {
        public bool IsActive { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
