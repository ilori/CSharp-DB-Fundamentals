namespace p08_AddressesByTown
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using P02_DatabaseFirst.Data;
    class Program
    {
        static void Main()
        {
            using (var db = new SoftUniContext())
            {
                var addresses = db.Addresses
                    .Include(x=>x.Employees)
                    .Include(x => x.Town)
                    .OrderByDescending(x => x.Employees.Count)
                    .ThenBy(x => x.Town.Name)
                    .ThenBy(x => x.AddressText)
                    .Take(10)
                    .ToList();

                foreach (var a in addresses)
                {
                    Console.WriteLine($"{a.AddressText}, {a.Town.Name} - {a.Employees.Count} employees");
                }

            }
        }
    }
}