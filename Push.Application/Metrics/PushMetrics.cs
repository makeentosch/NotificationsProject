using System.Diagnostics.Metrics;

namespace Push.Application.Metrics;

public static class PushMetrics
{
    public const string ServiceName = "Push";

    private static Meter Meter = new(ServiceName);

    public static Counter<int> SendNotificationCounter = Meter.CreateCounter<int>("send.count");
}