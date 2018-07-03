namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Data;
    using Models;

    public class ShareAlbumCommand
    {
        // ShareAlbum <albumId> <username> <permission>
        // For example:
        // ShareAlbum 4 dragon321 Owner
        // ShareAlbum 4 dragon11 Viewer
        public static string Execute(string[] data)
        {
            var albumId = int.Parse(data[0]);

            var userName = data[1];

            var permission = data[2];

            using (var db = new PhotoShareContext())
            {
                var album = db.Albums.Find(albumId);

                if (album == null)
                {
                    throw new ArgumentException($"Album {albumId} not found!");
                }

                var user = db.Users
                    .SingleOrDefault(x => x.Username == userName);

                if (user == null)
                {
                    throw new ArgumentException($"User {userName} not found!");
                }

                if (!Enum.TryParse(permission, out Role role))
                {
                    throw new ArgumentException($"Permission must be either “Owner” or “Viewer”!");
                }


                var albumRole = new AlbumRole()
                {
                    Album = album,
                    User = user,
                    Role = role
                };

                album.AlbumRoles.Add(albumRole);

                db.SaveChanges();
            }
            return $"Username {userName} added to album {albumId} {permission}";
        }
    }
}