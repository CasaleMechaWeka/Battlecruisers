using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils.BattleScene.Pools;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Pools
{
    public class UnitToPoolMap : IUnitToPoolMap
    {
        private readonly IUnitPoolProvider _unitPoolProvider;

        public UnitToPoolMap(IUnitPoolProvider unitPoolProvider)
        {
            Assert.IsNotNull(unitPoolProvider);
            _unitPoolProvider = unitPoolProvider;
        }

        public IPool<Unit, BuildableActivationArgs> GetPool(IUnit unit)
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

        private IPool<Unit, BuildableActivationArgs> GetAircraftPool(IUnit aircraft)
        {
            switch (aircraft.PrefabName)
            {
                case "Bomber":
                    return _unitPoolProvider.BomberPool;

                case "Fighter":
                    return _unitPoolProvider.FighterPool;

                case "Gunship":
                    return _unitPoolProvider.GunshipPool;

                case "SteamCopter":
                    return _unitPoolProvider.SteamCopterPool;

                case "TestAircraft":
                    return _unitPoolProvider.TestAircraftPool;

                default:
                    throw new ArgumentException($"Unsupported aircraft: {aircraft.PrefabName}");
            }
        }

        private IPool<Unit, BuildableActivationArgs> GetShipPool(IUnit ship)
        {
            switch (ship.PrefabName)
            {
                case "AttackBoat":
                    return _unitPoolProvider.AttackBoatPool;

                case "Frigate":
                    return _unitPoolProvider.FrigatePool;

                case "Destroyer":
                    return _unitPoolProvider.DestroyerPool;

                case "ArchonBattleship":
                    return _unitPoolProvider.ArchonPool;

                case "AttackRIB":
                    return _unitPoolProvider.AttackRIBPool;

                default:
                    throw new ArgumentException($"Unsupported ship: {ship.PrefabName}");
            }
        }
    }
}