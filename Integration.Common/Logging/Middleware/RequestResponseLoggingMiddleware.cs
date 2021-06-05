using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Integration.Common.Logging.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _requestLogger;
        private readonly ILogger _responseLogger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public RequestResponseLoggingMiddleware(RequestDelegate next,
                                                ILoggerFactory loggerFactory)
        {
            _next = next;
            _requestLogger = loggerFactory.CreateLogger("Request");
            _responseLogger = loggerFactory.CreateLogger("Response");
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.Value.Contains("swagger") || context.Request.Path.Value.Contains(".html"))
            {
                await _next(context);
            }
            else
            {
                await LogRequest(context);
                await LogResponse(context);
            }
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            var request = new
            {
                Schema = context.Request.Scheme,
                Host = context.Request.Host,
                Path = context.Request.Path,
                QueryString = context.Request.QueryString,
                Headers = context.Request.Headers,
                RequestBody = ReadStreamInChunks(requestStream)
            };
            var requestJson = JsonConvert.SerializeObject(request);
            _requestLogger.LogInformation(requestJson);
            context.Request.Body.Position = 0;
        }
        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);
            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);
            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;
            do
            {
                readChunkLength = reader.ReadBlock(readChunk,
                                                   0,
                                                   readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);
            return textWriter.ToString();
        }

        private async Task LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;
            await _next(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var response = new
            {
                Schema = context.Request.Scheme,
                Host = context.Request.Host,
                Path = context.Request.Path,
                QueryString = context.Request.QueryString,
                Headers = context.Response.Headers,
                StatusCode = context.Response.StatusCode,
                RequestBody = text
            };
            var responseJson = JsonConvert.SerializeObject(response);
            _responseLogger.LogInformation(responseJson);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
