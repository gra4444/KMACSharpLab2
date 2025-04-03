using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMA.Krachylo.Lab2.Exceptions
{
    internal class FutureBirthDateException : ArgumentException
    {
        public FutureBirthDateException() : base("Birth date cannot be in the future.") { }

        public FutureBirthDateException(string message) : base(message) { }
    }
}
