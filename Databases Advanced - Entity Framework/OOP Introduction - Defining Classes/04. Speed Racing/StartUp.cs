using System;
using System.Collections.Generic;
using System.Linq;

public class StartUp
{
    public static void Main(string[] args)
    {
        var n = int.Parse(Console.ReadLine());

        var cars = new Dictionary<string, Car>();

        for (int i = 0; i < n; i++)
        {
            var tokens = Console.ReadLine().Split(' ').ToArray();
            var model = tokens[0];
            var fuelAmount = double.Parse(tokens[1]);
            var consumption = double.Parse(tokens[2]);
            if (!cars.ContainsKey(model))
            {
                cars[model] = new Car
                {
                    Model = model,
                    FuelAmount = fuelAmount,
                    Consumption = consumption,
                    Distance = 0
                };
            }
        }
        string input;

        while ((input = Console.ReadLine()) != "End")
        {
            var tokens = input.Split(' ').ToArray();
            var command = tokens[0];
            var model = tokens[1];
            var distance = double.Parse(tokens[2]);
            if (command == "Drive")
            {
                if (cars.ContainsKey(model))
                {
                    var car = cars[model];
                    if (car.EnoughAmount(distance, car.FuelAmount, car.Consumption))
                    {
                        cars[model].Distance += distance;
                        cars[model].FuelAmount -= distance * cars[model].Consumption;
                        
                    }
                    else
                    {
                        Console.WriteLine("Insufficient fuel for the drive");
                    }
                }
            }
        }
        
        foreach (var car in cars.Values)
        {
            Console.WriteLine($"{car.Model} {car.FuelAmount:f2} {car.Distance}");
        }
    }
}