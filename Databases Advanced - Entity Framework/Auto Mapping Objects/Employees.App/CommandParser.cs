namespace Employees.App
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Commands.Contracts;

    internal class CommandParser
    {
        public static ICommand Parse(IServiceProvider serviceProvider, string commandName)
        {
            var assembley = Assembly.GetExecutingAssembly();

            var commandTypes = assembley.GetTypes().Where(e => e.GetInterfaces().Contains(typeof(ICommand)));

            var commandType = commandTypes.SingleOrDefault(x => x.Name == $"{commandName}Command");

            if (commandType == null)
            {
                throw new InvalidOperationException("Invalid Command.");
            }

            var constructor = commandType.GetConstructors().First();

            var constructorParams = constructor.GetParameters().Select(x => x.ParameterType);

            var constructorArgs = constructorParams.Select(serviceProvider.GetService).ToArray();

            var command = (ICommand) constructor.Invoke(constructorArgs);

            return command;
        }
    }
}