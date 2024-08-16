using System;
using System.Threading.Tasks;
using Nagornev.Graph.Commands;

namespace Nagornev.Graph.Helpers
{
    internal class CommandHelper
    {
        internal static async Task Invoke(Func<Task> command, Action start, Action end, int delay)
        {
            start.Invoke();

            await command.Invoke();

            end.Invoke();

            await Task.Delay(delay);
        }

        internal static async Task<T> Invoke<T>(Func<Task<T>> command, Action start, Action end, int delay)
            where T : struct, IConvertible
        {
            start.Invoke();

            T response = await command.Invoke();

            end.Invoke();

            await Task.Delay(delay);

            return (T)(object)response;
        }
    }
}
