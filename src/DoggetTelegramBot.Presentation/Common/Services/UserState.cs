using System.Collections.Concurrent;

namespace DoggetTelegramBot.Presentation.Common.Services
{
    public static class UserState
    {
        private static readonly ConcurrentDictionary<long, TaskCompletionSource<bool?>> State = new();

        public static async Task<bool?> WaitForResponseAsync(long userId, int timeoutInSeconds = 10)
        {
            TaskCompletionSource<bool?> tcs = new();
            State[userId] = tcs;

            Task timeoutTask = Task.Delay(TimeSpan.FromSeconds(timeoutInSeconds));

            var completedTask = await Task.WhenAny(tcs.Task, timeoutTask);

            State.TryRemove(userId, out _);

            return completedTask == tcs.Task ? tcs.Task.Result : null;
        }

        public static void SetUserResponse(long userId, bool? response)
        {
            if (State.TryGetValue(userId, out var tcs))
            {
                tcs.TrySetResult(response);
            }
        }
    }
}
