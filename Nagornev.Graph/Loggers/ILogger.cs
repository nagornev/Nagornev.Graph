using Nagornev.Graph.Nodes;

namespace Nagornev.Graph.Loggers
{
    public interface ILogger
    {
        void Log(Node node, string comment);
    }
}
