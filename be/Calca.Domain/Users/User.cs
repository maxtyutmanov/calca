using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Domain.Users
{
    public class User
    {
        public long Id { get; private set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public User(string email, string name)
        {
            Name = name;
            Email = email;
        }

        public User(long id, string email, string name)
            : this(email, name)
        {
            Id = id;
        }

        private User() { }
    }
}
