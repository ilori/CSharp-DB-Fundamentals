using Microsoft.EntityFrameworkCore;

namespace Instagraph.Data
{
    using EntityConfiguration;
    using Models;

    public class InstagraphContext : DbContext
    {
        public InstagraphContext()
        {
        }

        public InstagraphContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserFollower> UsersFollowers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.ApplyConfiguration(new PictureConfiguration());
            model.ApplyConfiguration(new PostConfiguration());
            model.ApplyConfiguration(new CommentConfiguration());
            model.ApplyConfiguration(new UserConfiguration());
            model.ApplyConfiguration(new UserFollowersConfiguration());
        }
    }
}