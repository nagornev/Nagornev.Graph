using System.Collections.Generic;
using Nagornev.Graph.Loggers;
using Nagornev.Graph.Nodes;

namespace Nagornev.Graph.Loggers
{
    public class Logger : ILogger
    {
        private IReadOnlyCollection<ILogger> _loggers;

        public Logger(params ILogger[] loggers)
        {
            _loggers = loggers;
        }

        public void Log(Node node, string comment)
        {
            foreach (ILogger logger in _loggers)
            {
                logger.Log(node, comment);
            }
        }
    }
}
