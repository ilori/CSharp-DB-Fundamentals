namespace PetClinic.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;
    public class AnimalConfig : IEntityTypeConfiguration<Animal>
    {
        public void Configure(EntityTypeBuilder<Animal> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Passport)
                .WithOne(x => x.Animal)
                .HasForeignKey<Animal>(x => x.PassportSerialNumber);
        }
    }
}