namespace Instagraph.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using Data;
    using Models;
    using Newtonsoft.Json;

    public class Deserializer
    {
        public static string ImportPictures(InstagraphContext context, string jsonString)
        {
            var pictures = JsonConvert.DeserializeObject<Picture[]>(jsonString);

            var sb = new StringBuilder();

            var validPictures = new List<Picture>();
            foreach (var p in pictures)
            {
                if (string.IsNullOrEmpty(p.Path) || string.IsNullOrWhiteSpace(p.Path))
                {
                    sb.AppendLine(MessageHelper.InvalidDataError);
                    continue;
                }
                if (p.Size <= 0)
                {
                    sb.AppendLine(MessageHelper.InvalidDataError);
                    continue;
                }
                if (validPictures.Any(e => e.Path == p.Path))
                {
                    sb.AppendLine(MessageHelper.InvalidDataError);
                    continue;
                }
                validPictures.Add(p);
                sb.AppendLine(string.Format(MessageHelper.SuccessMessage.PictureSuccess, p.Path));
            }
            context.Pictures.AddRange(validPictures);

            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportUsers(InstagraphContext context, string jsonString)
        {
            dynamic users = JsonConvert.DeserializeObject(jsonString);

            var sb = new StringBuilder();

            var validUsers = new List<User>();

            foreach (var u in users)
            {
                string username = u["Username"];
                string password = u["Password"];
                string path = Convert.ToString(u["ProfilePicture"]);


                if (username == null || password == null || path == null)
                {
                    sb.AppendLine(MessageHelper.InvalidDataError);
                    continue;
                }
                if (username.Length > 30 || password.Length > 20)
                {
                    sb.AppendLine(MessageHelper.InvalidDataError);
                    continue;
                }

                if (!context.Pictures.Any(e => e.Path == path))
                {
                    sb.AppendLine(MessageHelper.InvalidDataError);
                    continue;
                }

                var picture = context.Pictures.SingleOrDefault(e => e.Path == path);

                var user = new User
                {
                    Username = username,
                    Password = password,
                    ProfilePicture = picture
                };

                validUsers.Add(user);
                sb.AppendLine(string.Format(MessageHelper.SuccessMessage.UserSuccess, user.Username));
            }
            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return sb.ToString();
        }

        public static string ImportFollowers(InstagraphContext context, string jsonString)
        {
            dynamic userFollowers = JsonConvert.DeserializeObject(jsonString);

            var followers = new List<UserFollower>();

            var sb = new StringBuilder();

            foreach (var u in userFollowers)
            {
                string username = u["User"];
                string followerName = u["Follower"];

                var userId = context.Users.FirstOrDefault(e => e.Username == username)?.Id;
                var followerId = context.Users.FirstOrDefault(e => e.Username == followerName)?.Id;

                if (userId == null || followerId == null)
                {
                    sb.AppendLine(MessageHelper.InvalidDataError);
                    continue;
                }
                var alreadyFollowed = followers.Any(f => f.UserId == userId && f.FollowerId == followerId);

                if (alreadyFollowed)
                {
                    sb.AppendLine(MessageHelper.InvalidDataError);
                    continue;
                }

                var userFollower = new UserFollower
                {
                    UserId = userId.Value,
                    FollowerId = followerId.Value
                };

                followers.Add(userFollower);
                sb.AppendLine(string.Format(MessageHelper.SuccessMessage.UserFollowerSuccess, followerName, username));
            }
            context.UsersFollowers.AddRange(followers);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportPosts(InstagraphContext context, string xmlString)
        {
            var posts = XDocument.Parse(xmlString);

            var root = posts.Elements("posts");

            var elements = root.Elements("post");

            var sb = new StringBuilder();

            foreach (var e in elements)
            {
                var userName = e.Element("user")?.Value;
                var caption = e.Element("caption")?.Value;
                var picturePath = e.Element("picture")?.Value;


                var user = context.Users.SingleOrDefault(u => u.Username == userName);

                var picture = context.Pictures.FirstOrDefault(p => p.Path == picturePath);

                if (string.IsNullOrEmpty(caption) || string.IsNullOrWhiteSpace(caption))
                {
                    sb.AppendLine(MessageHelper.InvalidDataError);
                    continue;
                }

                if (user == null || picture == null)
                {
                    sb.AppendLine(MessageHelper.InvalidDataError);
                    continue;
                }

                var post = new Post()
                {
                    Caption = caption,
                    User = user,
                    Picture = picture
                };

                context.Posts.Add(post);

                sb.AppendLine(string.Format(MessageHelper.SuccessMessage.PostSuccess, caption));
                Console.WriteLine();
            }

            context.SaveChanges();
            return sb.ToString();
        }

        public static string ImportComments(InstagraphContext context, string xmlString)
        {
            var stringComments = XDocument.Parse(xmlString);

            var root = stringComments.Elements("comments");

            var comments = root.Elements("comment");
            var sb = new StringBuilder();

            foreach (var c in comments)
            {
                var content = c.Element("content")?.Value;
                var userName = c.Element("user")?.Value;
                var postId = c.Element("post")?.Attribute("id")?.Value;


                if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content) ||
                    string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userName) ||
                    string.IsNullOrEmpty(postId) || string.IsNullOrWhiteSpace(postId))
                {
                    sb.AppendLine(MessageHelper.InvalidDataError);
                    continue;
                }
                if (!int.TryParse(postId, out var a))
                {
                    sb.AppendLine(MessageHelper.InvalidDataError);
                    continue;
                }

                var user = context.Users.SingleOrDefault(e => e.Username == userName);
                var post = context.Posts.Find(int.Parse(postId));

                if (user == null || post == null)
                {
                    sb.AppendLine(MessageHelper.InvalidDataError);
                    continue;
                }

                var comment = new Comment
                {
                    User = user,
                    Post = post,
                    Content = content
                };

                context.Comments.Add(comment);
                sb.AppendLine(string.Format(MessageHelper.SuccessMessage.CommentSuccess, content));
            }
            context.SaveChanges();
            return sb.ToString();
        }
    }
}