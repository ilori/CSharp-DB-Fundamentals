namespace P01_HospitalDatabase
{
    using Initializer;
    using Data;
    using P01_HospitalDatabase.Data.Models;

    class StartUp
    {
        static void Main()
        {
            using (var db = new HospitalContext())
            {
            }
        }
    }
}