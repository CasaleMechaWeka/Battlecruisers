using BattleCruisers.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Utils.BattleScene.Pools;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools
{
    public class PvPUnitToPoolMap : IPvPUnitToPoolMap
    {
        private readonly IPvPUnitPoolProvider _unitPoolProvider;

        public PvPUnitToPoolMap(IPvPUnitPoolProvider unitPoolProvider)
        {
            Assert.IsNotNull(unitPoolProvider);
            _unitPoolProvider = unitPoolProvider;
        }

        public IPool<PvPUnit, PvPBuildableActivationArgs> GetPool(IPvPUnit unit)
        {
            Assert.IsNotNull(unit);

            switch (unit.Category)
            {
                case UnitCategory.Aircraft:
                    return GetAircraftPool(unit);

                case UnitCategory.Naval:
                    return GetShipPool(unit);

                default:
                    throw new ArgumentException($"Unsupported unit category: {unit.Category}");
            }
        }

        private IPool<PvPUnit, PvPBuildableActivationArgs> GetAircraftPool(IPvPUnit aircraft)
        {
            switch (aircraft.PrefabName)
            {
                case "PvPBomber":
                    return _unitPoolProvider.BomberPool;

                case "PvPFighter":
                    return _unitPoolProvider.FighterPool;

                case "PvPGunship":
                    return _unitPoolProvider.GunshipPool;

                case "PvPSteamCopter":
                    return _unitPoolProvider.SteamCopterPool;

                case "PvPBroadsword":
                    return _unitPoolProvider.BroadswordPool;

                case "PvPStratBomber":
                    return _unitPoolProvider.StratBomberPool;

                case "PvPSpyPlane":
                    return _unitPoolProvider.SpyPlanePool;
                case "PvPMissileFighter":
                    return _unitPoolProvider.MissileFighterPool;

                case "PvPTestAircraft":
                    return _unitPoolProvider.TestAircraftPool;

                default:
                    throw new ArgumentException($"Unsupported aircraft: {aircraft.PrefabName}");
            }
        }

        private IPool<PvPUnit, PvPBuildableActivationArgs> GetShipPool(IPvPUnit ship)
        {
            switch (ship.PrefabName)
            {
                case "PvPAttackBoat":
                    return _unitPoolProvider.AttackBoatPool;

                case "PvPFrigate":
                    return _unitPoolProvider.FrigatePool;

                case "PvPDestroyer":
                    return _unitPoolProvider.DestroyerPool;

                case "PvPSiegeDestroyer":
                    return _unitPoolProvider.SiegeDestroyerPool;

                case "PvPArchonBattleship":
                    return _unitPoolProvider.ArchonPool;

                case "PvPAttackRIB":
                    return _unitPoolProvider.AttackRIBPool;

                case "PvPGlassCannoneer":
                    return _unitPoolProvider.GlassCannoneerPool;

                case "PvPGunBoat":
                    return _unitPoolProvider.GunBoatPool;

                case "PvPRocketTurtle":
                    return _unitPoolProvider.RocketTurtlePool;

                case "PvPFlakTurtle":
                    return _unitPoolProvider.FlakTurtlePool;

                default:
                    throw new ArgumentException($"Unsupported ship: {ship.PrefabName}");
            }
        }
    }
}