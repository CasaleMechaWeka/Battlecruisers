using BattleCruisers.Effects.Deaths.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths.Pools
{
    public class PvPShipDeathPoolProvider : IShipDeathPoolProvider
    {
        public Pool<IPoolable<Vector3>, Vector3> ArchonPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> AttackBoatPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> AttackRIBPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> FrigatePool { get; }
        public Pool<IPoolable<Vector3>, Vector3> DestroyerPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> SiegeDestroyerPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> GlassCannoneerPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> GunBoatPool { get; }
        public Pool<IPoolable<Vector3>, Vector3> TurtlePool { get; }

        public PvPShipDeathPoolProvider()
        {
            AttackBoatPool = CreateShipDeathPool(PvPStaticPrefabKeys.PvPShipDeaths.PvPAttackBoat);
            AttackRIBPool = CreateShipDeathPool(PvPStaticPrefabKeys.PvPShipDeaths.PvPAttackRIB);
            FrigatePool = CreateShipDeathPool(PvPStaticPrefabKeys.PvPShipDeaths.PvPFrigate);
            DestroyerPool = CreateShipDeathPool(PvPStaticPrefabKeys.PvPShipDeaths.PvPDestroyer);
            SiegeDestroyerPool = CreateShipDeathPool(PvPStaticPrefabKeys.PvPShipDeaths.PvPSiegeDestroyer);
            ArchonPool = CreateShipDeathPool(PvPStaticPrefabKeys.PvPShipDeaths.PvPArchon);
            GlassCannoneerPool = CreateShipDeathPool(PvPStaticPrefabKeys.PvPShipDeaths.PvPGlassCannoneer);
            GunBoatPool = CreateShipDeathPool(PvPStaticPrefabKeys.PvPShipDeaths.PvPGunBoat);
            TurtlePool = CreateShipDeathPool(PvPStaticPrefabKeys.PvPShipDeaths.PvPTurtle);
        }

        private Pool<IPoolable<Vector3>, Vector3> CreateShipDeathPool(PvPShipDeathKey shipDeathKey)
        {
            return
                new Pool<IPoolable<Vector3>, Vector3>(
                    new PvPShipDeathFactory(shipDeathKey));
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