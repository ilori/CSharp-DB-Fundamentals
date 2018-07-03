namespace Employees.App.Commands
{
    using Contracts;
    using Services;

    class SetAddressCommand : ICommand
    {
        private readonly EmployeeService employeeService;

        public SetAddressCommand(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public string Execute(params string[] args)
        {
            var employeeId = int.Parse(args[0]);
            var address = args[1];

            var employeeName = employeeService.SetAddress(employeeId, address);

            return $"{employeeName}'s address was set to {args[1]}";
        }
    }
}