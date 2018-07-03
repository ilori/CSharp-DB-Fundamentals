namespace PetClinic.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;
    public class ProceduresAnimalConfig : IEntityTypeConfiguration<ProcedureAnimalAid>
    {
        public void Configure(EntityTypeBuilder<ProcedureAnimalAid> builder)
        {
            builder.HasKey(x => new {x.ProcedureId, x.AnimalAidId});

            builder.HasOne(x => x.Procedure)
                .WithMany(x => x.ProcedureAnimalAids)
                .HasForeignKey(x => x.ProcedureId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AnimalAid)
                .WithMany(x => x.AnimalAidProcedures)
                .HasForeignKey(x => x.AnimalAidId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}