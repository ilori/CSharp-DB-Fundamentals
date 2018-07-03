public class Car
{
    public string Model { get; set; }
    public double FuelAmount { get; set; }
    public double Consumption { get; set; }
    public double Distance { get; set; }

    public bool EnoughAmount(double distance, double carFuelAmount, double carConsumption)
    {
        var totalDistance = distance * carConsumption;
        if (totalDistance <= carFuelAmount)
        {
            return true;
        }
        return false;
    }
}