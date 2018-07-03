namespace p14_DeleteProjectById
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
                var project = db.Projects.Find(2);
                var ep = db.EmployeesProjects.ToList();
                foreach (var e in ep)
                {
                    db.EmployeesProjects.Remove(e);
                }
                db.Projects.Remove(project);
                db.SaveChanges();
                var projects = db.Projects.Take(10).Select(p => p.Name).ToList();
                foreach (var p in projects)
                {
                    Console.WriteLine(p);
                }
            }
        }
    }
}