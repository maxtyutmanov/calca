using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Infrastructure.Errors
{
    public class ConflictException : Exception
    {
        public ConflictException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
