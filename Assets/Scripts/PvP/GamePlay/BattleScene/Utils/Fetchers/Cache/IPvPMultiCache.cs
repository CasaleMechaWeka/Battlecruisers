using BattleCruisers.Data.Models.PrefabKeys;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Cache
{
    public interface IPvPMultiCache<TPrefab> where TPrefab : class
    {
        TPrefab GetPrefab(IPrefabKey prefabKey);
        ICollection<IPrefabKey> GetKeys();
        ICollection<TPrefab> GetValues();


    }
}