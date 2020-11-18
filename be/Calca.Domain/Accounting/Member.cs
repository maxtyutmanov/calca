using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Domain.Accounting
{
    public class Member
    {
        public int UserId { get; set; }

        public string Name { get; set; }

        public Member(int userId, string name)
        {
            UserId = userId;
            Name = name;
        }
    }
}
