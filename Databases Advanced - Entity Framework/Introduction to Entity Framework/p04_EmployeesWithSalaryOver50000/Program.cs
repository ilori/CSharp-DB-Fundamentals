namespace p04_EmployeesWithSalaryOver50000
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
                    .Where(x => x.Salary > 50000)
                    .Select(x => new
                    {
                        FirstName = x.FirstName
                    })
                    .OrderBy(x => x.FirstName)
                    .ToList();

                foreach (var e in employees)
                {
                    Console.WriteLine(e.FirstName);
                }
            }
        }
    }
}