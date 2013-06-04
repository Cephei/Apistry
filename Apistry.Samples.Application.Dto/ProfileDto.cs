namespace Apistry.Samples.Application.Dto
{
    using System;

    public class ProfileDto
    {
        public Int32 UserId { get; set; }

        public Byte[] Picture { get; set; }

        [Writable]
        public String About { get; set; }
    }
}