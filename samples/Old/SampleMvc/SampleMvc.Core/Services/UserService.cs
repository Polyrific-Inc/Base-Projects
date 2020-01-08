using SampleMvc.Core.Entities;
using SampleMvc.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleMvc.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return _userRepository.PasswordSignInAsync(email, password, isPersistent, lockoutOnFailure, cancellationToken);
        }

        public async Task ConfirmEmail(int userId, string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _userRepository.ConfirmEmail(userId, token, cancellationToken);
        }

        public async Task<User> CreateUser(string email, string password, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = new User
            {
                UserName = email,
                Email = email
            };

            var id = await _userRepository.Create(user, password, cancellationToken);
            if (id > 0)
                user.Id = id;

            return user;
        }

        public async Task<User> CreateUser(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var id = await _userRepository.Create(user, null, cancellationToken);
            if (id > 0)
                user.Id = id;

            return user;
        }

        public async Task DeleteUser(int userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _userRepository.Delete(userId, cancellationToken);
        }

        public Task<string> GenerateConfirmationToken(int userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return _userRepository.GenerateConfirmationToken(userId, cancellationToken);
        }

        public async Task<User> GetUser(string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _userRepository.GetByUserName(userName, cancellationToken);
        }

        public async Task<User> GetUserById(int userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _userRepository.GetById(userId, cancellationToken);
        }

        public async Task<User> GetUserByEmail(string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _userRepository.GetByEmail(email, cancellationToken);
        }

        public async Task<int> GetUserId(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userRepository.GetByPrincipal(principal, cancellationToken);

            return user?.Id ?? 0;
        }

        public async Task<string> GetUserEmail(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userRepository.GetByPrincipal(principal, cancellationToken);

            return user?.Email;
        }

        public async Task<List<User>> GetUsers(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _userRepository.GetUsers();
        }

        public async Task ResetPassword(int userId, string token, string newPassword, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _userRepository.ResetPassword(userId, token, newPassword, cancellationToken);
        }

        public async Task<string> GetResetPasswordToken(int userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await _userRepository.GetResetPasswordToken(userId, cancellationToken);
        }

        public async Task UpdatePassword(int userId, string oldPassword, string newPassword, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            await _userRepository.UpdatePassword(userId, oldPassword, newPassword, cancellationToken);
        }

        public async Task UpdateUser(User user, CancellationToken cancellationToken = default(CancellationToken))
        {
            await _userRepository.Update(user, cancellationToken);
        }

        public async Task<bool> ValidateUserPassword(string userName, string password, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _userRepository.ValidateUserPassword(userName, password, cancellationToken);
        }
    }
}
