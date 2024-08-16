using Nagornev.Graph.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nagornev.Graph.Commands;

namespace Nagornev.Graph.Nodes
{
    public class OnewayConvertibleNode<TConvertibleType> : OnewayImplementingNode<ConvertibleCommand<TConvertibleType>>
        where TConvertibleType : struct, IConvertible
    {

        public override event OnCommandStartedDelegate OnCommandStartedEvent;

        public override event OnCommandCompletedDelegate OnCommandCompletedEvent;

        private Dictionary<TConvertibleType, Node> _successors;

        private Node _successor;

        public OnewayConvertibleNode(Func<ConvertibleCommand<TConvertibleType>> creator)
            : base(creator)
        {
            _successors = new Dictionary<TConvertibleType, Node>();
        }

        /// <summary>
        /// Sets the successors for the node.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="successor"></param>
        /// <exception cref="ArgumentException"></exception>
        public virtual void SetSuccessor(TConvertibleType key, Node successor)
        {
            if (_successors.ContainsKey(key))
                throw new ArgumentException("The successor with that key is already intalled.");

            _successors.Add(key, successor);
        }

        protected async override Task<Node> Execute(ConvertibleCommand<TConvertibleType> command)
        {
            if (IsRunned)
            {
                return _successor;
            }

            try
            {
                TConvertibleType response = await CommandHelper.Invoke(command.Handle,
                                                                       () => OnCommandStartedEvent?.Invoke(this, EventArgs.Empty),
                                                                       () => OnCommandCompletedEvent?.Invoke(this, EventArgs.Empty),
                                                                       Delay);

                _successor = _successors[response];

                return _successor;
            }
            catch (KeyNotFoundException exception)
            {
                throw exception;
            }
            catch (Exception exception)
            {
                return HandleException(exception);
            }
        }
    }
}
