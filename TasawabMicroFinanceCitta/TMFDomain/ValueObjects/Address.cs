using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMFDomain.ValueObjects
{
    public class Address
    {
        public string Street { get; }
        public string City { get; }
        public string country { get; }

        public Address(string street, string city, string zipCode)
        {
            Street = street;
            City = city;
            country = zipCode;
        }

        public override string ToString() => $"{Street}, {City}, {country}";
    }
}
