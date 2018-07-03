namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Data;
    using Models;

    public class AddTagToCommand
    {
        // AddTagTo <albumName> <tag>
        public static string Execute(string[] data)
        {
            var albumName = data[0];

            var tagName = data[1];

            using (var db = new PhotoShareContext())
            {
                if (!db.Tags.Any(x => x.Name == tagName) || !db.Albums.Any(x => x.Name == albumName))
                {
                    throw new ArgumentException($"Either tag or album do not exist!");
                }

                var album = db.Albums
                    .SingleOrDefault(x => x.Name == albumName);

                var tag = db.Tags
                    .SingleOrDefault(x => x.Name == tagName);

                var albumTag = new AlbumTag()
                {
                    Album = album,
                    Tag = tag
                };

                db.AlbumTags.Add(albumTag);

                db.SaveChanges();
            }

            return $"Tag {tagName} added to {albumName}!";
        }
    }
}