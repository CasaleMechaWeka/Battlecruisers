using BattleCruisers.Effects.Deaths.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths.Pools
{
    public class PvPShipDeathPoolProvider : IShipDeathPoolProvider
    {
        public IPool<IPoolable<Vector3>, Vector3> ArchonPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> AttackBoatPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> AttackRIBPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> FrigatePool { get; }
        public IPool<IPoolable<Vector3>, Vector3> DestroyerPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> SiegeDestroyerPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> GlassCannoneerPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> GunBoatPool { get; }
        public IPool<IPoolable<Vector3>, Vector3> TurtlePool { get; }

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

        private IPool<IPoolable<Vector3>, Vector3> CreateShipDeathPool(IPvPPrefabFactory prefabFactory, PvPShipDeathKey shipDeathKey)
        {
            return
                new Pool<IPoolable<Vector3>, Vector3>(
                    new PvPShipDeathFactory(
                        prefabFactory,
                        shipDeathKey));
        }

        public void SetInitialCapacity()
        {
            AttackBoatPool.AddCapacity(0);
            AttackRIBPool.AddCapacity(0);
            FrigatePool.AddCapacity(0);
            DestroyerPool.AddCapacity(0);
            SiegeDestroyerPool.AddCapacity(0);
            ArchonPool.AddCapacity(0);
            GlassCannoneerPool.AddCapacity(0);
            GunBoatPool.AddCapacity(0);
            TurtlePool.AddCapacity(0);
        }
    }
}