using System;

namespace Nagornev.Graph.Commands
{
    public abstract class CallbackConditionCommand<TCallbackType> : ConditionCommand
    {
        private CommandCallbackDelegate<TCallbackType> _callbacks;

        public CallbackConditionCommand()
           : this(Array.Empty<CommandCallbackDelegate<TCallbackType>>())
        {
        }

        public CallbackConditionCommand(params CommandCallbackDelegate<TCallbackType>[] callbacks)
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
