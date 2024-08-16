using System;
using Nagornev.Graph.Commands;

namespace Nagornev.Graph.Nodes
{
    public class IntervalConditionNode : IntervalConvertibleNode<bool>
    {
        public IntervalConditionNode(Func<ConditionCommand> creator,
                                     int interval)
            : base(creator, interval)
        {
        }
    }
}
