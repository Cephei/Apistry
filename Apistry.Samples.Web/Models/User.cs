using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apistry.Samples.Web.Models
{
    public class User
    {
        public Int32 Id { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public Address Address { get; set; }

        public Profile Profile { get; set; }

        public IEnumerable<User> Friends { get; set; }
    }
}