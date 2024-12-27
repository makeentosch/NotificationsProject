using System.Diagnostics.Metrics;

namespace Mail.Application.Metrics;

public static class MailMetrics
{
    public const string ServiceName = "Mail";

    private static Meter Meter = new(ServiceName);

    public static Counter<int> SendNotificationCounter = Meter.CreateCounter<int>("send.count");
}