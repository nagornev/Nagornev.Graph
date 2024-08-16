using System;

namespace Nagornev.Graph.Commands
{
    public abstract class CallbackConvertibleCommand<TConvertibleType, TCallbackType> : ConvertibleCommand<TConvertibleType>
        where TConvertibleType : struct, IConvertible
    {
        private CommandCallbackDelegate<TCallbackType> _callbacks;

        public CallbackConvertibleCommand()
           : this(Array.Empty<CommandCallbackDelegate<TCallbackType>>())
        {
        }

        public CallbackConvertibleCommand(params CommandCallbackDelegate<TCallbackType>[] callbacks)
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
