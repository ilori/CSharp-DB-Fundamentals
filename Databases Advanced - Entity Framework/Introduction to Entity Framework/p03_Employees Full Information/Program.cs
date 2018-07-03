namespace p03_Employees_Full_Information
{
    using System;
    using System.Linq;
    using P02_DatabaseFirst.Data;

    class Program
    {
        static void Main()
        {
            var db = new SoftUniContext();
            using (db)
            {
                var employees = db.Employees
                    .OrderBy(x => x.EmployeeId)
                    .ToList();

                foreach (var e in employees)
                {
                    Console.WriteLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}");
                }
            }
        }
    }
}