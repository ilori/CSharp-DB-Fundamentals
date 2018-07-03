namespace Products.Data.EntityConfiguration
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(e => e.Id);

            builder.HasOne(e => e.Buyer)
                .WithMany(e => e.ProductsBought)
                .HasForeignKey(e => e.BuyerId);


            builder.HasOne(e => e.Seller)
                .WithMany(e => e.ProductsSold)
                .HasForeignKey(e => e.SellerId);
        }
    }
}