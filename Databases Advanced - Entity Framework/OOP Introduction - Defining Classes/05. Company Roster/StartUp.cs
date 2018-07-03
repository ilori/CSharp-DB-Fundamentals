using System;
using System.Collections.Generic;
using System.Linq;

public class StartUp
{
    public static void Main()
    {
        var employees = new List<Emoployee>();

        var n = int.Parse(Console.ReadLine());


        for (int i = 0; i < n; i++)
        {
            var input = Console.ReadLine().Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            var name = input[0];
            var salary = decimal.Parse(input[1]);
            var position = input[2];
            var department = input[3];
            var employee = new Emoployee(name, salary, position, department);
            if (input.Length > 5)
            {
                var email = input[4];
                var age = int.Parse(input[5]);

                employee.Age = age;
                employee.Email = email;
            }
            if (input.Length > 4)
            {
                var emailOrAge = input[4];
                if (emailOrAge.Contains("@"))
                {
                    employee.Email = emailOrAge;
                }
                else
                {
                    employee.Age = int.Parse(emailOrAge);
                }
            }
            employees.Add(employee);
        }

        var result = employees
            .GroupBy(x => x.Department)
            .Select(x => new
            {
                Department = x.Key,
                AverageSalary = x.Average(emp => emp.Salary),
                Employees = x.OrderByDescending(emp => emp.Salary)
            })
            .OrderByDescending(x => x.AverageSalary)
            .FirstOrDefault();
        Console.WriteLine($"Highest Average Salary: {result.Department}");

        foreach (var employee in result.Employees)
        {
            Console.WriteLine($"{employee.Name} {employee.Salary:F2} {employee.Email} {employee.Age}");
        }
    }
}