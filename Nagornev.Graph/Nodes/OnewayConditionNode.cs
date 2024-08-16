using System;
using Nagornev.Graph.Commands;

namespace Nagornev.Graph.Nodes
{
    public class OnewayConditionNode : OnewayConvertibleNode<bool>
    {
        public OnewayConditionNode(Func<ConditionCommand> creator)
            : base(creator)
        {
        }
    }
}
