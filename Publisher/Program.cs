using System;
using Events;
using NServiceBus;

class Program
{

    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.EndpointName("Publisher.ComplexSagaFindingLogic");
        busConfiguration.UseSerialization<XmlSerializer>();
        busConfiguration.EnableInstallers();
        busConfiguration.UsePersistence<InMemoryPersistence>();

        using (IStartableBus bus = Bus.Create(busConfiguration))
        {
            bus.Start();

            Console.WriteLine("\r\nPress any key to send message or 'q' to quit\r\n");
            ConsoleKeyInfo key;
            Random r = new Random();

            do
            {
                key = Console.ReadKey();
                var num = r.Next(1, 10000);
                bus.Publish<IStartMultipleSagas>(d => d.OrderId = num);

            } while (key.Key != ConsoleKey.Q);
        }
    }
}
