using Nagornev.Graph.Helpers;
using System;
using System.Threading.Tasks;
using Nagornev.Graph.Commands;

namespace Nagornev.Graph.Nodes
{
    public class DelayedConsistentNode : DelayedImplementingNode<ConsistentCommand>
    {
        public override event OnCommandStartedDelegate OnCommandStartedEvent;

        public override event OnCommandCompletedDelegate OnCommandCompletedEvent;

        private Node _successor;

        public DelayedConsistentNode(Func<ConsistentCommand> creator,
                                            int delayed)
            : base(creator, delayed)
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

        protected override async Task<Node> Execute(ConsistentCommand command)
        {
            if (IsSkip())
            {
                return _successor;
            }

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
