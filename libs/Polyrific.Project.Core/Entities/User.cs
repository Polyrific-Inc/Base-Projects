namespace Polyrific.Project.Core.Entities
{
    public class User : BaseEntity
    {
        public bool IsActive { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
