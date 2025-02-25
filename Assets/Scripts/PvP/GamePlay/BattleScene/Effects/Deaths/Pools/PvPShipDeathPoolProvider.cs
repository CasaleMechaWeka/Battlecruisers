using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Utils.BattleScene.Pools;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths.Pools
{
    public class PvPShipDeathPoolProvider : IPvPShipDeathPoolProvider
    {
        public IPvPPool<IPoolable<Vector3>, Vector3> ArchonPool { get; }
        public IPvPPool<IPoolable<Vector3>, Vector3> AttackBoatPool { get; }
        public IPvPPool<IPoolable<Vector3>, Vector3> AttackRIBPool { get; }
        public IPvPPool<IPoolable<Vector3>, Vector3> FrigatePool { get; }
        public IPvPPool<IPoolable<Vector3>, Vector3> DestroyerPool { get; }
        public IPvPPool<IPoolable<Vector3>, Vector3> SiegeDestroyerPool { get; }
        public IPvPPool<IPoolable<Vector3>, Vector3> GlassCannoneerPool { get; }
        public IPvPPool<IPoolable<Vector3>, Vector3> GunBoatPool { get; }
        public IPvPPool<IPoolable<Vector3>, Vector3> TurtlePool { get; }

        public PvPShipDeathPoolProvider(IPvPPrefabFactory prefabFactory)
        {
            Assert.IsNotNull(prefabFactory);

            AttackBoatPool = CreateShipDeathPool(prefabFactory, PvPStaticPrefabKeys.PvPShipDeaths.PvPAttackBoat);
            AttackRIBPool = CreateShipDeathPool(prefabFactory, PvPStaticPrefabKeys.PvPShipDeaths.PvPAttackRIB);
            FrigatePool = CreateShipDeathPool(prefabFactory, PvPStaticPrefabKeys.PvPShipDeaths.PvPFrigate);
            DestroyerPool = CreateShipDeathPool(prefabFactory, PvPStaticPrefabKeys.PvPShipDeaths.PvPDestroyer);
            SiegeDestroyerPool = CreateShipDeathPool(prefabFactory, PvPStaticPrefabKeys.PvPShipDeaths.PvPSiegeDestroyer);
            ArchonPool = CreateShipDeathPool(prefabFactory, PvPStaticPrefabKeys.PvPShipDeaths.PvPArchon);
            GlassCannoneerPool = CreateShipDeathPool(prefabFactory, PvPStaticPrefabKeys.PvPShipDeaths.PvPGlassCannoneer);
            GunBoatPool = CreateShipDeathPool(prefabFactory, PvPStaticPrefabKeys.PvPShipDeaths.PvPGunBoat);
            TurtlePool = CreateShipDeathPool(prefabFactory, PvPStaticPrefabKeys.PvPShipDeaths.PvPTurtle);
        }

        private IPvPPool<IPoolable<Vector3>, Vector3> CreateShipDeathPool(IPvPPrefabFactory prefabFactory, PvPShipDeathKey shipDeathKey)
        {
            return
                new PvPPool<IPoolable<Vector3>, Vector3>(
                    new PvPShipDeathFactory(
                        prefabFactory,
                        shipDeathKey));
        }

        public void SetInitialCapacity()
        {
            AttackBoatPool.AddCapacity(PvPInitialCapacity.ATTACK_BOAT);
            AttackRIBPool.AddCapacity(PvPInitialCapacity.ATTACK_RIB);
            FrigatePool.AddCapacity(PvPInitialCapacity.FRIGATE);
            DestroyerPool.AddCapacity(PvPInitialCapacity.DESTROYFER);
            SiegeDestroyerPool.AddCapacity(PvPInitialCapacity.SIEGEDESTROYER);
            ArchonPool.AddCapacity(PvPInitialCapacity.ARCHON);
            GlassCannoneerPool.AddCapacity(PvPInitialCapacity.GLASSCANNONEER);
            GunBoatPool.AddCapacity(PvPInitialCapacity.GUNBOAT);
            TurtlePool.AddCapacity(PvPInitialCapacity.TURTLE);
        }
    }
}