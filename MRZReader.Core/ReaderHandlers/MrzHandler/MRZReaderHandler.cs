using System;
using Abbyy.CloudOcrSdk;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("MRZReader.Core.Test")]
namespace MRZReader.Core
{
    public class MRZReaderHandler : RequestHandler<DocumentRequest>
    {
        private readonly IDataExtractor _dataExtractor;
        private RestServiceClient _restClient;
        private readonly ILogger _logger;
        private CloudOcrSettings _cloudOcrSettings;
        private readonly IDocumentRepository _documentRepository;

        public MRZReaderHandler(
            ILogger<MRZReaderHandler> logger,
            IDocumentRepository documentRepository, 
            IOptions<CloudOcrSettings> settings,
            IDataExtractor dataExtractor)
        {
            _logger = logger;
            _documentRepository = documentRepository;
            PopulateCloudOcrSettingsSettings(settings);
            _dataExtractor = dataExtractor;
        }
        public void PopulateCloudOcrSettingsSettings(IOptions<CloudOcrSettings> settings)
        {
            try
            {
                _restClient = new RestServiceClient();
                _restClient.Proxy.Credentials = CredentialCache.DefaultCredentials;
                _cloudOcrSettings = settings.Value;
                _restClient.ApplicationId = _cloudOcrSettings.ApplicationId;
                _restClient.Password = _cloudOcrSettings.Password;
            }
            catch (Exception e)
            {
                Log($"KO :Exception: {e.Message}", true);
                throw;
            }
        }
        protected override void Handle(DocumentRequest request)
        {
            try
            {
                ValidateRequest(request);
                PopulatFileFullName(request);
                PopulateFileUniqueName(request);
                PopulateSourceFilePath(request);
                UploadFile(request);
                PopulateTargetFilePath(request);
                ProcessMrz(request);
                ExtractDataFromXml(request);
                PersistSourceFileInfoInDatabase(request);
            }
            catch (Exception e)
            {
                request.IsSuccessed = false;
                Log($"KO :Exception: {e.Message}", true);
                throw;
            }
        }
        internal DocumentRequest ValidateRequest(DocumentRequest request)
        {
            if (request == null)
                throw new ArgumentException("DocumentRequest is invalid");

            if (string.IsNullOrEmpty(request.SourceFolder))
                throw new ArgumentException("SourceFolder is invalid");

            if (string.IsNullOrEmpty(request.DestinationFolder))
                throw new ArgumentException("DestinationFolder is invalid");

            if (request.OriginalFile==null)
                throw new ArgumentException("Document is invalid");

            request.ShouldContinue = true;
            return request;
        }
        internal DocumentRequest PopulatFileFullName(DocumentRequest request)
        {
            if (request.ShouldContinue)
                request.Document.FileFullName = request.OriginalFile.FileName;
            return request;
        }
        internal DocumentRequest PopulateFileUniqueName(DocumentRequest request)
        {
            //// Make sure the file name is unique, otherwise if we upload the same file, it will override the existing one.
            if (request.ShouldContinue)
                request.FileUniqueName = Guid.NewGuid().ToString() + "_" + request.Document.FileFullName;

            return request;
        }
        internal DocumentRequest PopulateSourceFilePath(DocumentRequest request)
        {
            if (request.ShouldContinue)
            {
                request.Document.SourceFilePath = Path.Combine(request.SourceFolder, request.FileUniqueName);
            }
            return request;
        }
        internal DocumentRequest UploadFile(DocumentRequest request)
        {
            if (request.ShouldContinue)
            {
                //Create the document.
                using (FileStream fileStream = System.IO.File.Create(request.Document.SourceFilePath))
                {
                    request.OriginalFile.CopyTo(fileStream);
                }
            }
            return request;
        }
        internal DocumentRequest PopulateTargetFilePath(DocumentRequest request)
        {
            if (request.ShouldContinue)
            {
                var fileWithoutExtension = Path.GetFileNameWithoutExtension($"{request.DestinationFolder}\\{request.FileUniqueName}");
                var _outputFilePath = $"{request.DestinationFolder}\\{fileWithoutExtension}.xml";
                request.Document.TargetFilePath = _outputFilePath;
            }
            return request;
        }
        internal DocumentRequest ProcessMrz(DocumentRequest request)
        {
            try
            {
                OcrSdkTask task = _restClient.ProcessMrz(request.Document.SourceFilePath);
                WaitAndDownload(task, request);
            }
            catch (Exception e)
            {
                Log($"KO :Exception: {e.Message}", true);
                request.IsSuccessed = false;
                throw;
            }
            return request;
        }
        internal DocumentRequest WaitAndDownload(OcrSdkTask task, DocumentRequest request)
        {
            task = WaitForTask(task);
            if (task.Status == TaskStatus.Completed)
            {
                _restClient.DownloadResult(task, request.Document.TargetFilePath);
                request.IsSuccessed = true;
            }
            return request;
        }
        internal OcrSdkTask WaitForTask(OcrSdkTask task)
        {
            _logger.LogTrace(string.Format("Task status: {0}", task.Status));
            while (task.IsTaskActive())
            {
                System.Threading.Thread.Sleep(5000);
                task = _restClient.GetTaskStatus(task.Id);
                _logger.LogTrace(string.Format("Task status: {0}", task.Status));
            }
            return task;
        }
        internal DocumentRequest ExtractDataFromXml(DocumentRequest request)
        {
            return _dataExtractor.Extract(request);
        }
        internal DocumentRequest PersistSourceFileInfoInDatabase(DocumentRequest request)
        {
            if (!request.IsSuccessed)
                return request;

            if (request.Document == null || request.Document?.User == null)
            {
                request.IsSuccessed = false;
                return request;
            }
            _documentRepository.Add(request);
            return request;
        }
        private void Log(string msg, bool isEception = false)
        {
            if (isEception)
            {
                _logger.LogError($"[MRZ_Logs] {msg}.");
            }
            else
            {
                _logger.LogTrace($"[MRZ_Logs] {msg}.");
            }
        }
    }
}