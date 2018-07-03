using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleApplication;

public class Program
{
    public static void Main(string[] args)
    {
        var animals = new List<Animal>();
        string input;

        while ((input = Console.ReadLine()) != "Beast!")
        {
            var tokens = Console.ReadLine().Split(' ').ToList();
            var name = tokens[0];
            var age = int.Parse(tokens[1]);
            var gender = tokens[2];

            try
            {
                if (input == "Cat")
                {
                    var cat = new Cat(name, age, gender);
                    animals.Add(cat);
                }
                else if (input == "Dog")
                {
                    var dog = new Dog(name, age, gender);
                    animals.Add(dog);
                }
                else if (input == "Frog")
                {
                    var frog = new Frog(name, age, gender);
                    animals.Add(frog);
                }
                else if (input == "Kitten")
                {
                    var kitten = new Kitten(name, age, gender);
                    animals.Add(kitten);
                }
                else if (input == "Tomcat")
                {
                    var tomcat = new Tomcat(name, age, gender);
                    animals.Add(tomcat);
                }
                else
                {
                    throw new ArgumentException("Invalid input!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        foreach (var animal in animals)
        {
            Console.WriteLine(animal);
        }
    }
}