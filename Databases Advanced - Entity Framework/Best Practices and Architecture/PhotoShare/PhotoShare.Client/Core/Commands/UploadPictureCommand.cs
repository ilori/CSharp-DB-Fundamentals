namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Data;
    using Models;

    public class UploadPictureCommand
    {
        // UploadPicture <albumName> <pictureTitle> <pictureFilePath>
        public static string Execute(string[] data)
        {
            var albumName = data[0];
            var pictureName = data[1];
            var picturePath = data[2];

            using (var db = new PhotoShareContext())
            {
                var album = db.Albums
                    .SingleOrDefault(x => x.Name == albumName);

                if (album == null)
                {
                    throw new ArgumentException($"Album {albumName} not found!");
                }
                album.Pictures.Add(new Picture()
                {
                    Title = pictureName,
                    Path = picturePath
                });

                db.SaveChanges();
            }
            return $"Picture {pictureName} added to {albumName}!";
        }
    }
}