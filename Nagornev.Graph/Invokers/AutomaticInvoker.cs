using System;
using System.Threading.Tasks;
using Nagornev.Graph.Loggers;
using Nagornev.Graph.Nodes;

namespace Nagornev.Graph.Invokers
{
    public class AutomaticInvoker : Invoker
    {
        private enum State
        {
            Executing,
            Stopped,
            Paused,
        }

        public override event InvokerStartedDelegate OnInvokerStartedEvent;

        public override event InvokerCompletedDelegate OnInvokerCompletedEvent;

        public override event InvokerCatchedUnhandledExceptionDelegate OnUnhandledExceptionCatchedEvent;

        private Action<Node> OnHandlingStartedEvent;

        private Action<Node> OnHandlingCompletedEvent;

        private Node _next;

        private Node _start;

        private Node _preview;

        private State _state = State.Stopped;

        public AutomaticInvoker()
            : this(null)
        {
        }

        public AutomaticInvoker(ILogger logger)
            : base(logger)
        {
            OnHandlingStartedEvent += (node) => Logger?.Log(node, HandlingComment);
            OnHandlingCompletedEvent += (node) => Logger?.Log(node, ExecutedComment);
            OnUnhandledExceptionCatchedEvent += (s, e) => Logger?.Log(e.Node, string.Format(ExceptionComment, e.Exception.GetType().FullName, e.Exception.Message));
        }

        public override void SetStart(Node node)
        {
            _start = node;
        }

        public async Task Start()
        {
            if (_state != State.Executing)
            {
                _state = State.Executing;
                _next = _start;
                await Execute();
            }
        }

        public async Task Resume()
        {
            _state = State.Executing;
            await Execute();
        }

        public void Stop()
        {
            _state = State.Stopped;
            _next = _start;
        }

        public void Pause()
        {
            _state = State.Paused;
        }

        private async Task Execute()
        {
            OnInvokerStartedEvent?.Invoke(this, new EventArgs());

            while (_state is State.Executing)
            {
                OnHandlingStartedEvent?.Invoke(_next);

                try
                {
                    _preview = _next;
                    _next = await _next.Handle();
                }
                catch (Exception exception)
                {
                    Stop();
                    OnUnhandledExceptionCatchedEvent?.Invoke(this, new UnhandledExceptionEventArgs(exception, _preview));
                    return;
                }

                OnHandlingCompletedEvent?.Invoke(_preview);

                //Stop handling
                if (_next is null)
                {
                    Stop();
                    OnInvokerCompletedEvent?.Invoke(this, new EventArgs());
                }
            }
        }
    }
}
