using System;
using System.Collections.Generic;
using System.Text;

namespace MyNotes.Application.SharedKernel
{
    /// <summary>
    /// Thrown whenever caller specifies invalid inputs for a command or query.
    /// </summary>
    public class InvalidCommandOrQueryException : Exception
    {
        public InvalidCommandOrQueryException(string message)
            : base(message)
        {
        }
    }
}
