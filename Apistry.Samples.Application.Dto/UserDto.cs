namespace Apistry.Samples.Application.Dto
{
    using System;
    using System.Collections.Generic;

    public class UserDto
    {
        public Int32 Id { get; set; }

        [Writable]
        public String FirstName { get; set; }

        [Writable]
        public String LastName { get; set; }

        [Writable]
        public AddressDto Address { get; set; }

        public ProfileDto Profile { get; set; }

        public IEnumerable<UserDto> Friends { get; set; }
    }
}