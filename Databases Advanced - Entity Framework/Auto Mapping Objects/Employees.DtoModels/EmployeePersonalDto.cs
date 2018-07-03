namespace Employees.DtoModels
{
    using System;
    using System.Collections.Generic;

    public class EmployeePersonalDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public decimal Salary { get; set; }

        public DateTime? BirthDay { get; set; }

        public string Address { get; set; }

        public EmployeePersonalDto Manager { get; set; }

        public ICollection<EmployeeDto> ManagerEmployees { get; set; } = new HashSet<EmployeeDto>();

        public int ManagerEmployeesCount { get; set; }
    }
}