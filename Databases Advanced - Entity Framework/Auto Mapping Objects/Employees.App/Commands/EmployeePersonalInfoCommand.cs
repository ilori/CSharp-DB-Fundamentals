﻿namespace Employees.App.Commands
{
    using System;
    using Contracts;
    using Services;

    public class EmployeePersonalInfoCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public EmployeePersonalInfoCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            var employeeId = int.Parse(args[0]);

            var employee = employeeService.PersonalById(employeeId);

            var birthday = "[no birthday specified]";

            if (employee.BirthDay != null)
            {
                birthday = employee.BirthDay.Value.ToString("dd-MM-yyyy");
            }

            var address = employee.Address ?? "[no address specified]";

            var result =
                $"ID: {employeeId} - {employee.FirstName} {employee.LastName} - ${employee.Salary:F2}{Environment.NewLine}" +
                $"Birthday: {birthday}{Environment.NewLine}" +
                $"Address: {address}";

            return result;
        }
    }
}