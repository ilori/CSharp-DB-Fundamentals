namespace Products.Application
{
    using System;
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class DatabaseTools
    {
        public static void ResetDatabase()
        {
            using (var db = new ProductsDbContext())
            {
                db.Database.EnsureDeleted();
                db.Database.Migrate();
            }

            Console.WriteLine("Database was successfully reseted !");
        }
    }
}