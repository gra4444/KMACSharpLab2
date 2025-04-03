using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMA.Krachylo.Lab2.Exceptions
{
    internal class TooOldBirthDateException : ArgumentException
    {
        public TooOldBirthDateException() : base("Birth date is unrealistically far in the past.") { }

        public TooOldBirthDateException(string message) : base(message) { }
    }
}
