using System;
using Nagornev.Graph.Commands;

namespace Nagornev.Graph.Nodes
{
    public abstract class IntervalImplementingNode<TCommandType> : ImplementingNode<TCommandType>
        where TCommandType : Command
    {
        public delegate void ExecuteStartedDelegate(Node commandNode, EventArgs args);

        public abstract event ExecuteStartedDelegate OnExecuteStartedEvent;

        private int _interval;

        private int _position;

        protected IntervalImplementingNode(Func<TCommandType> creator,
                                           int interval)
            : base(creator)
        {
            _position = -1;
            _interval = interval > 0 ? interval : throw new ArgumentException("The interval can not be less than one.");
            OnExecuteStartedEvent += (s, o) => IncrementPosition();
        }

        internal void IncrementPosition()
        {
            _position++;
        }

        internal void ResetPosition()
        {
            _position = -1;
        }

        internal bool IsSkip()
        {
            if (_position == 0)
            {
                return false;
            }
            else if (_position > 0 && _position < _interval)
            {
                return true;
            }
            else
            {
                ResetPosition();
                return true;
            }
        }
    }
}
