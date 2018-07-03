using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
class Program
{
    static void Main()
    {
        var n = int.Parse(Console.ReadLine());
        var persons = new List<Person>();
        for (int i = 0; i < n; i++)
        {
            var input = Console.ReadLine().Split();
            var name = input[0];
            var age = int.Parse(input[1]);
            var person = new Person(age,name);
            persons.Add(person);
        }

        foreach (var person in persons.OrderBy(x=>x.Name).Where(x=>x.Age > 30))
        {
            Console.WriteLine($"{person.Name} - {person.Age}");
        }
    }
}