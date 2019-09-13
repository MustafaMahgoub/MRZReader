using MRZReader.Core;
using System.Xml;

namespace MRZReader.DataExtractor
{
    public class XmlDataExtractor : IDataExtractor
    {
        public DocumentRequest Extract(DocumentRequest request)
        {
            var path = request.Document.TargetFilePath;
            XmlDocument doc = new XmlDocument();
            doc.Load(path);

            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                string text = node.InnerText;
                string attr = node.Attributes["type"]?.InnerText;

                switch (attr)
                {
                    // USER DATA
                    case DataFieldsHelper.LastName: request.Document.User.LastName = text; break;
                    case DataFieldsHelper.GivenName: request.Document.User.GivenName = text; break;
                    case DataFieldsHelper.Sex: request.Document.User.Sex = text; break;
                    case DataFieldsHelper.BirthDate: request.Document.User.BirthDate = text.ConvertToDateTimeSafely(); break;
                    case DataFieldsHelper.BirthDateVerified: request.Document.User.BirthDateVerified = text.ConvertToBoolSafely(); break;
                    case DataFieldsHelper.BirthDateCheck: request.Document.User.BirthDateCheck = text.ConvertToBoolSafely(); break;

                    // DOCUMENT DATA
                    case DataFieldsHelper.DocumentId: request.Document.DocumentOcrId = text.ConvertToIntSafely(); break;
                    case DataFieldsHelper.ReadableLine1: request.Document.ReadableLine1 = text; break;
                    case DataFieldsHelper.ReadableLine2: request.Document.ReadableLine2 = text; break;
                    case DataFieldsHelper.ReadableLine3: request.Document.ReadableLine3 = text; break;
                    case DataFieldsHelper.Checksum: request.Document.Checksum = text; break;
                    case DataFieldsHelper.ChecksumVerified: request.Document.ChecksumVerified = text.ConvertToBoolSafely(); break;
                    case DataFieldsHelper.DocumentType: request.Document.DocumentType = text; break;
                    case DataFieldsHelper.DocumentSubtype: request.Document.DocumentSubtype = text; break;
                    case DataFieldsHelper.IssuingCountry: request.Document.IssuingCountry = text; break;
                    case DataFieldsHelper.DocumentNumber: request.Document.DocumentNumber = text; break;
                    case DataFieldsHelper.DocumentNumberVerified: request.Document.DocumentNumberVerified = text.ConvertToBoolSafely(); break;
                    case DataFieldsHelper.DocumentNumberCheck: request.Document.DocumentNumberCheck = text.ConvertToBoolSafely(); break;
                    case DataFieldsHelper.ExpiryDate: request.Document.ExpiryDate = text.ConvertToDateTimeSafely(); break;
                    case DataFieldsHelper.ExpiryDateCheck: request.Document.ExpiryDateCheck = text.ConvertToBoolSafely(); break;
                    case DataFieldsHelper.ExpiryDateVerified: request.Document.ExpiryDateVerified = text.ConvertToBoolSafely(); break;
                    case DataFieldsHelper.Nationality: request.Document.Nationality = text; break;
                }
            }
            return request;
        }
    }
}
