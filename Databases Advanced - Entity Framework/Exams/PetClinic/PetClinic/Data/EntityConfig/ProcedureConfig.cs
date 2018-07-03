namespace PetClinic.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ProcedureConfig : IEntityTypeConfiguration<Procedure>
    {
        public void Configure(EntityTypeBuilder<Procedure> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Vet)
                .WithMany(x => x.Procedures)
                .HasForeignKey(x => x.VetId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Animal)
                .WithMany(x => x.Procedures)
                .HasForeignKey(x => x.AnimalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Ignore(x => x.Cost);
        }
    }
}