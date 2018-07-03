namespace Instagraph.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> model)
        {
            model.HasKey(e => e.Id);

            model.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(30);

            model.HasIndex(e => e.Username)
                .IsUnique();

            model.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(20);

            model.HasOne(e => e.ProfilePicture)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.ProfilePictureId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}