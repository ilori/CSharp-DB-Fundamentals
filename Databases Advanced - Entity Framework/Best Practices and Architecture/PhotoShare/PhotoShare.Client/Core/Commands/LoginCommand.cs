namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class LoginCommand
    {
        //Login <username> <password>
        public static string Execute(string[] data)
        {
            var userName = data[0];
            var userPassword = data[1];

            using (var db = new PhotoShareContext())
            {
                var user = db.Users
                    .SingleOrDefault(x => x.Username == userName && x.Password == userPassword);


                if (user == null)
                {
                    throw new ArgumentException($"Invalid username or password!");
                }

                if (user.IsLogged)
                {
                    throw new ArgumentException($"You should logout first!");
                }

                user.IsLogged = true;

                db.SaveChanges();
            }

            return $"User {userName} successfully logged in!";
        }
    }
}