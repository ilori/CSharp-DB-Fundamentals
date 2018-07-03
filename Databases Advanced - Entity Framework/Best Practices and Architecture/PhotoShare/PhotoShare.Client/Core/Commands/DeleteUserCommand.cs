namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Data;

    public class DeleteUserCommand
    {
        // DeleteUserCommand <username>
        public static string Execute(string[] data)
        {
            var username = data[0];

            using (var db = new PhotoShareContext())
            {
                var user = db.Users.SingleOrDefault(u => u.Username == username);
                if (user == null)
                {
                    throw new ArgumentException($"User {username} was not found!");
                }
                if (user.IsDeleted != null && user.IsDeleted.Value)
                {
                    throw new InvalidOperationException($"User {username} is already deleted!");
                }
                user.IsDeleted = true;
                db.SaveChanges();

                return $"User {username} was deleted successfully!";
            }
        }
    }
}