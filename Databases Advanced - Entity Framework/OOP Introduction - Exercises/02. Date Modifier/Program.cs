using System;

class Program
{
    static void Main(string[] args)
    {
        var firstDate = Console.ReadLine();
        var secondDate = Console.ReadLine();

        var dates = new DateModifier(firstDate, secondDate);

        var totalDifference = dates.CalculateDifference();

        Console.WriteLine(Math.Abs(totalDifference));
    }
}