using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calca.Domain.Users
{
    public interface IUserRepository
    {
        Task<User> GetByEmail(string email, CancellationToken ct);

        Task<long> Create(User user, CancellationToken ct);
    }
}
