namespace Products.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class CategoryProductConfiguration : IEntityTypeConfiguration<CategoryProduct>
    {
        public void Configure(EntityTypeBuilder<CategoryProduct> builder)
        {
            builder.HasKey(e => new {e.ProductId, e.CategoryId});

            builder.HasOne(e => e.Product)
                .WithMany(e => e.ProductsCategory)
                .HasForeignKey(e => e.ProductId);

            builder.HasOne(e => e.Category)
                .WithMany(e => e.CategoryProducts)
                .HasForeignKey(e => e.CategoryId);
        }
    }
}