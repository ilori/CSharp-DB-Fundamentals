namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class AddFriendCommand
    {
        // AddFriend <username1> <username2>
        public static string Execute(string[] data)
        {
            var userRequest = data[0];

            var userToAccept = data[1];

            using (var db = new PhotoShareContext())
            {
                var firstUser = db.Users
                    .Include(x => x.FriendsAdded)
                    .SingleOrDefault(x => x.Username == userRequest);

                if (firstUser == null)
                {
                    throw new ArgumentException($"{userRequest} not found!");
                }

                if (!firstUser.IsLogged)
                {
                    throw new InvalidOperationException($"Please login first!");
                }
                var secondUser = db.Users
                    .Include(x => x.FriendsAdded)
                    .ThenInclude(x => x.Friend)
                    .SingleOrDefault(x => x.Username == userToAccept);

                if (secondUser == null)
                {
                    throw new ArgumentException($"{userToAccept} not found!");
                }

                if (firstUser.FriendsAdded.Any(x => x.Friend == secondUser) &&
                    secondUser.FriendsAdded.Any(x => x.Friend == firstUser))
                {
                    throw new InvalidOperationException($"{userToAccept} is already friend to {userRequest}");
                }

                if (firstUser.FriendsAdded.Any(x => x.Friend == secondUser))
                {
                    throw new ArgumentException($"{userRequest} already sent request to {userToAccept}");
                }

                firstUser.FriendsAdded.Add(new Friendship()
                {
                    User = firstUser,
                    Friend = secondUser
                });

                db.SaveChanges();
            }
            return $"Friend {userToAccept} added to {userRequest}";
        }
    }
}