using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MRZReader.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRZReader.Cache
{
    public class MRZReaderCache : IMRZReaderCache
    {
        private readonly IMemoryCache _cache;
        private static CacheSettings _cacheSettings;
        private readonly DateTimeOffset _expirationDate;
        private const string DOCUMENT = "DocumentRequest";

        public MRZReaderCache(IMemoryCache cache, IOptions<CacheSettings> cacheSettings)
        {
            _cache = cache;
            _cacheSettings = cacheSettings.Value;
            _expirationDate = DateTimeOffset.ParseExact(
                $"" +
                $"{DateTime.Now.AddDays(1):yyyy-MM-dd} " +
                $"{_cacheSettings.ExpirationTime}",
                "yyyy-MM-dd HH:mm:ss",
                System.Globalization.CultureInfo.InvariantCulture);
        }
        public DocumentRequest GetDocumentRequestInfo(int documentId)
        {
            var key = $"{DOCUMENT}-{documentId}";
            return _cache.Get<DocumentRequest>(key);
        }
        public void Unlink(int documentId)
        {
            DocumentRequest currentDocumentRequest = GetDocumentRequestInfo(documentId);
            var key = $"{DOCUMENT}-{documentId}";
            if (currentDocumentRequest != null)
                _cache.Remove(key);
        }
        public void Update(int documentId, DocumentRequest request)
        {
            DocumentRequest currentDocumentRequest = GetDocumentRequestInfo(documentId);
            var key = $"{DOCUMENT}-{documentId}";
            if (currentDocumentRequest == null)
                _cache.Set(key, request);
        }
    }
}
