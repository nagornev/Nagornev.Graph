using System;
using Nagornev.Graph.Commands;

namespace Nagornev.Graph.Nodes
{
    public class DelayedConditionNode : DelayedConveribleNode<bool>
    {
        public DelayedConditionNode(Func<ConditionCommand> creator,
                                    int delayed)
            : base(creator, delayed)
        {
        }
    }
}
