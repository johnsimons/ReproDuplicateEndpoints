using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Common;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Saga;

#region thesaga
public class OrderSaga : Saga<OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleMessages<CompleteOrder>,
    IHandleMessages<AnotherMessage>,
    IHandleTimeouts<FooBar>,
    IHandleTimeouts<FooBar2>
{
    static ILog logger = LogManager.GetLogger(typeof(OrderSaga));

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.ConfigureMapping<StartOrder>(m => m.OrderId)
                .ToSaga(s => s.OrderId);
        mapper.ConfigureMapping<CompleteOrder>(m => m.OrderId)
                .ToSaga(s => s.OrderId);
        mapper.ConfigureMapping<AnotherMessage>(m => m.OrderId)
                .ToSaga(s => s.OrderId);
    }

#endregion
    public void Handle(StartOrder message)
    {
        Data.OrderId = message.OrderId;
        Data.Data = "Mmore items";
        Data.LineItems = new List<LineItem>() {new LineItem() {Count = 5, Name = "FooBar"}, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, };
        logger.Info(string.Format("Saga with OrderId {0} received StartOrder with OrderId {1}", Data.OrderId, message.OrderId));
        Bus.SendLocal(new AnotherMessage
        {
            OrderId = Data.OrderId,
            Data = "مرحبا بالعالم",
            LineItems = new List<LineItem>() { new LineItem() { Count = 23, Name = "FooBar2323" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 3213325, Name = "FooBar324324" }, new LineItem() { Count = 34345, Name = "Fo324324oBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, },
            ReallyLongNameOfPropertyToShowSomeBusWithReallyLongNamesToFigureOutHowToDealWithIt = 43,
            //Records = XElement.Load(new StreamReader(@"C:\Users\john\Desktop\data2.xml"))
        });
    }

    
    public void Handle(CompleteOrder message)
    {
        //throw new Exception("Sorry no go here");
        logger.Info(string.Format("Saga with OrderId {0} received CompleteOrder with OrderId {1}", Data.OrderId, message.OrderId));
        MarkAsComplete();
    }

    public void Handle(AnotherMessage message)
    {
        
        Data.Data = "مرحبا بالعالم";
        Data.LineItems = new List<LineItem>() { new LineItem() { Count = 23, Name = "FooBar2323" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 3213325, Name = "FooBar324324" }, new LineItem() { Count = 34345, Name = "Fo324324oBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, new LineItem() { Count = 5, Name = "FooBar" }, };
        Data.ReallyLongNameOfPropertyToShowSomeBusWithReallyLongNamesToFigureOutHowToDealWithIt = 2;

        RequestTimeout<FooBar>(TimeSpan.FromSeconds(10));
        RequestTimeout<FooBar2>(TimeSpan.FromSeconds(9));
        RequestTimeout<FooBar2>(TimeSpan.FromSeconds(8));
        RequestTimeout<FooBar2>(TimeSpan.FromSeconds(7));

        Bus.SendLocal(new Message6());
        Bus.SendLocal(new Message4());

        Bus.Publish<IMyEvent>(e =>
        {
            e.OrderId = message.OrderId;
            e.booGoo = Data.Data;
        });

        Bus.Publish<AnotherMessageReceived>(e =>
        {
            e.OrderId = message.OrderId;
        });
    }

    public void Timeout(FooBar2 state)
    {
    }

    public void Timeout(FooBar state)
    {
        Data.Data = "こんにちは世界";
        Data.LineItems = new List<LineItem>() { new LineItem() { Count = 23, Name = "FooBar2323" } };
        Data.ReallyLongNameOfPropertyToShowSomeBusWithReallyLongNamesToFigureOutHowToDealWithIt = 4545;

        Bus.SendLocal(new CompleteOrder
        {
            OrderId = Data.OrderId
        });
    }
}

public class FooBar
{
}

public class FooBar2
{
}

public class Message6 : ICommand
{
}

public class Message4 : ICommand
{
}

public class AnotherMessage : IMessage
{
    public int OrderId { get; set; }
    public string Data { get; set; }
    public List<LineItem> LineItems { get; set; }
    public int ReallyLongNameOfPropertyToShowSomeBusWithReallyLongNamesToFigureOutHowToDealWithIt { get; set; }

    public XElement Records { get; set; }
}

class F: IHandleMessages<Message4>
{
    public void Handle(Message4 message)
    {
        Console.Out.WriteLine("Message4 received");
    }
}

class F2 : IHandleMessages<Message6>
{
    public void Handle(Message6 message)
    {
        Console.Out.WriteLine("Message6 received");
    }
}
