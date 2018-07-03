namespace PhotoShare.Client.Core
{
    using System;
    using System.Linq;
    using Commands;

    public class CommandDispatcher
    {
        public string DispatchCommand(string[] commandParameters)
        {
            var command = commandParameters[0];

            var commandExecute = commandParameters.Skip(1).ToArray();

            var result = string.Empty;


            switch (command)
            {
                case "RegisterUser":
                    result = RegisterUserCommand.Execute(commandExecute);
                    break;
                case "AddTown":
                    result = AddTownCommand.Execute(commandExecute);
                    break;
                case "ModifyUser":
                    result = ModifyUserCommand.Execute(commandExecute);
                    break;
                case "DeleteUser":
                    result = DeleteUserCommand.Execute(commandExecute);
                    break;
                case "AddTag":
                    result = AddTagCommand.Execute(commandExecute);
                    break;
                case "CreateAlbum":
                    result = CreateAlbumCommand.Execute(commandExecute);
                    break;
                case "AddTagTo":
                    result = AddTagToCommand.Execute(commandExecute);
                    break;
                case "AddFriend":
                    result = AddFriendCommand.Execute(commandExecute);
                    break;
                case "AcceptFriend":
                    result = AcceptFriendCommand.Execute(commandExecute);
                    break;
                case "ListFriends":
                    result = PrintFriendsListCommand.Execute(commandExecute);
                    break;
                case "ShareAlbum":
                    result = ShareAlbumCommand.Execute(commandExecute);
                    break;
                case "UploadPicture":
                    result = UploadPictureCommand.Execute(commandExecute);
                    break;
                case "Login":
                    result = LoginCommand.Execute(commandExecute);
                    break;
                case "Logout":
                    result = LogoutCommand.Execute(commandExecute);
                    break;
                case "Exit":
                    result = ExitCommand.Execute();
                    break;

                default:
                    throw new InvalidOperationException($"Command {command} not valid!");
            }
            return result;
        }
    }
}