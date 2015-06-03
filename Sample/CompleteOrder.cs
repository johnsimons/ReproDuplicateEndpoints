using NServiceBus;

public class CompleteOrder : IMessage
{
    public int OrderId { get; set; }
}