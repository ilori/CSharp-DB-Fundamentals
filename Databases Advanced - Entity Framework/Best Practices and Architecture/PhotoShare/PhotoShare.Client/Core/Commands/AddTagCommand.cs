namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Models;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Utilities;

    public class AddTagCommand
    {
        // AddTag <tag>
        public static string Execute(string[] data)
        {
            var tag = data[0].ValidateOrTransform();

            using (var db = new PhotoShareContext())
            {
                var tagToSearch = db.Tags
                    .AsNoTracking()
                    .SingleOrDefault(x => x.Name == tag);

                if (tagToSearch != null)
                {
                    throw new ArgumentException($"Tag {tag} exists!");
                }

                db.Tags.Add(new Tag
                {
                    Name = tag
                });

                db.SaveChanges();
            }

            return $"Tag {tag} was added successfully!";
        }
    }
}