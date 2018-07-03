namespace p13_FindEmployeesByFirstNameStartingWithSa
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
                    .Where(x => x.FirstName.StartsWith("Sa"))
                    .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName)
                    .ToList();

                foreach (var e in employees)
                {
                    Console.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:F2})");
                }

            }
        }
    }
}
