using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polyrific.Project.Mvc
{
    public class ServicesOptions
    {
        public bool EnableIdentity { get; set; } = true;
        public bool UseDefaultUIForIdentity { get; set; } = true;
        public bool EnableSmtpEmailSender { get; set; } = true;
        public SmtpSetting SmtpSetting { get; set; }
    }
}
