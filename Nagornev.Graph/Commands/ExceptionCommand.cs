using System;
using System.Threading.Tasks;

namespace Nagornev.Graph.Commands
{
    public abstract class ExceptionCommand : Command
    {
        protected delegate void OnCommandBeginDelegate(ExceptionCommand sender, EventArgs args);

        protected delegate void OnCommandCompleteDelegate(ExceptionCommand sender, EventArgs args);

        protected event OnCommandBeginDelegate OnCommandBeginEvent;

        protected event OnCommandCompleteDelegate OnCommandCompleteEvent;

        protected Exception CatchedException { get; private set; }

        public ExceptionCommand()
        {
            OnCommandBeginEvent += (s, e) => Begin();
            OnCommandCompleteEvent += (s, e) => Complete();
        }

        internal void SetCatchedException(Exception exception)
        {
            CatchedException = exception;
        }

        internal async Task Handle()
        {
            OnCommandBeginEvent?.Invoke(this, EventArgs.Empty);

            await Execute();

            OnCommandCompleteEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// The main command method.
        /// </summary>
        /// <returns></returns>
        protected abstract Task Execute();

        /// <summary>
        /// The methods are executed before the main method.
        /// </summary>
        protected virtual void Begin() { }

        /// <summary>
        /// The methods are executed after the main method.
        /// </summary>
        protected virtual void Complete() { }
    }
}
