namespace Employees.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using DtoModels;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class EmployeeService
    {
        private readonly EmployeeDbContext context;


        public EmployeeService(EmployeeDbContext context)
        {
            this.context = context;
        }

        public EmployeeDto EmployeeById(int id)
        {
            var employee = context.Employees.Find(id);

            var employeeDto = Mapper.Map<EmployeeDto>(employee);

            return employeeDto;
        }

        public void AddEmployee(EmployeeDto dto)
        {
            var employee = Mapper.Map<Employee>(dto);

            context.Employees.Add(employee);

            context.SaveChanges();
        }

        public string SetBirthday(int employeeId, DateTime birthdayDate)
        {
            var employee = context.Employees.Find(employeeId);

            employee.BirthDay = birthdayDate;

            context.SaveChanges();

            return $"{employee.FirstName} {employee.LastName}";
        }

        public string SetAddress(int employeeId, string address)
        {
            var employee = context.Employees.Find(employeeId);

            employee.Address = address;

            context.SaveChanges();

            return $"{employee.FirstName} {employee.LastName}";
        }

        public EmployeePersonalDto PersonalById(int employeeId)
        {
            var employee = context.Employees.Find(employeeId);

            var employeeDto = Mapper.Map<EmployeePersonalDto>(employee);

            return employeeDto;
        }

        public EmployeePersonalDto SetManager(int employeeId, int managerId)
        {
            var employee = context.Employees
                .Find(employeeId);

            var manager = context.Employees
                .Find(managerId);

            employee = manager;

            context.SaveChanges();

            var employeePersonalDto = Mapper.Map<EmployeePersonalDto>(employee);

            return employeePersonalDto;
        }

        public ManagerDto GetManager(int managerId)
        {
            var employee = context.Employees
                .Include(m => m.ManagerEmployees)
                .SingleOrDefault(m => m.Id == managerId);

            var managerDto = Mapper.Map<ManagerDto>(employee);

            return managerDto;
        }

        public List<EmployeeManagerDto> OlderThan(int age)
        {
            var employees = context.Employees
                .Where(e => e.BirthDay != null && Math.Floor((DateTime.Now - e.BirthDay.Value).TotalDays / 365) > age)
                .Include(e => e.Manager)
                .ProjectTo<EmployeeManagerDto>()
                .ToList();

            return employees;
        }
    }
}