using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMA.Krachylo.Lab2.Exceptions
{
    internal class WrongEmailException : ArgumentException
    {
        public WrongEmailException() : base("Email is not valid.") { }

        public WrongEmailException(string message) : base(message) { }
    }
}
