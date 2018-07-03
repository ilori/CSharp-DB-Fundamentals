namespace p11_FindLatest10Projects
{
    using System;
    using System.Globalization;
    using System.Linq;
    using P02_DatabaseFirst.Data;

    class Program
    {
        static void Main()
        {
            using (var db = new SoftUniContext())
            {
                var projects = db.Projects
                    .OrderByDescending(x => x.StartDate)
                    .Take(10)
                    .Select(x => new
                        {
                            x.Name,
                            Desc = x.Description,
                            Date = $"{x.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)}"
                        }
                    )
                    .OrderBy(x => x.Name)
                    .ToList();

                foreach (var p in projects)
                {
                    Console.WriteLine($"{p.Name}");
                    Console.WriteLine($"{p.Desc}");
                    Console.WriteLine($"{p.Date}");
                }
            }
        }
    }
}