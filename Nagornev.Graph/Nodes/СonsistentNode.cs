using Nagornev.Graph.Helpers;
using System;
using System.Threading.Tasks;
using Nagornev.Graph.Commands;

namespace Nagornev.Graph.Nodes
{
    public class ConsistentNode : ImplementingNode<ConsistentCommand>
    {
        public override event OnCommandStartedDelegate OnCommandStartedEvent;

        public override event OnCommandCompletedDelegate OnCommandCompletedEvent;

        private Node _successor;

        public ConsistentNode(Func<ConsistentCommand> creator)
            : base(creator)
        {
        }

        /// <summary>
        /// Sets the successor for the node.
        /// </summary>
        /// <param name="successor"></param>
        /// <exception cref="ArgumentException"></exception>
        public void SetSuccessor(Node successor)
        {
            _successor = _successor is null ? successor : throw new ArgumentException("The successor is already installed.");
        }

        protected async override Task<Node> Execute(ConsistentCommand command)
        {
            try
            {
                await CommandHelper.Invoke(command.Handle,
                                           () => OnCommandStartedEvent?.Invoke(this, EventArgs.Empty),
                                           () => OnCommandCompletedEvent?.Invoke(this, EventArgs.Empty),
                                           Delay);

                return _successor;
            }
            catch (Exception exception)
            {
                return HandleException(exception);
            }
        }
    }
}
