using System;
using System.Collections.Generic;

public class Pizza
{
    private string name;
    private Dough dough;
    private List<Topping> toppings;

    public Pizza(string name)
    {
        this.Name = name;
        this.toppings = new List<Topping>();
    }

    public string Name
    {
        get { return this.name; }
        set
        {
            if (value == string.Empty || value.Length > 15)
            {
                throw new ArgumentException("Pizza name should be between 1 and 15 symbols.");
            }
            this.name = value;
        }
    }

    public Dough Dough
    {
        get { return this.dough; }
        set { this.dough = value; }
    }

    public int ToppingsCount()
    {
        return this.toppings.Count;
    }

    public void AddTopping(Topping topping)
    {
        if (ToppingsCount() == 10)
        {
            throw new ArgumentException("Number of toppings should be in range [0..10].");
        }
        this.toppings.Add(topping);
    }


    public double TotalCalories()
    {
        var totalCalories = 0d;
        totalCalories += this.Dough.Calories();
        foreach (var topping in toppings)
        {
            totalCalories += topping.Calories();
        }
        return totalCalories;
    }
}