namespace ConsumerVehicleRegistration
{
    public class Car
    {
        public int Passengers { get; set; }

        public override string ToString()
            => $"Car, Passengers = {Passengers}";
    }
}

namespace CommercialRegistration
{
    public class DeliveryTruck
    {
        public int GrossWeightClass { get; set; }

        public override string ToString()
            => $"Delivery Truck, GrossWeightClass = {GrossWeightClass}";
    }
}

namespace LiveryRegistration
{
    public class Taxi
    {
        public int Fares { get; set; }

        public override string ToString()
            => $"Taxi, Fares = {Fares}";
    }

    public class Bus
    {
        public int Capacity { get; set; }
        public int Riders { get; set; }

        public override string ToString()
            => $"Bus, Capacity = {Capacity}, Riders = {Riders}";

        public void Deconstruct(out int capacity, out int riders)
            => (capacity, riders) = (Capacity, Riders);
    }
}