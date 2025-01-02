using System.Diagnostics.Metrics;

namespace Sms.Application.Metrics;

public static class SmsMetrics
{
    public const string ServiceName = "Sms";

    private static Meter Meter = new(ServiceName);

    public static Counter<int> SendNotificationCounter = Meter.CreateCounter<int>("send.count");
}