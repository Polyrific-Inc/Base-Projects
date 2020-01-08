///OpenCatapultModelId:109
using System;
using System.Collections.Generic;

namespace SampleMvc.Admin.Models
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
