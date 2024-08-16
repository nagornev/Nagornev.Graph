using System;
using System.IO;
using Nagornev.Graph.Loggers;
using Nagornev.Graph.Nodes;

namespace Nagornev.Graph.Loggers
{
    public class FileLogger : ILogger
    {
        private string _fileName;

        public FileLogger(string fileName)
        {
            _fileName = fileName;
        }

        public void Log(Node node, string comment)
        {
            if (string.IsNullOrEmpty(node.Tag))
                return;

            string log = $"[{DateTime.Now}]: {node.Tag}. Comment:{comment}.";

            using (StreamWriter writer = new StreamWriter(_fileName, true))
            {
                writer.WriteLine(log);
            }
        }
    }
}
