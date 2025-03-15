using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace BattleCruisers.Utils
{
    // Helper class for handling asynchronous operations in Unity.
    // Provides utilities to safely execute async tasks while respecting Unity's threading rules.
    public static class AsyncHelper
    {
        // Captures Unity's main thread synchronization context when this class is first accessed.
        // This is crucial because Unity requires certain operations (like GetComponent) to run on the main thread.
        private static readonly SynchronizationContext UnityContext = SynchronizationContext.Current;

        // Executes an asynchronous task without waiting for its completion, while ensuring Unity operations run on the main thread.
        // This is useful for fire-and-forget operations that need to interact with Unity's API.
        //
        // Important threading notes:
        // 1. This helper must be initialized from Unity's main thread (e.g., in Start/Awake)
        // 2. Unity API calls (like GetComponent) will be automatically marshaled to the main thread
        // 3. Exceptions in the task will be caught and logged, preventing crashes
        //
        // Usage example:
        // AsyncHelper.FireAndForget(async () => {
        //     await someAsyncOperation();
        //     gameObject.GetComponent<T>(); // Safely runs on main thread
        // });
        public static void FireAndForget(Func<Task> task)
        {
            // Verify we have captured Unity's main thread context
            if (UnityContext == null)
            {
                Debug.LogError("AsyncHelper must be initialized on the main thread. Call this from a MonoBehaviour's Start/Awake.");
                return;
            }

            // Start a new task on the thread pool
            Task.Run(async () =>
            {
                try
                {
                    // Create a TaskCompletionSource to properly handle async/await across thread boundaries
                    var tcs = new TaskCompletionSource<bool>();

                    // Post the work back to Unity's main thread
                    // This ensures all Unity API calls run on the correct thread
                    UnityContext.Post(async _ =>
                    {
                        try
                        {
                            // Execute the actual task
                            await task();
                            // Signal successful completion
                            tcs.SetResult(true);
                        }
                        catch (Exception ex)
                        {
                            // If the task throws, capture the exception
                            tcs.SetException(ex);
                        }
                    }, null);

                    // Wait for the task to complete (success or failure)
                    await tcs.Task;
                }
                catch (Exception ex)
                {
                    // Log any errors that occurred during execution
                    // This prevents silent failures in fire-and-forget tasks
                    Debug.LogError($"Fire and forget task failed: {ex}");
                }
            });
        }
    }
} 