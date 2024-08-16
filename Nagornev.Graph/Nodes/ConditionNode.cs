using System;
using Nagornev.Graph.Commands;

namespace Nagornev.Graph.Nodes
{
    public class ConditionNode : ConvertibleNode<bool>
    {
        public ConditionNode(Func<ConditionCommand> creator)
            : base(creator)
        {
        }
    }
}
