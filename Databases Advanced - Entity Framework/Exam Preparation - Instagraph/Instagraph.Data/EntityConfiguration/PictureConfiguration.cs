namespace Instagraph.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class PictureConfiguration : IEntityTypeConfiguration<Picture>
    {
        public void Configure(EntityTypeBuilder<Picture> model)
        {
            model.HasKey(e => e.Id);

            model.Property(e => e.Path)
                .IsRequired();

            model.Property(e => e.Size)
                .IsRequired();
        }
    }
}