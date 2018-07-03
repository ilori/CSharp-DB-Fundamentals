using System;
using System.Linq;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var pizzaInformation = Console.ReadLine().Split().ToList();
            var pizzaName = pizzaInformation[1];
            var pizza = new Pizza(pizzaName);

            var doughInformation = Console.ReadLine().Split().ToList();
            var doughFlourType = doughInformation[1];
            var doughBakingTechnique = doughInformation[2];
            var doughWeight = double.Parse(doughInformation[3]);

            var dough = new Dough(doughFlourType, doughBakingTechnique, doughWeight);
            pizza.Dough = dough;

            string input;
            while ((input = Console.ReadLine()) != "END")
            {
                var toppingInformation = input.Split().ToList();
                var toppingType = toppingInformation[1];
                var toppingWeight = double.Parse(toppingInformation[2]);
                var topping = new Topping(toppingType, toppingWeight);
                pizza.AddTopping(topping);
            }
            Console.WriteLine($"{pizza.Name} - {pizza.TotalCalories():F2} Calories.");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}