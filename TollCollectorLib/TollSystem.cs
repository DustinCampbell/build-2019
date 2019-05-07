using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

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
                Account? account = await LookupAccountAsync(license);
                if (account != null)
                {
                    account.Charge(toll);
                    s_logger.SendMessage($"Charged {license} {toll:C}");
                }
                else
                {
                    var state = license[^2..];
                    var plate = license[..^3];
                    var owner = await LookupOwnerAsync(state, plate);
                    var finalToll = toll + 2.00m;
                    owner.SendBill(finalToll + 2.00m);
                    s_logger.SendMessage($"Sent bill for {finalToll:C} to {license}");
                }
            }
            catch (Exception ex)
            {
                s_logger.SendError(ex);
            }
        }
    }
}
