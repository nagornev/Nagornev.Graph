using System;
using Nagornev.Graph.Commands;

namespace Nagornev.Graph.Commands
{
    public abstract class CallbackExceptionCommand<TCallbackType> : ExceptionCommand
    {
        private CommandCallbackDelegate<TCallbackType> _callbacks;

        public CallbackExceptionCommand()
            : this(Array.Empty<CommandCallbackDelegate<TCallbackType>>())
        {
        }

        public CallbackExceptionCommand(params CommandCallbackDelegate<TCallbackType>[] callbacks)
        {
            OnCommandCompleteEvent += (s, e) => _callbacks?.Invoke(Call());

            foreach (CommandCallbackDelegate<TCallbackType> callback in callbacks)
            {
                AddCallback(callback);
            }
        }

        public void AddCallback(CommandCallbackDelegate<TCallbackType> callback)
        {
            _callbacks += callback;
        }

        public void RemoveCallback(CommandCallbackDelegate<TCallbackType> callback)
        {
            _callbacks -= callback;
        }

        protected abstract TCallbackType Call();
    }
}
