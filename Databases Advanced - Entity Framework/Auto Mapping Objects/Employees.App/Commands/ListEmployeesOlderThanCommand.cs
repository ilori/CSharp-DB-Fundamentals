namespace Employees.App.Commands
{
    using Services;
    using DtoModels;
    using System.Collections.Generic;
    using System.Text;
    using System;
    using System.Linq;
    using Contracts;

    class ListEmployeesOlderThanCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public ListEmployeesOlderThanCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            var age = int.Parse(args[0]);

            var employees = employeeService.OlderThan(age);

            if (employees == null)
            {
                throw new ArgumentException($"No employees older than {age} age.");
            }

            var output = new StringBuilder();

            var orderedEmployees = employees
                .OrderByDescending(e => e.Salary);

            foreach (var emp in orderedEmployees)
            {
                output.Append($"{emp.FirstName} {emp.LastName} - ${emp.Salary:F2} - Manager: ");

                output.Append(emp.Manager == null ? "[no manager]" : emp.Manager.LastName);
                output.Append(Environment.NewLine);
            }

            return output.ToString().TrimEnd();
        }
    }
}