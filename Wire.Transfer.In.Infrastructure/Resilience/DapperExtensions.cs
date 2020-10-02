using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using Wire.Transfer.In.Infrastructure.Database;
using Wire.Transfer.In.Infrastructure.Logs;
using SqlMapper = Dapper.SqlMapper;

namespace Wire.Transfer.In.Infrastructure.Resilience
{
    public static class DapperExtensions
    {
        private static readonly IEnumerable<TimeSpan> RetryTimes = new[]
        {
            TimeSpan.FromSeconds(5),
            TimeSpan.FromSeconds(15),
            TimeSpan.FromSeconds(20)
        };

        private static readonly AsyncRetryPolicy RetryPolicy = Policy
            .Handle<SqlException>(SqlServerTransientExceptionDetector.ShouldRetryOn)
            .Or<TimeoutException>()
            .OrInner<Win32Exception>(SqlServerTransientExceptionDetector.ShouldRetryOn)
            .WaitAndRetryAsync(RetryTimes,
                (exception, timeSpan, retryCount, context) =>
                {
                    Log.Warning(
                        $"Error talking to {DbNames.WireTransfersIn} DB, Waiting {timeSpan} before next retry. Retry attempt {retryCount}");
                });

        public static async Task ExecuteAsyncWithRetry(this IDbConnection cnn, string sql, object param = null)
        {
            await RetryPolicy.ExecuteAsync(async () =>
                await SqlMapper.ExecuteAsync(cnn, sql, param));
        }
    }
}