namespace Employees.App.Commands
{
    using Contracts;
    using Services;

    public class EmployeeInfoCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public EmployeeInfoCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            var employeeId = int.Parse(args[0]);


            var employee = employeeService.EmployeeById(employeeId);

            return $"ID: {employee.Id} - {employee.FirstName} {employee.LastName} - ${employee.Salary:f2}";
        }
    }
}