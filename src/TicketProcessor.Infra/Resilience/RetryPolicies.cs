using Polly;
using Polly.Timeout;
using Polly.Retry;

namespace TicketProcessor.Infra.Resilience
{
    public static class RetryPolicies
    {
        public static IAsyncPolicy GetDefaultRetryPolicy()
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt))
                );
        }
    }

    public static class TimeoutPolicies
    {
        public static IAsyncPolicy GetDefaultTimeoutPolicy()
        {
            return Policy.TimeoutAsync(TimeSpan.FromSeconds(30));
        }
    }
}
