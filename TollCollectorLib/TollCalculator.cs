using System;
using CommercialRegistration;
using ConsumerVehicleRegistration;
using LiveryRegistration;

namespace TollCollectorLib
{
    public static class TollCalculator
    {
        public static decimal CalculateToll(object vehicle)
            => vehicle switch
            {
                Car c => c.Passengers switch
                {
                    0 => 2.00m + 0.50m,
                    1 => 2.0m,
                    2 => 2.0m - 0.50m,
                    _ => 2.00m - 1.0m,
                },

                Taxi { Fares: 0 } => 3.50m + 1.00m,
                Taxi { Fares: 1 } => 3.50m,
                Taxi { Fares: 2 } => 3.50m - 0.50m,
                Taxi _ => 3.50m - 1.00m,

                Bus (var c, var r) when ((double)r / c) < 0.50 =>  5.00m + 2.00m,
                Bus (var c, var r) when ((double)r / c) > 0.90 => 5.00m - 1.00m,
                Bus _ => 5.00m,

                DeliveryTruck t when t.GrossWeightClass > 5000 => 10.00m + 5.00m,
                DeliveryTruck t when t.GrossWeightClass < 3000 => 10.00m - 2.00m,
                DeliveryTruck _ => 10.00m,

                _ => throw new ArgumentException(message: "Not a known vehicle type", paramName: nameof(vehicle))
            };

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
                DayOfWeek.Saturday => false,
                DayOfWeek.Sunday => false,
                 _ => true,
            };

        public static decimal PeakTimePremium(DateTime timeOfToll, bool inbound) =>
            (IsWeekDay(timeOfToll), GetTimeBand(timeOfToll), inbound) switch
            {
                (true, TimeBand.MorningRush, true) => 2.00m,
                (true, TimeBand.Daytime, _) => 1.50m,
                (true, TimeBand.EveningRush, false) => 2.00m,
                (true, TimeBand.Overnight, _) => 0.75m,
                (_, _, _) => 1.00m,
            };
    }
}
