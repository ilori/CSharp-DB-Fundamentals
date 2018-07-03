namespace p09_Employee147
{
    using System;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using P02_DatabaseFirst.Data;

    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new SoftUniContext())
            {
                var employee = db.Employees
                    .Where(x => x.EmployeeId == 147)
                    .Include(x => x.EmployeesProjects)
                    .Select(x => new
                    {
                        FullName = $"{x.FirstName} {x.LastName}",
                        JobTitle = x.JobTitle,
                        Projects = x.EmployeesProjects.Select(ep => ep.Project).ToList()
                    })
                    .ToList();

                foreach (var e in employee)
                {
                    Console.WriteLine($"{e.FullName} - {e.JobTitle}");
                    foreach (var p in e.Projects.OrderBy(x => x.Name))
                    {
                        Console.WriteLine($"{p.Name}");
                    }
                }
            }
        }
    }
}