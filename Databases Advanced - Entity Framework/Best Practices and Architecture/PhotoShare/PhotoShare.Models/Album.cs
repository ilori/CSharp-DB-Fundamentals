namespace PhotoShare.Models
{
    using System.Collections.Generic;

    public class Album
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Color? BackgroundColor { get; set; }

        public bool IsPublic { get; set; }

        public ICollection<AlbumRole> AlbumRoles { get; set; } = new HashSet<AlbumRole>();

        public ICollection<Picture> Pictures { get; set; } = new HashSet<Picture>();

        public ICollection<AlbumTag> AlbumTags { get; set; } = new HashSet<AlbumTag>();

        public override string ToString()
        {
            return $"{this.Name} has {this.Pictures.Count} pictures";
        }
    }
}
