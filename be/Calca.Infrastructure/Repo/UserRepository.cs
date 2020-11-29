using Calca.Domain.Users;
using Calca.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calca.Infrastructure.Repo
{
    public class UserRepository : IUserRepository
    {
        private readonly CalcaDbContext _ctx;

        public UserRepository(CalcaDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<long> Create(User user, CancellationToken ct)
        {
            _ctx.Add(user);
            await _ctx.SaveChangesAsync(ct);
            return user.Id;
        }

        public Task<User> GetByEmail(string email, CancellationToken ct)
        {
            var user = _ctx.Users.FirstOrDefaultAsync(x => x.Email == email, ct);
            return user;
        }
    }
}
