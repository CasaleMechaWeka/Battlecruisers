using UnityEngine;
using System;
using System.Threading.Tasks;

namespace BattleCruisers.Utils
{
    public static class AsyncHelper
    {
        public static void FireAndForget(Func<Task> task)
        {
            Task.Run(async () =>
            {
                try
                {
                    await task();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Fire and forget task failed: {ex}");
                }
            });
        }
    }
} 