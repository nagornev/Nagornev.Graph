using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nagornev.Graph.Commands;

namespace Nagornev.Graph.Nodes
{
    public abstract class ImplementingNode<TCommandType> : Node
        where TCommandType : Command
    {
        private bool _single;

        private Func<TCommandType> _creator;

        private TCommandType _command;

        private Dictionary<Type, ExceptionNode> _exceptionSuccessors;

        public ImplementingNode(Func<TCommandType> creator)
        {
            if (creator is null)
                throw new ArgumentNullException("The creator callback can not been null.");

            _creator = creator;
            _exceptionSuccessors = new Dictionary<Type, ExceptionNode>();
        }

        public override event OnCommandCatchedUnhandledExceptionDelegate OnCommandCatchedUnhandledExceptionEvent;

        protected int Delay { get; private set; }

        /// <summary>
        /// Sets parameter for creating a one command.
        /// </summary>
        /// <param name="single"></param>
        public void SetSingle(bool single)
        {
            _single = single;
        }

        /// <summary>
        /// Sets the delay after excuting the command.
        /// </summary>
        /// <param name="delay"></param>
        public void SetDelay(int delay)
        {
            Delay = delay;
        }

        /// <summary>
        /// Sets the successor that will be executed if an unhandled exception is found.
        /// </summary>
        /// <param name="exceptionSuccessor"></param>
        public void SetExceptionSuccessor<T>(ExceptionNode exceptionSuccessor)
            where T : Exception
        {
            SetParentNode(exceptionSuccessor);
            SubscribeOnClearRepetion(exceptionSuccessor);

            _exceptionSuccessors.Add(typeof(T), exceptionSuccessor);
        }

        private void SetParentNode(ExceptionNode exceptionSuccessor)
        {
            exceptionSuccessor.SetParentNode(this);
        }

        private void SubscribeOnClearRepetion(ExceptionNode exceptionSuccessor)
        {
            OnCommandCompletedEvent += (s, o) => exceptionSuccessor.ResetRepetion();
        }

        protected internal override async Task<Node> Handle()
        {
            _command = !_single ? _creator.Invoke() :
                                  _command == null ? _creator.Invoke() : _command;

            return await Execute(_command);
        }

        protected virtual Node HandleException(Exception exception)
        {
            foreach (Type key in _exceptionSuccessors.Keys)
            {
                Type exceptionType = exception.GetType();

                if (key == exceptionType ||
                    exceptionType.IsSubclassOf(key))
                {
                    ExceptionNode exceptionNode = _exceptionSuccessors[key];
                    exceptionNode.SetCatchedException(exception);

                    return exceptionNode;
                }
            }

            OnCommandCatchedUnhandledExceptionEvent?.Invoke(this, new UnhandledExceptionEventArgs(exception, this));

            throw exception;
        }

        protected abstract Task<Node> Execute(TCommandType command);
    }
}
