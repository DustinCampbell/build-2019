using System;
using System.Threading.Tasks;
using CommercialRegistration;
using ConsumerVehicleRegistration;
using LiveryRegistration;
using TollCollectorLib;

namespace TollCollector
{
    class Program
    {
        static async Task Main(string[] args)
        {
            TollSystem.Initialize(new Logger());

            await TollSystem.ChargeTollAsync(
                new Car { Passengers = 2 },
                time: DateTime.Now,
                inbound: true,
                license: "BSF-8479-WA");
        }

        private class Logger : ILogger
        {
            public void SendMessage(string message, LogLevel level)
            {
                Console.WriteLine(message);
            }
        }
    }
}
