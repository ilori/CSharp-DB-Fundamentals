namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class CreateAlbumCommand
    {
        // CreateAlbum <username> <albumTitle> <BgColor> <tag1> <tag2>...<tagN>
        public static string Execute(string[] data)
        {
            var userName = data[0];

            var albumName = data[1];

            var colorName = data[2];

            var tagNames = data.Skip(3).ToList();

            using (var db = new PhotoShareContext())
            {
                if (!db.Users.Any(x => x.Username == userName))
                {
                    throw new ArgumentException($"User {userName} not found!");
                }

                if (db.Albums.Any(x => x.Name == albumName))
                {
                    throw new ArgumentException($"Album {albumName} exists!");
                }

                if (!Enum.TryParse(colorName, out Color color))
                {
                    throw new ArgumentException($"Color {colorName} not found!");
                }


                if (tagNames.Any(tag => !db.Tags.Any(x => x.Name == tag)))
                {
                    throw new ArgumentException($"Invalid tags!");
                }

                var tags = db.Tags
                    .Where(x => tagNames.Any(tag => tag == x.Name))
                    .ToList();

                var album = new Album()
                {
                    Name = albumName,
                    BackgroundColor = color
                };

                var user = db.Users
                    .SingleOrDefault(x => x.Username == userName);

                var albumRole = new AlbumRole()
                {
                    User = user,
                    Album = album,
                    Role = Role.Owner
                };

                foreach (var tag in tags)
                {
                    var albumTags = new AlbumTag()
                    {
                        Album = album,
                        Tag = tag
                    };
                    album.AlbumTags.Add(albumTags);
                }

                user.AlbumRoles.Add(albumRole);
                db.SaveChanges();
            }


            return $"Album {albumName} successfully created!";
        }
    }
}