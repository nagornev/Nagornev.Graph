using System;
using System.Threading.Tasks;
using Nagornev.Graph.Commands;

namespace Nagornev.Graph.Nodes
{
    public class ExceptionNode : ImplementingNode<ExceptionCommand>
    {
        public override event OnCommandStartedDelegate OnCommandStartedEvent;

        public override event OnCommandCompletedDelegate OnCommandCompletedEvent;

        private Node _parentCommandNode;

        private Node _successor;

        private Exception _catchedException;

        private int _repeat;

        private int _repetion;

        public ExceptionNode(Func<ExceptionCommand> creator)
            : base(creator)
        {
            OnCommandCompletedEvent += (s, o) => _repetion++;
        }

        /// <summary>
        /// Sets the successor for the node.
        /// </summary>
        /// <param name="successor"></param>
        /// <exception cref="ArgumentException"></exception>
        public virtual void SetSuccessor(Node successor)
        {
            _successor = _successor is null ? successor : throw new ArgumentException("The successor is already installed.");
        }

        protected override async Task<Node> Execute(ExceptionCommand command)
        {
            try
            {
                OnCommandStartedEvent?.Invoke(this, new EventArgs());

                command?.SetCatchedException(_catchedException);

                await command?.Handle();

                Node successor = _repetion >= _repeat ? _successor : _parentCommandNode;

                OnCommandCompletedEvent?.Invoke(this, new EventArgs());

                await Task.Delay(Delay);

                return successor;
            }
            catch (Exception exception)
            {
                return HandleException(exception);
            }
        }

        /// <summary>
        /// Sets the repetition <paramref name="count"/> for the parent node.
        /// </summary>
        /// <param name="count"></param>
        public void SetRepeat(int count)
        {
            _repeat = count;
        }

        /// <summary>
        /// Sets the parent node to check for catching the unhandled exception.
        /// </summary>
        /// <param name="commandNode"></param>
        internal void SetParentNode(Node commandNode)
        {
            _parentCommandNode = commandNode;
        }

        /// <summary>
        /// Sets the catched unhandled exception.
        /// </summary>
        /// <param name="catchedException"></param>
        internal void SetCatchedException(Exception catchedException)
        {
            _catchedException = catchedException;
        }

        /// <summary>
        /// Clear current count repeats. This method has been executed when parent node handle the command without an unhandled exception.
        /// </summary>
        internal void ResetRepetion()
        {
            _repetion = 0;
        }
    }
}
