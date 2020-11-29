using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Infrastructure.Errors
{
    public class ConcurrencyConflictException : Exception
    {
        public ConcurrencyConflictException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
