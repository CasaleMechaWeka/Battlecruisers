using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
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

        public IPvPPool<PvPUnit, PvPBuildableActivationArgs> GetPool(IPvPUnit unit)
        {
            Assert.IsNotNull(unit);

            switch (unit.Category)
            {
                case PvPUnitCategory.Aircraft:
                    return GetAircraftPool(unit);

                case PvPUnitCategory.Naval:
                    return GetShipPool(unit);

                default:
                    throw new ArgumentException($"Unsupported unit category: {unit.Category}");
            }
        }

        private IPvPPool<PvPUnit, PvPBuildableActivationArgs> GetAircraftPool(IPvPUnit aircraft)
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

        private IPvPPool<PvPUnit, PvPBuildableActivationArgs> GetShipPool(IPvPUnit ship)
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