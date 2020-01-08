using SampleMvc.Core.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleMvc.Core.Services
{
    public interface IUserService
    {
        Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure, CancellationToken cancellationToken = default(CancellationToken));

        Task<User> CreateUser(string email, string password, CancellationToken cancellationToken = default(CancellationToken));

        Task<User> CreateUser(User user, CancellationToken cancellationToken = default(CancellationToken));

        Task UpdateUser(User user, CancellationToken cancellationToken = default(CancellationToken));

        Task DeleteUser(int userId, CancellationToken cancellationToken = default(CancellationToken));

        Task<string> GenerateConfirmationToken(int userId, CancellationToken cancellationToken = default(CancellationToken));

        Task<User> GetUserById(int userId, CancellationToken cancellationToken = default(CancellationToken));

        Task<User> GetUserByEmail(string email, CancellationToken cancellationToken = default(CancellationToken));

        Task ConfirmEmail(int userId, string token, CancellationToken cancellationToken = default(CancellationToken));

        Task<int> GetUserId(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken));

        Task<string> GetUserEmail(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> ValidateUserPassword(string userName, string password, CancellationToken cancellationToken = default(CancellationToken));

        Task<User> GetUser(string userName, CancellationToken cancellationToken = default(CancellationToken));

        Task<List<User>> GetUsers(CancellationToken cancellationToken = default(CancellationToken));

        Task<string> GetResetPasswordToken(int userId, CancellationToken cancellationToken = default(CancellationToken));

        Task ResetPassword(int userId, string token, string newPassword, CancellationToken cancellationToken = default(CancellationToken));

        Task UpdatePassword(int userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default(CancellationToken));
    }
}
