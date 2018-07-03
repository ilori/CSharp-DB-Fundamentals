namespace p05_EmployeesFromResearchAndDevelopment
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
                var employees = db.Employees
                    .Include(x => x.Department)
                    .Where(x => x.Department.Name == "Research and Development")
                    .OrderBy(x => x.Salary)
                    .ThenByDescending(x => x.FirstName)
                    .Select(x => new
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        DepName = x.Department.Name,
                        Salary = x.Salary
                    })
                    .ToList();

                foreach (var e in employees)
                {
                    Console.WriteLine($"{e.FirstName} {e.LastName} from {e.DepName} - ${e.Salary:f2}");
                }
            }
        }
    }
}
