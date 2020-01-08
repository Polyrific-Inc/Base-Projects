using Polyrific.Project.Core;
using SampleAngular.Core.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SampleAngular.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure, CancellationToken cancellationToken = default(CancellationToken));

        Task<User> GetByUserName(string userName, CancellationToken cancellationToken = default(CancellationToken));

        Task<User> GetByEmail(string email, CancellationToken cancellationToken = default(CancellationToken));

        Task<int> Create(User entity, string password, CancellationToken cancellationToken = default(CancellationToken));

        Task<string> GenerateConfirmationToken(int userId, CancellationToken cancellationToken = default(CancellationToken));

        Task ConfirmEmail(int userId, string token, CancellationToken cancellationToken = default(CancellationToken));

        Task<User> GetByPrincipal(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken));

        Task<bool> ValidateUserPassword(string userName, string password, CancellationToken cancellationToken = default(CancellationToken));

        Task<List<User>> GetUsers(CancellationToken cancellationToken = default(CancellationToken));

        Task<string> GetResetPasswordToken(int userId, CancellationToken cancellationToken = default(CancellationToken));

        Task ResetPassword(int userId, string token, string newPassword, CancellationToken cancellationToken = default(CancellationToken));

        Task UpdatePassword(int userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default(CancellationToken));

        Task SetUserRole(int userId, string roleName, CancellationToken cancellationToken = default(CancellationToken));
    }
}
