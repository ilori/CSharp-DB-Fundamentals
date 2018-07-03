namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class AcceptFriendCommand
    {
        // AcceptFriend UserToAccept RequestedUser
        public static string Execute(string[] data)
        {
            var firstUser = data[0];

            var secondUser = data[1];


            using (var db = new PhotoShareContext())
            {
                var userToAccept = db.Users
                    .Include(x => x.FriendsAdded)
                    .ThenInclude(x => x.Friend)
                    .SingleOrDefault(x => x.Username == firstUser);

                if (userToAccept == null)
                {
                    throw new ArgumentException($"{firstUser} not found!");
                }


                var requestedUser = db.Users
                    .Include(x => x.FriendsAdded)
                    .ThenInclude(x => x.Friend)
                    .SingleOrDefault(x => x.Username == secondUser);

                if (requestedUser == null)
                {
                    throw new ArgumentException($"{secondUser} not found!");
                }

                if (userToAccept.FriendsAdded.Any(x => x.Friend == requestedUser) &&
                    requestedUser.FriendsAdded.Any(x => x.Friend == userToAccept))
                {
                    throw new InvalidOperationException($"{secondUser} is already a friend to {firstUser}");
                }
                if (requestedUser.FriendsAdded.All(x => x.Friend != userToAccept))
                {
                    throw new InvalidOperationException($"{secondUser} has not added {firstUser} as a friend");
                }

                userToAccept.FriendsAdded.Add(new Friendship()
                {
                    User = userToAccept,
                    Friend = requestedUser
                });

                db.SaveChanges();
            }
            return $"{firstUser} accepted {secondUser} as a friend";
        }
    }
}