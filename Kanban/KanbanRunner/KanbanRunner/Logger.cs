using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanbanRunner
{
    /*
     * A helper class which logs messages to the console, with a timestamp and
     * name
     */
    class Logger
    {
        private readonly string name;

        public Logger(string name)
        {
            this.name = name;
        }

        /*
         * DECRIPTION:
         *      Logs a message
         * PARAMETERS:
         *      message - The message
         */
        public void Log(string message)
        {
            Console.WriteLine(GetPrefix() + message);
        }

        /*
         * DECRIPTION:
         *      Logs a message with an argument
         * PARAMETERS:
         *      message - The message
         *      arg - The argument (console-type formatting)
         */
        public void Log(string message, object arg)
        {
            Console.WriteLine(GetPrefix() + message, arg);
        }

        /*
         * DECRIPTION:
         *      Logs a message with multiple arguments
         * PARAMETERS:
         *      message - The message
         *      args - The params list of arguments
         */
        public void Log(string message, params object[] args)
        {
            Console.WriteLine(GetPrefix() + message, args);
        }

        /*
         * DESCRIPTION:
         *      Gets the prefix for a message
         * RETURNS:
         *      The prefix, with format "[H:MM AM/PM <name>]: "
         */
        private string GetPrefix()
        {
            return string.Format("[{0} {1}]: ", DateTime.Now.ToShortTimeString(), name);
        }
    }
}
