namespace Sample
{
    using System;
    using Events;
    using NServiceBus;
    using NServiceBus.Saga;

    class MySampleSaga : Saga<MySampleSaga.SampleSagaData>, IAmStartedByMessages<IStartMultipleSagas>, IHandleMessages<SampleMessage1Reply>
    {
        public class SampleSagaData : ContainSagaData
        {
            public int OrderId { get; set; }
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SampleSagaData> mapper)
        {
            mapper.ConfigureMapping<SampleMessage1>(m => m.OrderId).ToSaga(s => s.OrderId);
            mapper.ConfigureMapping<SampleMessage1Reply>(m => m.OrderId).ToSaga(s => s.OrderId);
        }

        public void Handle(IStartMultipleSagas message)
        {
            Bus.SendLocal(new SampleMessage1() { OrderId = message.OrderId });
        }

        public void Handle(SampleMessage1Reply message)
        {
            MarkAsComplete();
            Console.Out.WriteLine("MySampleSaga complete");
        }
    }

    class MySampleSaga2 : Saga<MySampleSaga2.SampleSagaData2>, IAmStartedByMessages<IStartMultipleSagas>, IHandleMessages<SampleMessage2Reply>
    {
        public class SampleSagaData2 : ContainSagaData
        {
            public int OrderId { get; set; }
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SampleSagaData2> mapper)
        {
            mapper.ConfigureMapping<SampleMessage2>(m => m.OrderId).ToSaga(s => s.OrderId);
            mapper.ConfigureMapping<SampleMessage2Reply>(m => m.OrderId).ToSaga(s => s.OrderId);
        }

        public void Handle(IStartMultipleSagas message)
        {
            Bus.SendLocal(new SampleMessage2() { OrderId = message.OrderId });
        }

        public void Handle(SampleMessage2Reply message)
        {
            MarkAsComplete();
            Console.Out.WriteLine("MySampleSaga2 complete");
        }
    }

    class SampleMessage1 : ICommand
    {
        public int OrderId { get; set; }
    }
    class SampleMessage1Reply : IMessage
    {
        public int OrderId { get; set; }
    }

    class SampleMessage2 : ICommand
    {
        public int OrderId { get; set; }
    }
    class SampleMessage2Reply : IMessage
    {
        public int OrderId { get; set; }
    }

    class DD : IHandleMessages<SampleMessage1>
    {
        public IBus Bus { get; set; }

        public void Handle(SampleMessage1 message)
        {
            Bus.Reply(new SampleMessage1Reply() { OrderId = message.OrderId });
        }
    }

    class DD2 : IHandleMessages<SampleMessage2>
    {
        public IBus Bus { get; set; }

        public void Handle(SampleMessage2 message)
        {
            Bus.Reply(new SampleMessage2Reply() { OrderId = message.OrderId });
        }
    }
}
