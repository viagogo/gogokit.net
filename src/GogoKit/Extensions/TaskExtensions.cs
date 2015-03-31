using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using HalKit;

namespace GogoKit
{
    public static class TaskExtensions
    {
        public static ConfiguredTaskAwaitable<TResult> ConfigureAwait<TResult>(
            this Task<TResult> task,
            IHalClient client)
        {
            return task.ConfigureAwait(client.Configuration);
        }

        public static ConfiguredTaskAwaitable<TResult> ConfigureAwait<TResult>(
            this Task<TResult> task,
            IGogoKitConfiguration configuration)
        {
            return task.ConfigureAwait(configuration.CaptureSynchronizationContext);
        }

        public static ConfiguredTaskAwaitable ConfigureAwait(this Task task, IGogoKitConfiguration configuration)
        {
            return task.ConfigureAwait(configuration.CaptureSynchronizationContext);
        }
    }
}
