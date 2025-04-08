using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMFDomain.ValueObjects
{
    namespace TMFDomain.ValueObjects
    {
        public class Email
        {
            public string Value { get; }

            public Email(string value)
            {
                if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
                    throw new ArgumentException("Invalid email format.");

                Value = value;
            }

            public override string ToString() => Value;
        }
    }
}