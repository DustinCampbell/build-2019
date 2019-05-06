using CommercialRegistration;
using ConsumerVehicleRegistration;
using LiveryRegistration;
using System;

namespace TollCollector
{
    public class TollCalculator
    {
        public decimal CalculateToll(object vehicle)
        {
            switch (vehicle)
            {
                case Car _:
                    return 2.00m;
                case Taxi _:
                    return 3.00m;
                case Bus _:
                    return 5.00m;
                case DeliveryTruck _:
                    return 10.00m;
                case null:
                    throw new ArgumentNullException(nameof(vehicle));
                default:
                    throw new ArgumentException(message: "Not a known vehicle type", paramName: nameof(vehicle));
            }
        }
    }
}
