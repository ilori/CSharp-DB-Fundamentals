namespace P01_BillsPaymentSystem.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.UserId);

            builder.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Email)
                .HasMaxLength(80)
                .IsRequired()
                .IsUnicode(false);


            builder.Property(e => e.Password)
                .HasMaxLength(25)
                .IsRequired()
                .IsUnicode(false);
        }
    }
}