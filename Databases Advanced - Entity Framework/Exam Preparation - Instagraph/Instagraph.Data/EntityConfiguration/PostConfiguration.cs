namespace Instagraph.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> model)
        {
            model.HasKey(e => e.Id);

            model.Property(e => e.Caption)
                .IsRequired();

            model.HasOne(e => e.Picture)
                .WithMany(e => e.Posts)
                .HasForeignKey(e => e.PictureId)
                .OnDelete(DeleteBehavior.Restrict);

            model.HasOne(e => e.User)
                .WithMany(e => e.Posts)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}