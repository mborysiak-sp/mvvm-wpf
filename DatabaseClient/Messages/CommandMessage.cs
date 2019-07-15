using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Refresh
    }
}
