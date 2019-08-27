using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils.BattleScene.Pools;
using System;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Pools
{
    // FELIX  Test???
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
            switch (aircraft.Name)
            {
                case "Bomber":
                    return _unitPoolProvider.BomberPool;

                case "Fighter":
                    return _unitPoolProvider.FighterPool;

                case "Turtle Gunship":
                    return _unitPoolProvider.GunshipPool;

                default:
                    throw new ArgumentException($"Unsupported aircraft: {aircraft.Name}");
            }
        }

        private IPool<Unit, BuildableActivationArgs> GetShipPool(IUnit ship)
        {
            switch (ship.Name)
            {
                case "Attack Boat":
                    return _unitPoolProvider.AttackBoatPool;

                case "Frigate":
                    return _unitPoolProvider.FrigatePool;

                case "Destroyer":
                    return _unitPoolProvider.DestroyerPool;

                case "Archon Battleship":
                    return _unitPoolProvider.ArchonPool;

                default:
                    throw new ArgumentException($"Unsupported ship: {ship.Name}");
            }
        }
    }
}