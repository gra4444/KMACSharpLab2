using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMA.Krachylo.Lab2.Exceptions
{
    internal class InvalidNameException : ArgumentException
    {
        public InvalidNameException() : base("Name is not valid.") { }

        public InvalidNameException(string message) : base(message) { }
    }
}
