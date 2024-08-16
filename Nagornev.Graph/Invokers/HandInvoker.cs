using System;
using System.Threading.Tasks;
using Nagornev.Graph.Loggers;
using Nagornev.Graph.Nodes;

namespace Nagornev.Graph.Invokers
{
    public class HandInvoker : Invoker
    {
        public override event InvokerStartedDelegate OnInvokerStartedEvent;

        public override event InvokerCompletedDelegate OnInvokerCompletedEvent;

        public override event InvokerCatchedUnhandledExceptionDelegate OnUnhandledExceptionCatchedEvent;

        private Action<Node> OnHandlingStartedEvent;

        private Action<Node> OnHandlingCompletedEvent;

        private Node _start;

        private Node _next;

        private Node _preview;

        public HandInvoker()
            :this(null)
        {
        }

        public HandInvoker(ILogger logger)
            : base(logger)
        {
            OnHandlingStartedEvent += (node) => Logger?.Log(_next, HandlingComment);
            OnHandlingCompletedEvent += (node) => Logger?.Log(_next, ExecutedComment);
            OnUnhandledExceptionCatchedEvent += (s, e) => Logger?.Log(_next, string.Format(ExceptionComment, e.Exception.GetType().FullName, e.Exception.Message));
        }

        public override void SetStart(Node node)
        {
            _start = node;
        }

        public void Restart()
        {
            _next = _start;
        }

        public async Task Execute()
        {
            OnInvokerStartedEvent?.Invoke(this, new EventArgs());

            OnHandlingStartedEvent?.Invoke(_next);

            try
            {
                _preview = _next;
                _next = await _next.Handle();
            }
            catch (Exception exception)
            {
                OnUnhandledExceptionCatchedEvent?.Invoke(this, new UnhandledExceptionEventArgs(exception, _preview));
                return;
            }

            OnHandlingCompletedEvent?.Invoke(_preview);

            //Stop handling
            if (_next is null)
                OnInvokerCompletedEvent?.Invoke(this, new EventArgs());
        }
    }
}
