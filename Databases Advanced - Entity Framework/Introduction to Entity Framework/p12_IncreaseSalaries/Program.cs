namespace p12_IncreaseSalaries
{
    using System;
    using System.Linq;
    using P02_DatabaseFirst.Data;

    class Program
    {
        static void Main()
        {
            using (var db = new SoftUniContext())
            {
                var employees = db.Employees
                    .Where(x => x.Department.Name == "Engineering" || x.Department.Name == "Tool Design" ||
                                x.Department.Name == "Marketing" || x.Department.Name == "Information Services")
                    .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName)
                    .ToList();

                foreach (var e in employees)
                {
                    e.Salary *= 1.12M;
                    Console.WriteLine($"{e.FirstName} {e.LastName} (${e.Salary:F2})");
                }

                db.SaveChanges();
            }
        }
    }
}