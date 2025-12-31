using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils.BattleScene.Pools;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Pools
{
    public class UnitPoolProvider
    {
        private readonly IList<Pool<Unit, BuildableActivationArgs>> _pools;

        // Don't want more than 1 because unit may never be built.  Want at least 1
        // to force prefab to be loaded.  First time load is the slowest, because
        // it fetches everything the prefab needs (materials, sprites???).
        private const int INITIAL_UNIT_CAPACITY = 1;

        // Aircraft
        public Pool<Unit, BuildableActivationArgs> BomberPool { get; }
        public Pool<Unit, BuildableActivationArgs> FighterPool { get; }
        public Pool<Unit, BuildableActivationArgs> GunshipPool { get; }
        public Pool<Unit, BuildableActivationArgs> SteamCopterPool { get; }
        public Pool<Unit, BuildableActivationArgs> BroadswordPool { get; }
        public Pool<Unit, BuildableActivationArgs> StratBomberPool { get; }
        public Pool<Unit, BuildableActivationArgs> SpyPlanePool { get; }
        public Pool<Unit, BuildableActivationArgs> TestAircraftPool { get; }
        public Pool<Unit, BuildableActivationArgs> MissileFighterPool { get; }

        // Ships
        public Pool<Unit, BuildableActivationArgs> AttackBoatPool { get; }
        public Pool<Unit, BuildableActivationArgs> AttackRIBPool { get; }
        public Pool<Unit, BuildableActivationArgs> FrigatePool { get; }
        public Pool<Unit, BuildableActivationArgs> DestroyerPool { get; }
        public Pool<Unit, BuildableActivationArgs> SiegeDestroyerPool { get; }
        public Pool<Unit, BuildableActivationArgs> ArchonPool { get; }
        public Pool<Unit, BuildableActivationArgs> GlassCannoneerPool { get; }
        public Pool<Unit, BuildableActivationArgs> GunBoatPool { get; }
        public Pool<Unit, BuildableActivationArgs> RocketTurtlePool { get; }
        public Pool<Unit, BuildableActivationArgs> FlakTurtlePool { get; }
        public Pool<Unit, BuildableActivationArgs> TeslaTurtlePool { get; }

        public UnitPoolProvider()
        {
            _pools = new List<Pool<Unit, BuildableActivationArgs>>();

            // Aircraft - check each unit key for null before creating pool
            if (StaticPrefabKeys.Units.Bomber == null) Debug.LogError("StaticPrefabKeys.Units.Bomber is null!");
            BomberPool = CreatePool(StaticPrefabKeys.Units.Bomber);
            
            if (StaticPrefabKeys.Units.Fighter == null) Debug.LogError("StaticPrefabKeys.Units.Fighter is null!");
            FighterPool = CreatePool(StaticPrefabKeys.Units.Fighter);
            
            if (StaticPrefabKeys.Units.Gunship == null) Debug.LogError("StaticPrefabKeys.Units.Gunship is null!");
            GunshipPool = CreatePool(StaticPrefabKeys.Units.Gunship);
            
            if (StaticPrefabKeys.Units.SteamCopter == null) Debug.LogError("StaticPrefabKeys.Units.SteamCopter is null!");
            SteamCopterPool = CreatePool(StaticPrefabKeys.Units.SteamCopter);
            
            if (StaticPrefabKeys.Units.Broadsword == null) Debug.LogError("StaticPrefabKeys.Units.Broadsword is null!");
            BroadswordPool = CreatePool(StaticPrefabKeys.Units.Broadsword);
            
            if (StaticPrefabKeys.Units.StratBomber == null) Debug.LogError("StaticPrefabKeys.Units.StratBomber is null!");
            StratBomberPool = CreatePool(StaticPrefabKeys.Units.StratBomber);
            
            if (StaticPrefabKeys.Units.SpyPlane == null) Debug.LogError("StaticPrefabKeys.Units.SpyPlane is null!");
            SpyPlanePool = CreatePool(StaticPrefabKeys.Units.SpyPlane);
            
            if (StaticPrefabKeys.Units.TestAircraft == null) Debug.LogError("StaticPrefabKeys.Units.TestAircraft is null!");
            TestAircraftPool = CreatePool(StaticPrefabKeys.Units.TestAircraft);
            
            if (StaticPrefabKeys.Units.MissileFighter == null) Debug.LogError("StaticPrefabKeys.Units.MissileFighter is null!");
            MissileFighterPool = CreatePool(StaticPrefabKeys.Units.MissileFighter);

            // Ships - check each unit key for null before creating pool
            if (StaticPrefabKeys.Units.AttackBoat == null) Debug.LogError("StaticPrefabKeys.Units.AttackBoat is null!");
            AttackBoatPool = CreatePool(StaticPrefabKeys.Units.AttackBoat);
            
            if (StaticPrefabKeys.Units.AttackRIB == null) Debug.LogError("StaticPrefabKeys.Units.AttackRIB is null!");
            AttackRIBPool = CreatePool(StaticPrefabKeys.Units.AttackRIB);
            
            if (StaticPrefabKeys.Units.Frigate == null) Debug.LogError("StaticPrefabKeys.Units.Frigate is null!");
            FrigatePool = CreatePool(StaticPrefabKeys.Units.Frigate);
            
            if (StaticPrefabKeys.Units.Destroyer == null) Debug.LogError("StaticPrefabKeys.Units.Destroyer is null!");
            DestroyerPool = CreatePool(StaticPrefabKeys.Units.Destroyer);
            
            if (StaticPrefabKeys.Units.SiegeDestroyer == null) Debug.LogError("StaticPrefabKeys.Units.SiegeDestroyer is null!");
            SiegeDestroyerPool = CreatePool(StaticPrefabKeys.Units.SiegeDestroyer);
            
            if (StaticPrefabKeys.Units.ArchonBattleship == null) Debug.LogError("StaticPrefabKeys.Units.ArchonBattleship is null!");
            ArchonPool = CreatePool(StaticPrefabKeys.Units.ArchonBattleship);
            
            if (StaticPrefabKeys.Units.GlassCannoneer == null) Debug.LogError("StaticPrefabKeys.Units.GlassCannoneer is null!");
            GlassCannoneerPool = CreatePool(StaticPrefabKeys.Units.GlassCannoneer);
            
            if (StaticPrefabKeys.Units.GunBoat == null) Debug.LogError("StaticPrefabKeys.Units.GunBoat is null!");
            GunBoatPool = CreatePool(StaticPrefabKeys.Units.GunBoat);
            
            if (StaticPrefabKeys.Units.RocketTurtle == null) Debug.LogError("StaticPrefabKeys.Units.RocketTurtle is null!");
            RocketTurtlePool = CreatePool(StaticPrefabKeys.Units.RocketTurtle);
            
            if (StaticPrefabKeys.Units.FlakTurtle == null) Debug.LogError("StaticPrefabKeys.Units.FlakTurtle is null!");
            FlakTurtlePool = CreatePool(StaticPrefabKeys.Units.FlakTurtle);
            
            if (StaticPrefabKeys.Units.TeslaTurtle == null) Debug.LogError("StaticPrefabKeys.Units.TeslaTurtle is null!");
            TeslaTurtlePool = CreatePool(StaticPrefabKeys.Units.TeslaTurtle);
        }

        private Pool<Unit, BuildableActivationArgs> CreatePool(IPrefabKey unitKey)
        {
            if (unitKey == null)
            {
                // Create a stack trace to identify which unit is null
                System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
                string callerName = stackTrace.GetFrame(1)?.GetMethod()?.Name ?? "Unknown";
                Debug.LogError($"UnitPoolProvider: Attempted to create pool with null unit key in {callerName}. This unit may not be defined in StaticPrefabKeys.Units.");
                throw new System.ArgumentNullException(nameof(unitKey), $"Unit key is null when creating pool in {callerName}");
            }
            
            try
            {
                Pool<Unit, BuildableActivationArgs> pool
                    = new Pool<Unit, BuildableActivationArgs>(
                        new UnitFactory(unitKey));
                _pools.Add(pool);
                return pool;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"UnitPoolProvider: Failed to create pool for unit {unitKey.PrefabName}: {ex.Message}\n{ex.StackTrace}");
                throw; // Re-throw to see full stack trace
            }
        }

        public void SetInitialCapacity()
        {
            foreach (Pool<Unit, BuildableActivationArgs> pool in _pools)
            {
                pool.AddCapacity(INITIAL_UNIT_CAPACITY);
            }
        }
    }
}