namespace Instagraph.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using Data;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportUncommentedPosts(InstagraphContext context)
        {
            var posts = context.Posts
                .Where(e => e.Comments.Count == 0)
                .OrderBy(e => e.Id)
                .Select(x => new
                {
                    x.Id,
                    Picture = x.Picture.Path,
                    User = x.User.Username
                })
                .ToList();

            var jsonString = JsonConvert.SerializeObject(posts, Formatting.Indented);


            return jsonString;
        }

        public static string ExportPopularUsers(InstagraphContext context)
        {
            var users = context.Users
                .Where(u => u.Posts.Any(p => p.Comments.Any(c => u.Followers.Any(f => f.FollowerId == c.UserId))))
                .OrderBy(u => u.Id)
                .Select(x => new
                {
                    x.Username,
                    Followers = x.Followers.Count
                })
                .ToList();

            var jsonString = JsonConvert.SerializeObject(users, Formatting.Indented);

            return jsonString;
        }

        public static string ExportCommentsOnPosts(InstagraphContext context)
        {
            var users = context.Users
                .Select(u => new
                {
                    u.Username,
                    PostsCommentCount = u.Posts.Select(p => p.Comments.Count).ToArray()
                });

            var ordered = new List<Tuple<string, int>>().Select(t => new {Username = t.Item1, MostComments = t.Item2})
                .ToList();

            var xDoc = new XDocument();
            xDoc.Add(new XElement("users"));

            foreach (var u in users)
            {
                var mostComments = 0;
                if (u.PostsCommentCount.Any())
                {
                    mostComments = u.PostsCommentCount.OrderByDescending(c => c).First();
                }

                var obj = new
                {
                    u.Username,
                    MostComments = mostComments
                };

                ordered.Add(obj);
            }

            ordered = ordered.OrderByDescending(u => u.MostComments)
                .ThenBy(u => u.Username).ToList();

            foreach (var u in ordered)
            {
                xDoc.Root.Add(new XElement("user",
                    new XElement("Username", u.Username),
                    new XElement("MostComments", u.MostComments)
                ));
            }

            var result = xDoc.ToString();
            return result;
        }
    }
}