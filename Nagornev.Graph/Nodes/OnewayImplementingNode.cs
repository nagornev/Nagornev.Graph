using System;
using Nagornev.Graph.Commands;

namespace Nagornev.Graph.Nodes
{
    public abstract class OnewayImplementingNode<TCommandType> : ImplementingNode<TCommandType>
        where TCommandType : Command
    {
        protected bool IsRunned { get; private set; }

        protected OnewayImplementingNode(Func<TCommandType> creator)
            : base(creator)
        {
            OnCommandCompletedEvent += (s, o) => SetFlag();
        }

        private void SetFlag()
        {
            IsRunned = true;
        }
    }
}
