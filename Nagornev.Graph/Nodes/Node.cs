using System;
using System.Threading.Tasks;

namespace Nagornev.Graph.Nodes
{
    public abstract class Node
    {
        public class UnhandledExceptionEventArgs : EventArgs
        {
            public Exception Exception { get; private set; }

            public Node Node { get; private set; }

            internal UnhandledExceptionEventArgs(Exception exception,
                                                 Node node)
            {
                Exception = exception;
                Node = node;
            }
        }

        public delegate void OnCommandStartedDelegate(Node sender, EventArgs args);

        public delegate void OnCommandCompletedDelegate(Node sender, EventArgs args);

        public delegate void OnCommandCatchedUnhandledExceptionDelegate(Node sender, UnhandledExceptionEventArgs args);

        public abstract event OnCommandStartedDelegate OnCommandStartedEvent;

        public abstract event OnCommandCompletedDelegate OnCommandCompletedEvent;

        public abstract event OnCommandCatchedUnhandledExceptionDelegate OnCommandCatchedUnhandledExceptionEvent;

        public Node()
        {
            
        }

        /// <summary>
        /// This tag used in the logging node.
        /// </summary>
        public string Tag { get; private set; }

        /// <summary>
        /// Sets the node tag.
        /// </summary>
        /// <param name="tag">The node tag.</param>
        /// <exception cref="ArgumentException">The tag is already installed.</exception>
        public void SetTag(string tag)
        {
            if (!string.IsNullOrEmpty(Tag))
                throw new ArgumentException("The tag is already installed.");

            Tag = tag;
        }

        /// <summary>
        /// The main node method.
        /// </summary>
        /// <returns></returns>
        protected internal abstract Task<Node> Handle();
    }
}
