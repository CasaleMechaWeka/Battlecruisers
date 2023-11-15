using System.Collections;
using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache
{
    public interface IPvPMultiCache<TPrefab> where TPrefab : class
    {
        TPrefab GetPrefab(IPvPPrefabKey prefabKey);
        ICollection<IPvPPrefabKey> GetKeys();
        ICollection<TPrefab> GetValues();


    }
}