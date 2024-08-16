using System;
using System.Threading.Tasks;

namespace Nagornev.Graph.Commands
{
    public class FunctionalConsistentCommand : ConsistentCommand
    {
        private Func<Task> _function;

        public FunctionalConsistentCommand(Func<Task> function)
        {
            if (function is null)
                throw new ArgumentNullException("The function can not be null.");

            _function = function;
        }

        protected override async Task Execute()
        {
            await _function.Invoke();
        }

        public static FunctionalConsistentCommand Create(Func<Task> function)
        {
            return new FunctionalConsistentCommand(function);
        }
    }
}
