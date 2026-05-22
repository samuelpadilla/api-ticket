using Polly;
using System;

namespace TicketProcessor.Infra.Resilience
{
    public static class TimeoutPolicies
    {
        public static IAsyncPolicy GetDefaultTimeoutPolicy()
        {
            return Policy.TimeoutAsync(TimeSpan.FromSeconds(30));
        }
    }
}