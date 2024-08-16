using System;
using Nagornev.Graph.Loggers;
using Nagornev.Graph.Nodes;

namespace Nagornev.Graph.Invokers
{
    public abstract class Invoker
    {
        protected const string ExecutedComment = "executed";

        protected const string HandlingComment = "handling";

        protected const string ExceptionComment = "catched undhandled exception. {0}: {1}.";

        public class UnhandledExceptionEventArgs : EventArgs
        {
            public Exception Exception { get; private set; }

            public Node Node { get; private set; }

            internal UnhandledExceptionEventArgs(Exception exception,
                                                 Node node)
            {
                Exception = exception;
                Node = node;
            }
        }

        public delegate void InvokerCatchedUnhandledExceptionDelegate(Invoker sender, UnhandledExceptionEventArgs args);

        public delegate void InvokerStartedDelegate(Invoker sender, EventArgs args);

        public delegate void InvokerCompletedDelegate(Invoker sender, EventArgs args);

        public abstract event InvokerStartedDelegate OnInvokerStartedEvent;

        public abstract event InvokerCompletedDelegate OnInvokerCompletedEvent;

        public abstract event InvokerCatchedUnhandledExceptionDelegate OnUnhandledExceptionCatchedEvent;

        protected ILogger Logger { get; private set; }

        public Invoker()
            : this(null)
        {
        }

        public Invoker(ILogger logger)
        {
            SetLogger(logger);
        }

        public void SetLogger(ILogger logger)
        {
            Logger = logger;
        }

        public abstract void SetStart(Node node);
    }
}
