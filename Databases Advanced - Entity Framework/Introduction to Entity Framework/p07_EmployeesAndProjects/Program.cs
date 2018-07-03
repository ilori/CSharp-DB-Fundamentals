namespace p07_EmployeesAndProjects
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using P02_DatabaseFirst.Data;

    class Program
    {
        static void Main()
        {
            using (var db = new SoftUniContext())
            {
                var employees = db.Employees
                    .Where(x => x.EmployeesProjects.Any(ep =>
                        ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                    .Take(30)
                    .Select(x => new
                    {
                        EmpFirstName = x.FirstName,
                        EmpLastName = x.LastName,
                        ManagerFirstName = x.Manager.FirstName,
                        ManagerLastName = x.Manager.LastName,
                        Projects = x.EmployeesProjects.Select(ep => ep.Project).ToList()
                    })
                    .ToList();

                foreach (var e in employees)
                {
                    Console.WriteLine(
                        $"{e.EmpFirstName} {e.EmpLastName} – Manager: {e.ManagerFirstName} {e.ManagerLastName}");

                    foreach (var p in e.Projects)
                    {
                        Console.WriteLine(
                            $"--{p.Name} - {p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - {(p.EndDate == null ? "not finished" : p.EndDate.ToString())}");
                    }
                }
            }
        }
    }
}