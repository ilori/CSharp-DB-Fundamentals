using System;
using ConsoleApplication;

public class Topping
{
    private string toppingType;
    private double weight;

    public Topping(string toppingType, double weight)
    {
        this.ToppingType = toppingType;
        this.Weight = weight;
    }

    private string ToppingType
    {
        get { return this.toppingType; }
        set
        {
            if (value.ToLower() != "meat" && value.ToLower() != "veggies" && value.ToLower() != "cheese" &&
                value.ToLower() != "sauce")
            {
                throw new ArgumentException($"Cannot place {value} on top of your pizza.");
            }
            this.toppingType = value;
        }
    }

    private double Weight
    {
        get { return this.weight; }
        set
        {
            if (value < 1 || value > 50)
            {
                throw new ArgumentException($"{this.ToppingType} weight should be in the range [1..50].");
            }
            this.weight = value;
        }
    }

    public double Calories()
    {
        var baseCalories = 2d;

        if (toppingType.ToLower() == "meat")
        {
            baseCalories *= Modifiers.Meat;
        }
        else if (toppingType.ToLower() == "veggies")
        {
            baseCalories *= Modifiers.Veggies;
        }
        else if (toppingType.ToLower() == "cheese")
        {
            baseCalories *= Modifiers.Cheese;
        }
        else
        {
            baseCalories *= Modifiers.Sauce;
        }
        baseCalories *= this.Weight;
        return baseCalories;
    }
}