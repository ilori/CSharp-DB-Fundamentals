namespace SalesStartUp
{
    using P03_SalesDatabase.Data;

    public class StartUp
    {
        static void Main()
        {
            using (var db = new SalesContext())
            {
                db.Database.EnsureCreated();
            }
        }
    } 
}