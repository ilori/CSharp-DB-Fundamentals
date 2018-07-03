using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApplication;

public class Program
{
    public static void Main()
    {
        var n = int.Parse(Console.ReadLine());
        var cars = new List<Car>();

        AddCars(n, cars);

        var input = Console.ReadLine();

        if (input == "fragile")
        {
            foreach (var car in cars.Where(x => x.Cargo.Type == input && x.Tires.Any(t => t.Pressure < 1)))
            {
                Console.WriteLine(car.Model);
            }
        }
        else if (input == "flammable")
        {
            foreach (var car in cars.Where(x => x.Cargo.Type == input && x.Engine.Power > 250))
            {
                Console.WriteLine(car.Model);
            }
        }
    }

    private static void AddCars(int n, List<Car> cars)
    {
        for (int i = 0; i < n; i++)
        {
            var tokens = Console.ReadLine().Split();
            var carModel = tokens[0];
            var engineSpeed = int.Parse(tokens[1]);
            var enginePower = int.Parse(tokens[2]);
            var cargoWeight = int.Parse(tokens[3]);
            var cargoType = tokens[4];

            var firstTirePressure = double.Parse(tokens[5]);
            var firstTireAge = int.Parse(tokens[6]);
            var firstTire = new Tire(firstTirePressure, firstTireAge);

            var secondTirePressure = double.Parse(tokens[7]);
            var secondTireAge = int.Parse(tokens[8]);
            var secondTire = new Tire(secondTirePressure, secondTireAge);

            var thirdTirePressure = double.Parse(tokens[9]);
            var thirdTireAge = int.Parse(tokens[10]);
            var thirdTire = new Tire(thirdTirePressure, thirdTireAge);

            var forthTirePressure = double.Parse(tokens[11]);
            var forthTireAge = int.Parse(tokens[12]);
            var forthTire = new Tire(forthTirePressure, forthTireAge);

            var engine = new Engine(engineSpeed, enginePower);
            var cargo = new Cargo(cargoWeight, cargoType);

            var car = new Car(carModel, engine, cargo);
            car.Tires.Add(firstTire);
            car.Tires.Add(secondTire);
            car.Tires.Add(thirdTire);
            car.Tires.Add(forthTire);

            cars.Add(car);
        }
    }
}