namespace PhotoShare.Models
{
    using System.Collections.Generic;
    using PhotoShare.Models.Validation;

    public class Tag
    {
        public int Id { get; set; }

        [Tag]
        public string Name { get; set; }

        public ICollection<AlbumTag> AlbumTags { get; set; } = new HashSet<AlbumTag>();

        public override string ToString()
        {
            return $"{this.Name}";
        }
    }
}