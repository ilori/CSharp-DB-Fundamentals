namespace Instagraph.DataProcessor
{
    internal static class MessageHelper
    {
        public const string InvalidDataError = "Error: Invalid data.";


        internal static class SuccessMessage
        {
            public const string PictureSuccess = "Successfully imported Picture {0}.";
            public const string UserSuccess = "Successfully imported User {0}.";
            public const string PostSuccess = "Successfully imported Post {0}.";
            public const string CommentSuccess = "Successfully imported Comment {0}.";
            public const string UserFollowerSuccess = "Successfully imported Follower {0} to User {1}.";
        }
    }
}