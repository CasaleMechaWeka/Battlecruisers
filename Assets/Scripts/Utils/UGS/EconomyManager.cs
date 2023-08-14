using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Economy.Model;
using Unity.Services.Economy;
using UnityEngine;

namespace BattleCruisers.Utils.UGS.Samples
{
    public class EconomyManager
    {
        public static Task<GetBalancesResult> GetEconomyBalances()
        {
            var options = new GetBalancesOptions { ItemsPerFetch = 100 };
            return EconomyService.Instance.PlayerBalances.GetBalancesAsync(options);
        }

        public static async Task SetEconomyBalance(string currencyId, long balance)
        {
            await EconomyService.Instance.PlayerBalances.SetBalanceAsync(currencyId, balance);
        }
    }
}

