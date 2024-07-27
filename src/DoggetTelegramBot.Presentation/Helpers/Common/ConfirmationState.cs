using System.Collections.Concurrent;

namespace DoggetTelegramBot.Presentation.Helpers.Common
{
    public static class ConfirmationState<T>
    {
        private static readonly ConcurrentDictionary<long, TaskCompletionSource<T?>> State = new();

        public static async Task<T?> WaitForResponseAsync(long userId, int timeoutInSeconds = 10)
        {
            TaskCompletionSource<T?> tcs = new(TaskCreationOptions.RunContinuationsAsynchronously);
            State[userId] = tcs;

            try
            {
                Task timeoutTask = Task.Delay(TimeSpan.FromSeconds(timeoutInSeconds));

                var completedTask = await Task.WhenAny(tcs.Task, timeoutTask).ConfigureAwait(false);

                return completedTask == tcs.Task ? tcs.Task.Result : default;
            }
            finally
            {
                State.TryRemove(userId, out _);
            }
        }

        public static void SetUserResponse(long userId, T? response)
        {
            if (State.TryGetValue(userId, out var tcs))
            {
                tcs.TrySetResult(response);
            }
        }
    }
}
