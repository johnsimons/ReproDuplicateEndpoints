
namespace Common
{
    using NServiceBus;

    public interface IMyEvent : IEvent
    {
        int OrderId { get; set; }
        string booGoo { get; set; }
    }

    public interface AnotherMessageReceived : IEvent
    {
        int OrderId { get; set; }
    }
}

namespace Events
{
    using NServiceBus;

    public interface IStartMultipleSagas : IEvent
    {
        int OrderId { get; set; }
    }
}
