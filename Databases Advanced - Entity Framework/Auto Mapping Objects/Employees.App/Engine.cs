namespace Employees.App
{
    using System;
    using System.Linq;

    internal class Engine
    {
        private readonly IServiceProvider serviceProvider;

        public Engine(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }


        internal void Run()
        {
            while (true)
            {
                var input = Console.ReadLine().Split(' ');

                var commandName = input[0];

                var commandArgs = input.Skip(1).ToArray();

                var command = CommandParser.Parse(serviceProvider, commandName);

                var result = command.Execute(commandArgs);

                Console.WriteLine(result);
            }
        }
    }
}