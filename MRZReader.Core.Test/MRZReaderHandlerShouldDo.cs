//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using NSubstitute;
//using System;
//using Microsoft.AspNetCore.Http;

//namespace MRZReader.Core.Test
//{

//    [TestClass]
//    public class MRZReaderHandlerShouldDo
//    {
//        private MRZReaderHandler handler;

//        [TestInitialize]
//        public void TestInitialize()
//        {
//            var logger = Substitute.For<ILogger<MRZReaderHandler>>();
//            var documentRepository = Substitute.For<IDocumentRepository>();
//            var dataExtractor = Substitute.For<IDataExtractor>();
//            var options = Substitute.For<IOptions<CloudOcrSettings>>();
//            options.Value.Returns(new CloudOcrSettings()
//            {
//                Password = "MGM_Doc_Processing_V1",
//                ApplicationId = "6kK8tZ9ystP+ZnOcmqucxm25"
//            });
//            handler = new MRZReaderHandler(logger, documentRepository, options, dataExtractor);
//        }

//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException), "DocumentRequest is invalid")]
//        public void ValidateRequest_Null_Test()
//        {
//            handler.ValidateRequest(null);
//        }
//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException), "SourceFolder is invalid")]
//        public void ValidateRequest_SourceFolder_Null_Test()
//        {

//            DocumentRequest request = new DocumentRequest()
//            {
//                SourceFolder = "",
//                DestinationFolder = @"somepath",
//                OriginalFile = Substitute.For<IFormFile>()
//            };
//            handler.ValidateRequest(request);
//        }
//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException), "DestinationFolder is invalid")]
//        public void ValidateRequest_DestinationFolder_Null_Test()
//        {

//            DocumentRequest request = new DocumentRequest()
//            {
//                SourceFolder = @"somepath",
//                DestinationFolder = "",
//                OriginalFile = Substitute.For<IFormFile>()
//            };
//            handler.ValidateRequest(request);
//        }
//        [TestMethod]
//        [ExpectedException(typeof(ArgumentException), "Document is invalid")]
//        public void ValidateRequest_Document_Null_Test()
//        {

//            DocumentRequest request = new DocumentRequest()
//            {
//                SourceFolder = @"somepath",
//                DestinationFolder = "somepath"
//            };
//            handler.ValidateRequest(request);
//        }
//        [TestMethod]
//        public void ValidateRequest_Happy_Path_ShouldContinue_Test()
//        {

//            DocumentRequest request = new DocumentRequest()
//            {
//                SourceFolder = @"somepath",
//                DestinationFolder = "somepath",
//                OriginalFile = Substitute.For<IFormFile>()
//            };
//            handler.ValidateRequest(request);
//            Assert.IsTrue(request.ShouldContinue);
//        }
//        [TestMethod]
//        public void ValidateRequest_PopulateFileUniqueName_Happy_Path_ShouldContinue_Test()
//        {

//            DocumentRequest request = new DocumentRequest()
//            {
//                SourceFolder = @"somepath",
//                DestinationFolder = "somepath",
//                OriginalFile = Substitute.For<IFormFile>(),
//                Document =
//                {
//                    FileFullName="File101.jpg"
//                },
//                ShouldContinue = true
//            };
//            handler.PopulateFileUniqueName(request);

//            Assert.IsTrue(request.ShouldContinue);
//            Assert.IsTrue(request.FileUniqueName.EndsWith($"_{request.Document.FileFullName}"));
//        }
//        [TestMethod]
//        public void ValidateRequest_PopulateFileUniqueName_ShouldContinue_False_Test()
//        {

//            DocumentRequest request = new DocumentRequest()
//            {
//                SourceFolder = @"somepath",
//                DestinationFolder = "somepath",
//                OriginalFile = Substitute.For<IFormFile>(),
//                ShouldContinue = false
//            };
//            handler.PopulateFileUniqueName(request);
//            Assert.IsFalse(request.ShouldContinue);
//            Assert.IsNull(request.FileUniqueName);
//        }
//    }
//}
