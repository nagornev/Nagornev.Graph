using System;
using System.Threading.Tasks;

namespace Nagornev.Graph.Commands
{
    public class FunctionalConditionCommand : ConditionCommand
    {
        private Func<Task<bool>> _function;

        public FunctionalConditionCommand(Func<Task<bool>> function)
        {
            if (function is null)
                throw new ArgumentNullException("The function can not be null.");

            _function = function;
        }

        protected override async Task<bool> Execute()
        {
            return await _function.Invoke();
        }

        public static FunctionalConditionCommand Create(Func<Task<bool>> function)
        {
            return new FunctionalConditionCommand(function);
        }
    }
}
