using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Polyrific.Project.Core;
using SampleAngular.Core.Entities;
using SampleAngular.Core.Repositories;
using SampleAngular.Data.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleAngular.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;

        public UserRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<Core.Entities.SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent, lockoutOnFailure);
            return new Core.Entities.SignInResult
            {
                Succeeded = result.Succeeded,
                RequiresTwoFactor = result.RequiresTwoFactor,
                IsLockedOut = result.IsLockedOut,
                IsNotAllowed = result.IsNotAllowed
            };
        }

        public async Task ConfirmEmail(int userId, string token, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (!result.Succeeded)
                    result.ThrowErrorException();
            }
        }

        public Task<int> CountBySpec(ISpecification<User> spec, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> Create(User entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = _mapper.Map<ApplicationUser>(entity);

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                result.ThrowErrorException();

            return user.Id;
        }

        public async Task<int> Create(User entity, string password, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = _mapper.Map<ApplicationUser>(entity);

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                result.ThrowErrorException();

            return user.Id;
        }

        public async Task Delete(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                    result.ThrowErrorException();
            }
        }

        public async Task<string> GenerateConfirmationToken(int userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
                return await _userManager.GenerateEmailConfirmationTokenAsync(user);

            return "";
        }

        public async Task<List<User>> GetUsers(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var users = await _userManager.Users.ToListAsync();

            return _mapper.Map<List<User>>(users);
        }

        public async Task<User> GetById(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.Users.Include("Roles.Role").FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
            if (user != null)
                return _mapper.Map<User>(user);

            return await Task.FromResult((User)null);
        }

        public async Task<User> GetByEmail(string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
            if (user != null)
                return _mapper.Map<User>(user);

            return await Task.FromResult((User)null);
        }

        public async Task<User> GetByPrincipal(ClaimsPrincipal principal, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var appUser = await _userManager.FindByNameAsync(principal.Identity.Name);

            return _mapper.Map<User>(appUser);
        }

        public async Task SetUserRole(int userId, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                    throw new ArgumentException($"Role {roleName} not found");

                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains(roleName))
                {
                    var result = await _userManager.RemoveFromRolesAsync(user, roles);
                    if (!result.Succeeded)
                        result.ThrowErrorException();

                    result = await _userManager.AddToRoleAsync(user, roleName);
                    if (!result.Succeeded)
                        result.ThrowErrorException();
                }
            }
        }

        public Task<IEnumerable<User>> GetBySpec(ISpecification<User> spec, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetSingleBySpec(ISpecification<User> spec, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> GetByUserName(string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var appUser = await _userManager.Users.Include("Roles.Role").FirstOrDefaultAsync(u => u.UserName == userName);

            return _mapper.Map<User>(appUser);
        }

        public async Task<bool> ValidateUserPassword(string userName, string password, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByNameAsync(userName);
            if (user != null && user.EmailConfirmed)
                return await _userManager.CheckPasswordAsync(user, password);

            return false;
        }

        public async Task Update(User entity, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == entity.Id);
            if (user != null)
            {
                user = _mapper.Map(entity, user);
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task<List<User>> GetAll(CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var users = await _userManager.Users.ToListAsync();

            return _mapper.Map<List<User>>(users);
        }

        public async Task<string> GetResetPasswordToken(int userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task ResetPassword(int userId, string token, string newPassword, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
                result.ThrowErrorException();
        }

        public async Task UpdatePassword(int userId, string oldPassword, string newPassword, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
                result.ThrowErrorException();
        }
    }
}
