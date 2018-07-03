namespace Instagraph.App
{
    using System;
    using System.IO;
    using Data;
    using DataProcessor;
    using System.Text;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(ResetDatabase());

            Console.WriteLine(ImportData());

            ExportData();
        }

        private static string ImportData()
        {
            var sb = new StringBuilder();

            using (var context = new InstagraphContext())
            {
                var picturesJson = File.ReadAllText("files/input/pictures.json");

                sb.AppendLine(Deserializer.ImportPictures(context, picturesJson));

                var usersJson = File.ReadAllText("files/input/users.json");

                sb.AppendLine(Deserializer.ImportUsers(context, usersJson));

                var followersJson = File.ReadAllText("files/input/users_followers.json");

                sb.AppendLine(Deserializer.ImportFollowers(context, followersJson));

                var postsXml = File.ReadAllText("files/input/posts.xml");

                sb.AppendLine(Deserializer.ImportPosts(context, postsXml));

                var commentsXml = File.ReadAllText("files/input/comments.xml");

                sb.AppendLine(Deserializer.ImportComments(context, commentsXml));
            }

            var result = sb.ToString().Trim();
            return result;
        }

        private static void ExportData()
        {
            using (var context = new InstagraphContext())
            {
                var uncommentedPostsOutput = Serializer.ExportUncommentedPosts(context);

                File.WriteAllText("files/output/UncommentedPosts.json", uncommentedPostsOutput);

                var usersOutput = Serializer.ExportPopularUsers(context);

                File.WriteAllText("files/output/PopularUsers.json", usersOutput);

                var commentsOutput = Serializer.ExportCommentsOnPosts(context);

                File.WriteAllText("files/output/CommentsOnPosts.xml", commentsOutput);
            }
        }

        private static string ResetDatabase()
        {
            using (var context = new InstagraphContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }

            return $"Database reset succsessfully.";
        }
    }
}