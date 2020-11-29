using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calca.Domain.Users
{
    public interface IUserService
    {
        Task<User> GetOrAddUser(ClaimsPrincipal principal, CancellationToken ct);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<User> GetOrAddUser(ClaimsPrincipal principal, CancellationToken ct)
        {
            var email = principal.FindFirst(ClaimTypes.Email)?.Value;
            var name = principal.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(name))
            {
                // TODO: typed
                throw new InvalidOperationException("Name and email are required fields");
            }

            var existingUser = await _repo.GetByEmail(email, ct);
            if (existingUser != null)
                return existingUser;

            var user = new User(email, name);
            await _repo.Create(user, ct);
            return user;
        }
    }
}
