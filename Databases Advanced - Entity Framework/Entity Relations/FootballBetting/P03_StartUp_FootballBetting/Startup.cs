namespace P03_StartUp_FootballBetting
{
    using Microsoft.EntityFrameworkCore;
    using P03_FootballBetting.Data;

    public class Startup
    {
        static void Main()
        {
            var context = new FootballBettingContext();

            context.Database.EnsureDeleted();

            context.Database.Migrate();
        }
    }

}
