using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TollCollectorLib
{
    public static partial class TollSystem
    {
        private static readonly ConcurrentQueue<(object, DateTime, bool, string)> s_queue = new ConcurrentQueue<(object, DateTime, bool, string)>();

        private static ILogger s_logger;

        public static void Initialize(ILogger logger)
        {
            s_logger = logger;
        }

        public static void AddEntry(object vehicle, DateTime time, bool inbound, string license)
        {
            s_logger.SendMessage($"{time}: {(inbound ? "Inbound" : "Outbound")} {license} - {vehicle}");
            s_queue.Enqueue((vehicle, time, inbound, license));
        }

        public static async IAsyncEnumerable<(object vehicle, DateTime time, bool inbound, string license)> GetVehiclesAsync()
        {
            while (true)
            {
                if (s_queue.TryDequeue(out var entry))
                {
                    yield return entry;
                }

                await Task.Delay(500);
            }
        }

        public static async Task ChargeTollAsync(object vehicle, DateTime time, bool inbound, string license)
        {
            try
            {
                decimal baseToll = TollCalculator.CalculateToll(vehicle);
                decimal peakPremium = TollCalculator.PeakTimePremium(time, inbound);
                decimal toll = baseToll * peakPremium;
                Account account = await LookupAccountAsync(license);
                account.Charge(toll);
                s_logger.SendMessage($"Charged {license} {toll:C}");
            }
            catch (Exception ex)
            {
                s_logger.SendMessage(ex.Message, LogLevel.Error);
            }
        }
    }
}
