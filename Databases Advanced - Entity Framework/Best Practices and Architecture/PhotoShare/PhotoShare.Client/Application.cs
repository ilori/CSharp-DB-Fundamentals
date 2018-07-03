namespace PhotoShare.Client
{
    using System;
    using Core;
    using Core.Commands;
    using Data;
    using Models;

    public class Application
    {
        public static void Main()
        {
            //ResetDatabase();

            var commandDispatcher = new CommandDispatcher();
            var engine = new Engine(commandDispatcher);
            engine.Run();
        }

        private static void ResetDatabase()
        {
            using (var db = new PhotoShareContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                Console.WriteLine($"Database reseted ! :)");

                Console.WriteLine();

                Console.WriteLine($"----------------------------------------------------");

                Console.WriteLine();
            }
        }
    }
}