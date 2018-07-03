namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Models;
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class RegisterUserCommand
    {
        // RegisterUser <username> <password> <repeat-password> <email>
        public static string Execute(string[] data)
        {
            var username = data[0];
            var password = data[1];
            var repeatPassword = data[2];
            var email = data[3];


            using (var db = new PhotoShareContext())
            {
                var userToFind = db.Users
                    .AsNoTracking()
                    .Any(x => x.Username == username);

                if (userToFind)
                {
                    throw new InvalidOperationException($"Username {username} is already taken!");
                }

                var user = new User
                {
                    Username = username,
                    Password = password,
                    Email = email,
                    IsDeleted = false,
                    RegisteredOn = DateTime.Now,
                    LastTimeLoggedIn = DateTime.Now
                };

                if (password != repeatPassword)
                {
                    throw new ArgumentException("Passwords do not match!");
                }

                db.Users.Add(user);
                db.SaveChanges();

                return $"User {username} was registered successfully!";
            }
        }
    }
}