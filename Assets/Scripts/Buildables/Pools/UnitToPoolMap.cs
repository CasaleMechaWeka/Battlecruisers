using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils.BattleScene.Pools;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Pools
{
    public class UnitToPoolMap
    {
        private readonly UnitPoolProvider _unitPoolProvider;

        public UnitToPoolMap(UnitPoolProvider unitPoolProvider)
        {
            Assert.IsNotNull(unitPoolProvider);
            _unitPoolProvider = unitPoolProvider;
        }

        public Pool<Unit, BuildableActivationArgs> GetPool(IUnit unit)
        {
            Assert.IsNotNull(unit);

            return unit.Category switch
            {
                UnitCategory.Aircraft => GetAircraftPool(unit),
                UnitCategory.Naval => GetShipPool(unit),
                _ => throw new ArgumentException($"Unsupported unit category: {unit.Category}"),
            };
        }

        private Pool<Unit, BuildableActivationArgs> GetAircraftPool(IUnit aircraft)
        {
            return aircraft.PrefabName switch
            {
                "Bomber" => _unitPoolProvider.BomberPool,
                "Fighter" => _unitPoolProvider.FighterPool,
                "Gunship" => _unitPoolProvider.GunshipPool,
                "SteamCopter" => _unitPoolProvider.SteamCopterPool,
                "Broadsword" => _unitPoolProvider.BroadswordPool,
                "StratBomber" => _unitPoolProvider.StratBomberPool,
                "SpyPlane" => _unitPoolProvider.SpyPlanePool,
                "TestAircraft" => _unitPoolProvider.TestAircraftPool,
                "MissileFighter" => _unitPoolProvider.MissileFighterPool,
                _ => throw new ArgumentException($"Unsupported aircraft: {aircraft.PrefabName}"),
            };
        }

        private Pool<Unit, BuildableActivationArgs> GetShipPool(IUnit ship)
        {
            return ship.PrefabName switch
            {
                "AttackBoat" => _unitPoolProvider.AttackBoatPool,
                "Frigate" => _unitPoolProvider.FrigatePool,
                "Destroyer" => _unitPoolProvider.DestroyerPool,
                "SiegeDestroyer" => _unitPoolProvider.SiegeDestroyerPool,
                "ArchonBattleship" => _unitPoolProvider.ArchonPool,
                "AttackRIB" => _unitPoolProvider.AttackRIBPool,
                "GlassCannoneer" => _unitPoolProvider.GlassCannoneerPool,
                "GunBoat" => _unitPoolProvider.GunBoatPool,
                "RocketTurtle" => _unitPoolProvider.RocketTurtlePool,
                "FlakTurtle" => _unitPoolProvider.FlakTurtlePool,
                "TeslaTurtle" => _unitPoolProvider.TeslaTurtlePool,
                _ => throw new ArgumentException($"Unsupported ship: {ship.PrefabName}"),
            };
        }
    }
}