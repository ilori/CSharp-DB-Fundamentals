namespace Employees.App
{
    using System;
    using AutoMapper;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Services;

    public class Application
    {


        public static void Main()
        {
            var context = new EmployeeDbContext();

            var serviceProvider = ConfigureServices();

            var engine = new Engine(serviceProvider);

            engine.Run();
        }


        private static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<EmployeeDbContext>(cfg => cfg.UseSqlServer(Configuration.ConnectionString));

            serviceCollection.AddTransient<EmployeeService>();

            serviceCollection.AddAutoMapper(cfg => cfg.AddProfile<EmployeesProfile>());

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}