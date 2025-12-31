using System.Threading.Tasks;
using Unity.Services.Economy.Model;
using Unity.Services.Economy;
using UnityEngine;
using System;
using Unity.Services.Authentication;
using Unity.Services.Core;

namespace BattleCruisers.Utils.UGS.Samples
{
    public class EconomyManager
    {
        public static Task<GetBalancesResult> GetEconomyBalances()
        {
            var options = new GetBalancesOptions { ItemsPerFetch = 100 };
            return EconomyService.Instance.PlayerBalances.GetBalancesAsync(options);
        }

        public static Task<GetInventoryResult> GetEconomyInventories()
        {
            var options = new GetInventoryOptions { ItemsPerFetch = 100 };
            return EconomyService.Instance.PlayerInventory.GetInventoryAsync(options);
        }

        public static async Task SetEconomyBalance(string currencyId, long balance)
        {
            try
            {
                if (UnityServices.State != ServicesInitializationState.Initialized || !AuthenticationService.Instance.IsSignedIn)
                {
                    Debug.LogWarning($"UGS not ready. Skipping SetBalance for {currencyId}={balance}.");
                    return;
                }

                if (string.IsNullOrEmpty(currencyId))
                {
                    Debug.LogError("SetEconomyBalance: currencyId is null or empty.");
                    return;
                }

                if (balance < 0)
                {
                    Debug.LogWarning($"SetEconomyBalance: clamping negative {currencyId} from {balance} to 0.");
                    balance = 0;
                }

                await EconomyService.Instance.PlayerBalances.SetBalanceAsync(currencyId, balance);
            }
            catch (EconomyException e)
            {
                Debug.LogError($"SetEconomyBalance validation failed for {currencyId}={balance}: {e}");
            }
            catch (Exception e)
            {
                Debug.LogError($"SetEconomyBalance failed for {currencyId}={balance}: {e}");
            }
        }

        public static async Task<MakeVirtualPurchaseResult> MakeVirtualPurchaseAsync(string virtualPurchaseId)
        {
            try
            {
                return await EconomyService.Instance.Purchases.MakeVirtualPurchaseAsync(virtualPurchaseId);
            }
            catch (EconomyException e)
            {
                Debug.LogException(e);
                return null;
            }
        }

        /*        public static async Task<RedeemGooglePlayPurchaseResult> MakeRealMoneyPurchaseAsync(string purchaseId)
                {
                    try
                    {
                        RedeemGooglePlayStorePurchaseArgs args = new RedeemGooglePlayStorePurchaseArgs(purchaseId, "PURCHASE_DATA", "PURCHASE_DATA_SIGNATURE", 0, "USD");
                        return await EconomyService.Instance.Purchases.RedeemGooglePlayPurchaseAsync()
                    }
                }*/

        public static async Task RefreshCurrencyBalances()
        {
            GetBalancesResult balanceResult = null;

            try
            {
                balanceResult = await GetEconomyBalances();
            }
            catch (EconomyRateLimitedException e)
            {

                balanceResult = await Utils.RetryEconomyFunction(GetEconomyBalances, e.RetryAfter);
            }
            catch (Exception e)
            {
                Debug.Log("Problem getting Economy currency balances:");
                Debug.LogException(e);
            }
        }
    }
}

