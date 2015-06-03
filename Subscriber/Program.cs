using System;
using Common;
using Events;
using NServiceBus;
using NServiceBus.Saga;

class Program
{

    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Subscriber.ComplexSagaFindingLogic");
        busConfiguration.UseSerialization<XmlSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IStartableBus bus = Bus.Create(busConfiguration))
        {
            bus.Start();
            
            Console.WriteLine("\r\nPress any key to stop program\r\n");
            Console.ReadKey();
        }
    }
}


class G: IHandleMessages<IMyEvent>
{
    public void Handle(IMyEvent message)
    {
        Console.Out.WriteLine("Received event");
    }
}

class G3 : IHandleMessages<AnotherMessageReceived>
{
    public void Handle(AnotherMessageReceived message)
    {
        Console.Out.WriteLine("AnotherMessageReceived event");
    }
}

class MySubscriberSaga : Saga<MySubscriberSaga.SubscriberSagaData>, IAmStartedByMessages<IStartMultipleSagas>, IHandleMessages<SubscriberMessage1Reply>
{
    public class SubscriberSagaData : ContainSagaData
    {
        public int OrderId { get; set; }
    }

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SubscriberSagaData> mapper)
    {
        mapper.ConfigureMapping<SubscriberMessage1>(m => m.OrderId).ToSaga(s => s.OrderId);
        mapper.ConfigureMapping<SubscriberMessage1Reply>(m => m.OrderId).ToSaga(s => s.OrderId);
    }

    public void Handle(IStartMultipleSagas message)
    {
        Bus.Send(Address.Parse("foobar"), new SendingToStoppedEndpoint());
        Bus.SendLocal(new SubscriberMessage1() { OrderId = message.OrderId });
    }

    public void Handle(SubscriberMessage1Reply message)
    {
        MarkAsComplete();
        Console.Out.WriteLine("MySubscriberSaga complete");

    }
}

class SendingToStoppedEndpoint : IMessage
{
}

class SubscriberMessage1 : ICommand
{
    public int OrderId { get; set; }
}
class SubscriberMessage1Reply : IMessage
{
    public int OrderId { get; set; }
}

class DD : IHandleMessages<SubscriberMessage1>
{
    public IBus Bus { get; set; }

    public void Handle(SubscriberMessage1 message)
    {
        Bus.Reply(new SubscriberMessage1Reply(){ OrderId = message.OrderId});
    }
}
