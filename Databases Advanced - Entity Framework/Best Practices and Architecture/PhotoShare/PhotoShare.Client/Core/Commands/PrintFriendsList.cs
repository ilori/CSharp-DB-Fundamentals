namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class PrintFriendsListCommand
    {
        // PrintFriendsList <username>
        public static string Execute(string[] data)
        {
            var userName = data[0];
            var sb = new StringBuilder();

            using (var db = new PhotoShareContext())
            {
                if (!db.Users.Any(x => x.Username == userName))
                {
                    throw new ArgumentException($"User {userName} not found!");
                }

                var user = db.Users
                    .Include(x => x.FriendsAdded)
                    .ThenInclude(x => x.Friend)
                    .SingleOrDefault(x => x.Username == userName);


                if (user.FriendsAdded.Count == 0)
                {
                    return $"No friends for this user. :(";
                }

                sb.AppendLine("Friends:");
                foreach (var friend in user.FriendsAdded)
                {
                    sb.AppendLine($"-{friend.Friend.Username}");
                }
            }
            return sb.ToString();
        }
    }
}