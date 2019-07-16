namespace DatabaseClient.Messages
{
    public class CommandMessage
    {
        public CommandType Command { get; set; }
    }

    public enum CommandType
    {
        Insert,
        Edit,
        Delete,
        Commit,
        Refresh,
        Quit,
        None
    }
}
