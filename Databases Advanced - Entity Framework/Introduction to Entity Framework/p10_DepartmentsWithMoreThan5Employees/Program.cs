namespace p10_DepartmentsWithMoreThan5Employees
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using P02_DatabaseFirst.Data;

    class Program
    {
        static void Main()
        {
            var path = @"text.txt";
            using (var db = new SoftUniContext())
            {
                if (!File.Exists(path))
                {
                    using (var sw = File.CreateText(path))
                    {
                        var deparments = db.Departments
                            .Include(x => x.Employees)
                            .Include(x => x.Manager)
                            .Where(x => x.Employees.Count > 5)
                            .OrderBy(x => x.Employees.Count)
                            .ThenBy(x => x.Name)
                            .ToList();
                        foreach (var d in deparments)
                        {
                            sw.WriteLine($"{d.Name} - {d.Manager.FirstName} {d.Manager.LastName}");
                            foreach (var e in d.Employees.OrderBy(x => x.FirstName).ThenBy(x => x.LastName))
                            {
                                sw.WriteLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                            }
                            sw.WriteLine("----------");
                        }
                    }
                }
                
            }
        }
    }
}