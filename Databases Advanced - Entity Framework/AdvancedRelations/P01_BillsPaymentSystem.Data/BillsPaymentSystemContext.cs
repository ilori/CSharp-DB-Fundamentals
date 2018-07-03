namespace P01_BillsPaymentSystem.Data
{
    using EntityConfiguration;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class BillsPaymentSystemContext : DbContext
    {
        public BillsPaymentSystemContext()
        {
        }

        public BillsPaymentSystemContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(Configuration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.ApplyConfiguration(new CreditCardConfiguration());

            model.ApplyConfiguration(new UserConfiguration());

            model.ApplyConfiguration(new BankAccountConfiguration());

            model.ApplyConfiguration(new PaymentMethodConfiguration());
        }
    }
}