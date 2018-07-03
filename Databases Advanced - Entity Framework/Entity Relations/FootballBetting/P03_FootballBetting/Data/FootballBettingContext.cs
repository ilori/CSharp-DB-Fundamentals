namespace P03_FootballBetting.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
        {
        }

        public FootballBettingContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Bet> Bets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Country> Countries { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(@"Server=DESKTOP-ELJB4JK\SQLEXPRESS;Database=Sales;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.TeamId);

                entity.HasOne(e => e.PrimaryKitColor)
                    .WithMany(e => e.PrimaryKitTeams)
                    .HasForeignKey(e=>e.PrimaryKitColorId);

                entity.HasOne(e => e.SecondaryKitColor)
                    .WithMany(e => e.SecondaryKitTeams)
                    .HasForeignKey(e=>e.SecondaryKitColorId);

                entity.HasOne(e => e.Town)
                    .WithMany(e => e.Teams)
                    .HasForeignKey(e => e.TownId);
            });

            model.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.Username)
                    .IsRequired(false);

                entity.Property(e => e.Password)
                    .IsRequired(false);

                entity.Property(e => e.Email)
                    .IsRequired(false);

                entity.Property(e => e.Name)
                    .IsRequired(false);

            });

            model.Entity<Color>(entity =>
            {
                entity.HasKey(e => e.ColorId);

                entity.HasMany(e => e.PrimaryKitTeams)
                    .WithOne(e => e.PrimaryKitColor)
                    .HasForeignKey(e => e.PrimaryKitColorId);

                entity.HasMany(e => e.SecondaryKitTeams)
                    .WithOne(e => e.SecondaryKitColor)
                    .HasForeignKey(e => e.SecondaryKitColorId);
            });

            model.Entity<Game>(entity =>
            {
                entity.HasKey(e => e.GameId);

                entity.HasOne(e => e.HomeTeam)
                    .WithMany(e => e.HomeGames)
                    .HasForeignKey(e => e.HomeTeamId);


                entity.HasOne(e => e.AwayTeam)
                    .WithMany(e => e.AwayGames)
                    .HasForeignKey(e => e.AwayTeamId);
            });

            model.Entity<Town>(entity =>
            {
                entity.HasKey(e => e.TownId);

                entity.HasOne(e => e.Country)
                    .WithMany(e => e.Towns)
                    .HasForeignKey(e => e.CountryId);
            });

            model.Entity<Player>(entity =>
            {
                entity.HasKey(e => e.PlayerId);

                entity.HasOne(e => e.Team)
                    .WithMany(e => e.Players)
                    .HasForeignKey(e => e.TeamId);

                entity.HasOne(e => e.Position)
                    .WithMany(e => e.Players)
                    .HasForeignKey(e => e.PositionId);
            });

            model.Entity<PlayerStatistic>(entity =>
            {
                entity.HasKey(e => new {e.PlayerId, e.GameId});

                entity.HasOne(e => e.Game)
                    .WithMany(e => e.PlayerStatistics)
                    .HasForeignKey(e => e.GameId);

                entity.HasOne(e => e.Player)
                    .WithMany(e => e.PlayerStatistics)
                    .HasForeignKey(e => e.PlayerId);
            });

            model.Entity<Bet>(entity =>
            {
                entity.HasKey(e => e.BetId);

                entity.Property(e => e.Prediction)
                    .IsRequired();

                entity.Property(e => e.DateTime)
                    .IsRequired()
                    .HasColumnType("DATE2")
                    .HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.User)
                    .WithMany(e => e.Bets)
                    .HasForeignKey(e => e.UserId);

                entity.HasOne(e => e.Game)
                    .WithMany(e => e.Bets)
                    .HasForeignKey(e => e.GameId);
            });

            model.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryId);

                entity.Property(e => e.Name)
                    .IsRequired();
            });

            model.Entity<Position>(entity =>
            {
                entity.HasKey(e => e.PositionId);

                entity.Property(e => e.Name)
                    .IsRequired(false);
            });

        }
    }
}