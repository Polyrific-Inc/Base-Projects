using System;
using System.Collections.Generic;
using System.Text;

namespace SampleAngular.Core.Exceptions
{
    public class UpdatePasswordFailedException : Exception
    {
        public int UserId { get; set; }

        public UpdatePasswordFailedException(int userId)
            : base($"Failed to update password for user \"{userId}\".")
        {
            UserId = userId;
        }

        public UpdatePasswordFailedException(int userId, Exception ex)
            : base($"Failed to update password for user \"{userId}\".", ex)
        {
            UserId = userId;
        }
    }
}
