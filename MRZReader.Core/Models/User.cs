using System;

namespace MRZReader.Core
{
    public class User
    {
        public int UserId { get; set; }
        public string LastName { get; set; }
        public string GivenName { get; set; }
        public string Sex { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool BirthDateVerified { get; set; } = false;
        public bool BirthDateCheck { get; set; } = false;
    }
}
