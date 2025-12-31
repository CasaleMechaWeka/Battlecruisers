using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine; // Required for Debug.Log

namespace BattleCruisers.Utils
{
    public static class TransactionLocker
    {
        private static readonly HashSet<int> _purchasesInProgress = new HashSet<int>();

        private static bool AcquireLock(int itemId)
        {
            bool acquired = _purchasesInProgress.Add(itemId);
#if UNITY_EDITOR
            if (acquired)
            {
                Debug.Log($"Transaction Lock ACQUIRED for item ID: {itemId}");
            }
#endif
            return acquired;
        }

        private static void ReleaseLock(int itemId)
        {
            _purchasesInProgress.Remove(itemId);
#if UNITY_EDITOR
            Debug.Log($"Transaction Lock RELEASED for item ID: {itemId}");
#endif
        }

        public static async Task ProcessTransaction(int itemId, Func<Task> purchaseAction)
        {
            if (!AcquireLock(itemId))
            {
#if UNITY_EDITOR
                Debug.Log($"Transaction Lock BLOCKED for item ID: {itemId} (already in progress)");
#endif
                return;
            }

            try
            {
                await purchaseAction();
            }
            finally
            {
                ReleaseLock(itemId);
            }
        }
    }
}
