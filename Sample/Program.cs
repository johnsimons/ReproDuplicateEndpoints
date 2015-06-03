using System;
using NServiceBus;

class Program
{
    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Samples.ComplexSagaFindingLogic");
        busConfiguration.UseSerialization<XmlSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IStartableBus bus = Bus.Create(busConfiguration))
        {
            bus.Start();

            Random r = new Random();
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();
                var num = r.Next(1, 10000);
                bus.SendLocal(new StartOrder
                {
                    OrderId = num,
                    DD = "dsfdsf sdfsdf sdf sdf"
                });
                
            } while (key.Key != ConsoleKey.Q);
        }
    }
}
