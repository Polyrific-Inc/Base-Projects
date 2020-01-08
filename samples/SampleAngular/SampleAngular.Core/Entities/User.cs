﻿using Polyrific.Project.Core;

namespace SampleAngular.Core.Entities
{
    public class User : BaseEntity
    {
        public bool IsActive { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
    }
}
