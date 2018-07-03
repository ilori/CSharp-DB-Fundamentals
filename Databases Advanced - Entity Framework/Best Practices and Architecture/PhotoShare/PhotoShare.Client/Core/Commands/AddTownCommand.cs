namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Models;
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class AddTownCommand
    {
        // AddTown <townName> <countryName>
        public static string Execute(string[] data)
        {
            var townName = data[0];
            var country = data[1];

            using (var db = new PhotoShareContext())
            {
                var townToFind = db.Towns
                    .AsNoTracking()
                    .Any(x => x.Name == townName);

                if (townToFind)
                {
                    throw new ArgumentException($"Town {townName} was already added!");
                }

                var town = new Town
                {
                    Name = townName,
                    Country = country
                };

                db.Towns.Add(town);
                db.SaveChanges();

                return $"Town {townName} was added successfully!";
            }
        }
    }
}