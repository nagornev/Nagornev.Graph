using System;
using System.Threading.Tasks;

namespace Nagornev.Graph.Commands
{
    public abstract class ConvertibleCommand<TConvertibleType> : Command
    {
        protected class OnCommandCompletedEventArgs
        {
            public OnCommandCompletedEventArgs(TConvertibleType response)
            {
                Response = response;
            }

            public TConvertibleType Response { get; private set; }
        }

        protected delegate void OnCommandBeginDelegate(ConvertibleCommand<TConvertibleType> sender, EventArgs args);

        protected delegate void OnCommandCompleteDelegate(ConvertibleCommand<TConvertibleType> sender, OnCommandCompletedEventArgs args);

        protected event OnCommandBeginDelegate OnCommandBeginEvent;

        protected event OnCommandCompleteDelegate OnCommandCompleteEvent;

        public ConvertibleCommand()
        {
            OnCommandBeginEvent += (s, e) => Begin();
            OnCommandCompleteEvent += (s, e) => Complete(e.Response);
        }

        internal async Task<TConvertibleType> Handle()
        {
            OnCommandBeginEvent?.Invoke(this, EventArgs.Empty);

            TConvertibleType response = await Execute();

            OnCommandCompleteEvent?.Invoke(this, new OnCommandCompletedEventArgs(response));

            return response;
        }

        /// <summary>
        /// The main command method.
        /// </summary>
        /// <returns></returns>
        protected abstract Task<TConvertibleType> Execute();

        /// <summary>
        /// The methods are executed before the main method.
        /// </summary>
        protected virtual void Begin() { }

        /// <summary>
        /// The methods are executed after the main method.
        /// </summary>
        /// <param name="commandResponse"></param>
        protected virtual void Complete(TConvertibleType commandResponse) { }
    }
}
