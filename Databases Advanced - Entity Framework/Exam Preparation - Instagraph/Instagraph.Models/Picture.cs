﻿namespace Instagraph.Models
{
    using System.Collections.Generic;

    public class Picture
    {
        public int Id { get; set; }

        public string Path { get; set; }

        public decimal Size { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<Post> Posts{ get; set; } = new List<Post>();
    }
}
