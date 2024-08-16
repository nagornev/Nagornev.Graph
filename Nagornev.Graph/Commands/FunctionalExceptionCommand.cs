using System;
using System.Threading.Tasks;
using Nagornev.Graph.Commands;

namespace Nagornev.Graph.Commands
{
    public class FunctionalExceptionCommand : ExceptionCommand
    {
        private Func<Exception, Task> _function;

        public FunctionalExceptionCommand(Func<Exception, Task> function)
        {
            if (function is null)
                throw new ArgumentNullException("The function can not be null.");

            _function = function;
        }

        protected override async Task Execute()
        {
            await _function.Invoke(CatchedException);
        }

        public static FunctionalExceptionCommand Create(Func<Exception, Task> function)
        {
            return new FunctionalExceptionCommand(function);
        }
    }
}
