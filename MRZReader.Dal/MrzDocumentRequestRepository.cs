//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Runtime.InteropServices.WindowsRuntime;
//using Microsoft.EntityFrameworkCore;
//using MRZReader.Core;

//namespace MRZReader.Dal
//{
//    public class MrzDocumentRequestRepository : IMrzDocumentRequestRepository
//    {
//        private readonly MrzReaderDbContext _context;

//        public MrzDocumentRequestRepository(MrzReaderDbContext context)
//        {
//            this._context = context;
//        }
//        public DocumentRequest Add(DocumentRequest request)
//        {
//            using (var transaction = _context.Database.BeginTransaction())
//            {
//                try
//                {
//                    _context.Document.Add(new Document()
//                    {
//                        DocumentExtension = request.Document.DocumentExtension,
//                        DocumentLocation = request.Document.DocumentLocation,
//                        DocumentOcrId = request.Document.DocumentOcrId,
//                        ReadableLine1 = request.Document.ReadableLine1,
//                        ReadableLine2 = request.Document.ReadableLine2,
//                        ReadableLine3 = request.Document.ReadableLine3,
//                        Checksum = request.Document.Checksum,
//                        ChecksumVerified = request.Document.ChecksumVerified,
//                        DocumentType = request.Document.DocumentType,
//                        DocumentSubtype = request.Document.DocumentSubtype,
//                        IssuingCountry = request.Document.IssuingCountry,
//                        DocumentNumber = request.Document.DocumentNumber,
//                        DocumentNumberVerified = request.Document.DocumentNumberVerified,
//                        DocumentNumberCheck = request.Document.DocumentNumberCheck,
//                        ExpiryDate = request.Document.ExpiryDate,
//                        ExpiryDateCheck = request.Document.ExpiryDateCheck,
//                        ExpiryDateVerified = request.Document.ExpiryDateVerified,
//                        Nationality = request.Document.Nationality,

//                        User=new User()
//                        {
//                            LastName = request.Document.User.LastName,
//                            GivenName = request.Document.User.GivenName,
//                            Sex = request.Document.User.Sex,
//                            BirthDate = request.Document.User.BirthDate,
//                            BirthDateVerified = request.Document.User.BirthDateVerified,
//                            BirthDateCheck = request.Document.User.BirthDateCheck
//                        }
//                    });
//                    _context.SaveChanges();

//                    // Commit transaction if all commands succeed, transaction will auto-rollback
//                    // when disposed if either commands fails
//                    transaction.Commit();
//                }
//                catch (Exception)
//                {
//                    request.IsSuccessed = false;
//                }
//            }
//            return request;
//        }
//        public IEnumerable<Document> GetAllDocuments()
//        {
//            return _context.Document.Include("User").ToList();
//        }

//        public IEnumerable<DocumentRequest> GetAll()
//        {
//            throw new NotImplementedException();
//        }
//        public DocumentRequest GetById(int id)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
