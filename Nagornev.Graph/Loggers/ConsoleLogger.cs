using System;
using Nagornev.Graph.Nodes;

namespace Nagornev.Graph.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public void Log(Node node, string comment)
        {
            if (string.IsNullOrEmpty(node.Tag))
                return;

            string log = $"[{DateTime.Now}]: {node.Tag}. Comment: {comment}.";

            Console.WriteLine(log);
        }
    }
}
