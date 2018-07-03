namespace p06_AddingANewAddressAndUpdatingEmployee
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using P02_DatabaseFirst.Data;
    using P02_DatabaseFirst.Data.Models;

    class Program
    {
        static void Main()
        {
            using (var db = new SoftUniContext())
            {
                var address = new Address()
                {
                    AddressText = "Vitoshka 15",
                    TownId = 4
                };
                var employee = db.Employees.SingleOrDefault(x => x.LastName == "Nakov");
                employee.Address = address;

                db.SaveChanges();

                var employees = db.Employees
                    .Include(x => x.Address)
                    .OrderByDescending(x => x.AddressId)
                    .Take(10)
                    .Select(x => new
                    {
                        Text = x.Address.AddressText
                    })
                    .ToList();

                foreach (var e in employees)
                {
                    Console.WriteLine($"{e.Text}");
                }
            }
        }
    }
}