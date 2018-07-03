using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoShare.Client.Core.Commands
{
    using System.Linq;
    using Data;

    public class LogoutCommand
    {
        public static string Execute(string[] data)
        {
            var userName = data[0];

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
                    throw new InvalidOperationException($"You should log in first in order to logout");
                }

                user.IsLogged = false;

                db.SaveChanges();
            }
            return $"User {userName} successfully logged out!";
        }
    }
}