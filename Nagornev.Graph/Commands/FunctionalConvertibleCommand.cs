using System;
using System.Threading.Tasks;

namespace Nagornev.Graph.Commands
{
    public class FunctionalConvertibleCommand<TResponseType> : ConvertibleCommand<TResponseType>
        where TResponseType : struct, IConvertible
    {
        private Func<Task<TResponseType>> _function;

        public FunctionalConvertibleCommand(Func<Task<TResponseType>> function)
        {
            if (function is null)
                throw new ArgumentNullException("The function can not be null.");

            _function = function;
        }

        protected override async Task<TResponseType> Execute()
        {
            return await _function.Invoke();
        }

        public static FunctionalConvertibleCommand<TResponseType> Create(Func<Task<TResponseType>> function)
        {
            return new FunctionalConvertibleCommand<TResponseType>(function);
        }
    }
}
