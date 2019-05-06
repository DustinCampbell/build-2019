using System;
using CommercialRegistration;
using ConsumerVehicleRegistration;
using LiveryRegistration;

namespace TollCollectorLib
{
    public static class TollCalculator
    {
        public static decimal CalculateToll(object vehicle)
        {
            switch (vehicle)
            {
                case Car c when c.Passengers == 0:
                    return 2.00m + 0.50m;
                case Car c when c.Passengers == 1:
                    return 2.0m;
                case Car c when c.Passengers == 2:
                    return 2.0m - 0.50m;
                case Car _:
                    return 2.00m - 1.0m;

                case Taxi t when t.Fares == 0:
                    return 3.50m + 1.00m;
                case Taxi t when t.Fares == 1:
                    return 3.50m;
                case Taxi t when t.Fares == 2:
                    return 3.50m - 0.50m;
                case Taxi _:
                    return 3.50m - 1.00m;

                case Bus b when ((double)b.Riders / b.Capacity) < 0.50:
                    return 5.00m + 2.00m;
                case Bus b when ((double)b.Riders / b.Capacity) > 0.90:
                    return 5.00m - 1.00m;
                case Bus _:
                    return 5.00m;

                case DeliveryTruck t when t.GrossWeightClass > 5000:
                    return 10.00m + 5.00m;
                case DeliveryTruck t when t.GrossWeightClass < 3000:
                    return 10.00m - 2.00m;
                case DeliveryTruck _:
                    return 10.00m;
            };

            throw new ArgumentException(message: "Not a known vehicle type", paramName: nameof(vehicle));
        }

        private enum TimeBand
        {
            MorningRush,
            Daytime,
            EveningRush,
            Overnight
        }

        private static TimeBand GetTimeBand(DateTime timeOfToll)
        {
            int hour = timeOfToll.Hour;
            if (hour < 6)
                return TimeBand.Overnight;
            else if (hour < 10)
                return TimeBand.MorningRush;
            else if (hour < 16)
                return TimeBand.Daytime;
            else if (hour < 20)
                return TimeBand.EveningRush;
            else
                return TimeBand.Overnight;
        }

        private static bool IsWeekDay(DateTime timeOfToll) =>
            timeOfToll.DayOfWeek switch
            {
                DayOfWeek.Monday => true,
                DayOfWeek.Tuesday => true,
                DayOfWeek.Wednesday => true,
                DayOfWeek.Thursday => true,
                DayOfWeek.Friday => true,
                DayOfWeek.Saturday => false,
                DayOfWeek.Sunday => false
            };

        public static decimal PeakTimePremium(DateTime timeOfToll, bool inbound) =>
            (IsWeekDay(timeOfToll), GetTimeBand(timeOfToll), inbound) switch
            {
                (true, TimeBand.MorningRush, true) => 2.00m,
                (true, TimeBand.MorningRush, false) => 1.00m,
                (true, TimeBand.Daytime, true) => 1.50m,
                (true, TimeBand.Daytime, false) => 1.50m,
                (true, TimeBand.EveningRush, true) => 1.00m,
                (true, TimeBand.EveningRush, false) => 2.00m,
                (true, TimeBand.Overnight, true) => 0.75m,
                (true, TimeBand.Overnight, false) => 0.75m,
                (false, TimeBand.MorningRush, true) => 1.00m,
                (false, TimeBand.MorningRush, false) => 1.00m,
                (false, TimeBand.Daytime, true) => 1.00m,
                (false, TimeBand.Daytime, false) => 1.00m,
                (false, TimeBand.EveningRush, true) => 1.00m,
                (false, TimeBand.EveningRush, false) => 1.00m,
                (false, TimeBand.Overnight, true) => 1.00m,
                (false, TimeBand.Overnight, false) => 1.00m,
            };
    }
}
