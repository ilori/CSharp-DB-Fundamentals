using System;
using ConsoleApplication;

public class Dough
{
    private string flourType;
    private string bakingTechnique;
    private double weight;


    public Dough(string flourType, string bakingTechnique, double weight)
    {
        this.FlourType = flourType;
        this.BakingTecnique = bakingTechnique;
        this.Weight = weight;
    }

    private string FlourType
    {
        get { return this.flourType; }
        set
        {
            if (value.ToLower() != "white" && value.ToLower() != "wholegrain")
            {
                throw new Exception("Invalid type of dough.");
            }
            this.flourType = value;
        }
    }

    private string BakingTecnique
    {
        get { return this.bakingTechnique; }
        set
        {
            if (value.ToLower() != "crispy" && value.ToLower() != "chewy" && value.ToLower() != "homemade")
            {
                throw new ArgumentException("Invalid type of dough.");
            }
            this.bakingTechnique = value;
        }
    }

    private double Weight
    {
        get { return this.weight; }
        set
        {
            if (value < 1 || value > 200)
            {
                throw new ArgumentException(    "Dough weight should be in the range [1..200].");
            }
            this.weight = value;
        }
    }


    public double Calories()
    {
        var baseCalories = 2d;
        if (this.FlourType.ToLower() == "white")
        {
            baseCalories *= Modifiers.White;
        }
        else if (this.FlourType.ToLower() == "wholegrain")
        {
            baseCalories *= Modifiers.Homemade;
        }
        if (this.BakingTecnique.ToLower() == "crispy")
        {
            baseCalories *= Modifiers.Crispy;
        }
        else if (this.BakingTecnique.ToLower() == "chewy")
        {
            baseCalories *= Modifiers.Chewy;
        }
        else if (this.BakingTecnique.ToLower() == "homemade")
        {
            baseCalories *= Modifiers.Homemade;
        }

        baseCalories *= this.Weight;
        return baseCalories;
    }
}