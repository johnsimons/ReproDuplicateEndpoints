using NServiceBus;

public class StartOrder:IMessage
{
    public int OrderId { get; set; }
    public string DD { get; set; }
}