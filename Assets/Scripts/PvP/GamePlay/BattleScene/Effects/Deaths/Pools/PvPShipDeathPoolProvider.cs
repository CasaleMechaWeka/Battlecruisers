using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths.Pools
{
    public class PvPShipDeathPoolProvider : IPvPShipDeathPoolProvider
    {
        public IPvPPool<IPvPShipDeath, Vector3> ArchonPool { get; }
        public IPvPPool<IPvPShipDeath, Vector3> AttackBoatPool { get; }
        public IPvPPool<IPvPShipDeath, Vector3> AttackRIBPool { get; }
        public IPvPPool<IPvPShipDeath, Vector3> FrigatePool { get; }
        public IPvPPool<IPvPShipDeath, Vector3> DestroyerPool { get; }

        public PvPShipDeathPoolProvider(IPvPPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);

            AttackBoatPool = CreateShipDeathPool(prefabFactory, PvPStaticPrefabKeys.PvPShipDeaths.PvPAttackBoat);
            AttackRIBPool = CreateShipDeathPool(prefabFactory, PvPStaticPrefabKeys.PvPShipDeaths.PvPAttackRIB);
            FrigatePool = CreateShipDeathPool(prefabFactory, PvPStaticPrefabKeys.PvPShipDeaths.PvPFrigate);
            DestroyerPool = CreateShipDeathPool(prefabFactory, PvPStaticPrefabKeys.PvPShipDeaths.PvPDestroyer);
            ArchonPool = CreateShipDeathPool(prefabFactory, PvPStaticPrefabKeys.PvPShipDeaths.PvPArchon);
        }

        private IPvPPool<IPvPShipDeath, Vector3> CreateShipDeathPool(IPvPPrefabFactory prefabFactory, PvPShipDeathKey shipDeathKey)
        {
            return
                new PvPPool<IPvPShipDeath, Vector3>(
                    new PvPShipDeathFactory(
                        prefabFactory,
                        shipDeathKey));
        }

        public async Task SetInitialCapacity()
        {
            await AttackBoatPool.AddCapacity(PvPInitialCapacity.ATTACK_BOAT);
            await AttackRIBPool.AddCapacity(PvPInitialCapacity.ATTACK_RIB);
            await FrigatePool.AddCapacity(PvPInitialCapacity.FRIGATE);
            await DestroyerPool.AddCapacity(PvPInitialCapacity.DESTROYFER);
            await ArchonPool.AddCapacity(PvPInitialCapacity.ARCHON);
        }
    }
}