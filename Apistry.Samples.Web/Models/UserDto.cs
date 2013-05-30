using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apistry.Samples.Web.Models
{
    public class UserDto
    {
        public Int32 Id { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public AddressDto Address { get; set; }

        public ProfileDto Profile { get; set; }

        public IEnumerable<UserDto> Friends { get; set; }
    }
}