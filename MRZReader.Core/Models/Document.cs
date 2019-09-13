using System;

namespace MRZReader.Core
{
    public class Document
    {
        // Primary Key
        public int DocumentId { get; set; }
        public string DocumentExtension { get; set; }
        public string DocumentLocation { get; set; }
        public int DocumentOcrId { get; set; } = 0;
        public string ReadableLine1 { get; set; }
        public string ReadableLine2 { get; set; }
        public string ReadableLine3 { get; set; }
        public string Checksum { get; set; }
        public bool ChecksumVerified { get; set; } = false;
        public string DocumentType { get; set; }
        public string DocumentSubtype { get; set; }
        public string IssuingCountry { get; set; }
        public string DocumentNumber { get; set; }
        public bool DocumentNumberVerified { get; set; } = false;
        public bool DocumentNumberCheck { get; set; } = false;
        public DateTime? ExpiryDate { get; set; }
        public bool ExpiryDateCheck { get; set; } = false;
        public bool ExpiryDateVerified { get; set; } = false;
        public string Nationality { get; set; }// Added to doc table as someone may have two passports with two nationalties

        public User User { get; set; } = new User();
        public int UserId { get; set; }
    }
}
