using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Wire.Transfer.In.Domain.SeedWorks;
using static System.String;

namespace Wire.Transfer.In.Infrastructure.Logs
{
    public class Logging : ILogging
    {
        public static readonly ILogging Instance = new Logging();
        private readonly IHttpContextAccessor _contextAccessor;

        public Logging() : this(new HttpContextAccessor())
        {
        }

        internal Logging(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        private static string HeaderKey => "x-correlation-id";

        public string CorrelationId
        {
            get
            {
                var header = Empty;

                if (_contextAccessor.HttpContext.Request.Headers.TryGetValue(HeaderKey, out var values))
                    header = values.FirstOrDefault();
                else if (_contextAccessor.HttpContext.Response.Headers.TryGetValue(HeaderKey, out values))
                    header = values.FirstOrDefault();

                var correlationId = IsNullOrEmpty(header)
                    ? Guid.NewGuid().ToString()
                    : header;

#if NETFULL
            if(!_contextAccessor.HttpContext.Response.HeadersWritten &&
                !_contextAccessor.HttpContext.Response.Headers.AllKeys.Contains(_headerKey))
#else
                if (!_contextAccessor.HttpContext.Response.Headers.ContainsKey(HeaderKey))
#endif
                    _contextAccessor.HttpContext.Response.Headers.Add(HeaderKey, correlationId);

                return correlationId;
            }
        }

        public string TimeStamp => DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss.fff");
        public string Application => "SampleBankWireTransferApi";
        public LogLevel Level { get; set; }
        public string Type { get; set; }

        /// <summary>
        ///     Create Logging level Error and Throw Exception
        /// </summary>
        /// <param name="exception"></param>
        /// <exception cref="Exception"></exception>
        public void Error(object exception)
        {
            Serilog.Log.Error("{{ \"Headers\": {@Logging}, \"Message\": {@Message} }}", new Logging
            {
                Level = LogLevel.Error
            }, exception);
        }

        /// <summary>
        ///     Create Logging level Warning
        /// </summary>
        /// <param name="message"></param>
        public void Warning(object message)
        {
            Serilog.Log.Warning("{{ \"Headers\": {@Logging}, \"Message\": {@Message} }}", new Logging
            {
                Level = LogLevel.Warning
            }, message);
        }

        /// <summary>
        ///     Create Logging level Information
        /// </summary>
        /// <param name="message"></param>
        public void Information(object message)
        {
            Serilog.Log.Information("{{ \"Headers\": {@Logging}, \"Message\": {@Message} }}", new Logging
            {
                Level = LogLevel.Information
            }, message);
        }
    }
}