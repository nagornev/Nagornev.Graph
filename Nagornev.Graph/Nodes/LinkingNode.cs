using System;
using System.Threading.Tasks;
using Nagornev.Graph.Commands;

namespace Nagornev.Graph.Nodes
{
    public class LinkingNode : Node
    {
        public override event OnCommandStartedDelegate OnCommandStartedEvent;

        public override event OnCommandCompletedDelegate OnCommandCompletedEvent;

        public override event OnCommandCatchedUnhandledExceptionDelegate OnCommandCatchedUnhandledExceptionEvent;

        private Node _successor;

        private int _delay;

        /// <summary>
        /// Sets the successor for the node.
        /// </summary>
        /// <param name="successor"></param>
        /// <exception cref="ArgumentException"></exception>
        public void SetSuccessor(Node successor)
        {
            _successor = _successor is null ? successor : throw new ArgumentException("The successor is already installed.");
        }


        public void SetDelay(int delay)
        {
            _delay = delay;
        }

        protected internal override async Task<Node> Handle()
        {
            OnCommandStartedEvent?.Invoke(this, new EventArgs());
            OnCommandCompletedEvent?.Invoke(this, new EventArgs());

            await Task.Delay(_delay);

            return _successor;
        }
    }
}
