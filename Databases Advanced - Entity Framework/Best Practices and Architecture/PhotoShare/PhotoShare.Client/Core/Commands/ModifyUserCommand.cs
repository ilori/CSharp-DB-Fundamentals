namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ModifyUserCommand
    {
        // ModifyUser <username> <property> <new value>
        // For example:
        // ModifyUser <username> Password <NewPassword>
        // ModifyUser <username> BornTown <newBornTownName>
        // ModifyUser <username> CurrentTown <newCurrentTownName>
        // !!! Cannot change username
        public static string Execute(string[] data)
        {
            var userName = data[0];
            var property = data[1];
            var newValue = data[2];

            using (var db = new PhotoShareContext())
            {
                var user = db.Users
                    .SingleOrDefault(x => x.Username == userName);


                if (user == null)
                {
                    throw new ArgumentException($"User {userName} not found!");
                }

                if (!user.IsLogged)
                {
                    throw new InvalidOperationException($"Please login first!");
                }

                switch (property)
                {
                    case "Password":
                        if (!newValue.Any(char.IsLower) || !newValue.Any(char.IsDigit))
                        {
                            throw new ArgumentException($"Value {newValue} not valid.\r\nInvalid password");
                        }

                        user.Password = newValue;
                        db.SaveChanges();
                        break;
                    case "BornTown":
                        var bornTown = db.Towns
                            .SingleOrDefault(x => x.Name == newValue);

                        if (bornTown == null)
                        {
                            throw new ArgumentException(
                                $"Value {newValue} not valid.\r\nTown {newValue} not found!");
                        }

                        user.BornTown = bornTown;
                        db.SaveChanges();
                        break;
                    case "CurrentTown":
                        var currentTown = db.Towns
                            .SingleOrDefault(x => x.Name == newValue);

                        if (currentTown == null)
                        {
                            throw new ArgumentException(
                                $"Value {newValue} not valid.\r\nTown {newValue} not found!");
                        }
                        user.CurrentTown = currentTown;
                        db.SaveChanges();
                        break;
                    default:
                        throw new ArgumentException($"Property {property} not supported!");
                }
            }
            return $"User {userName} {property} is {newValue}.";
        }
    }
}