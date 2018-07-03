namespace Employees.App.Commands
{
    using System;
    using Contracts;

    internal class ExitCommand : ICommand
    {
        public string Execute(params string[] args)
        {
            Console.WriteLine("Bye bye !");

            Environment.Exit(0);

            return null;
        }
    }
}