using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys.Wrappers
{
    public class PvPStaticPrefabKeyWrapper : IPvPPrefabKeyWrapper
    {
        public bool HasKey => true;
        public PvPBuildingKey Key { get; }

        public PvPStaticPrefabKeyWrapper(PvPBuildingKey key)
        {
            Assert.IsNotNull(key);
            Key = key;
        }

        public void Initialise(IPvPBuildOrders buildOrders)
        {
            // Do nothing
        }
    }
}
