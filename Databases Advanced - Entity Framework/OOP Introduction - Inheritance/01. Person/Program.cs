using System;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var name = Console.ReadLine();
            var age = int.Parse(Console.ReadLine());
            var child = new Child(name, age);
            Console.WriteLine(child);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}