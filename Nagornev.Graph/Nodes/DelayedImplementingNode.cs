using System;
using Nagornev.Graph.Commands;

namespace Nagornev.Graph.Nodes
{
    public abstract class DelayedImplementingNode<TCommandType> : ImplementingNode<TCommandType>
        where TCommandType : Command
    {
        private DateTime _outdate;

        private int _delayed;

        protected DelayedImplementingNode(Func<TCommandType> creator,
                                          int delayed)
            : base(creator)
        {
            _outdate = DateTime.Now;
            _delayed = delayed > 0 ? delayed : throw new ArgumentException("Delayed can not be less than one.");
            OnCommandCompletedEvent += (s, o) => UpdateOutdate();
        }

        internal void UpdateOutdate()
        {
            _outdate = DateTime.Now.AddMilliseconds(_delayed);
        }

        internal bool IsSkip()
        {
            return _outdate > DateTime.Now;
        }
    }
}
