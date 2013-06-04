namespace Apistry.Samples.Application.Dto
{
    using System;

    public class AddressDto
    {
        [Writable]
        public String AddressLine1 { get; set; }

        [Writable]
        public String AddressLine2 { get; set; }

        [Writable]
        public String City { get; set; }

        [Writable]
        public String State { get; set; }

        [Writable]
        public String ZipCode { get; set; }

        [Writable]
        public String Country { get; set; }
    }
}