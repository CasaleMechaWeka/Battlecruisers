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
                case "PvPBomber":
                    return _unitPoolProvider.BomberPool;

                case "PvPFighter":
                    return _unitPoolProvider.FighterPool;

                case "PvPGunship":
                    return _unitPoolProvider.GunshipPool;

                case "PvPSteamCopter":
                    return _unitPoolProvider.SteamCopterPool;

                case "PvPTestAircraft":
                    return _unitPoolProvider.TestAircraftPool;

                default:
                    throw new ArgumentException($"Unsupported aircraft: {aircraft.PrefabName}");
            }
        }

        private IPvPPool<PvPUnit, PvPBuildableActivationArgs> GetShipPool(IPvPUnit ship)
        {
            switch (ship.PrefabName)
            {
                case "PvPAttackBoat":
                    return _unitPoolProvider.AttackBoatPool;

                case "PvPFrigate":
                    return _unitPoolProvider.FrigatePool;

                case "PvPDestroyer":
                    return _unitPoolProvider.DestroyerPool;

                case "PvPArchonBattleship":
                    return _unitPoolProvider.ArchonPool;

                case "PvPAttackRIB":
                    return _unitPoolProvider.AttackRIBPool;

                default:
                    throw new ArgumentException($"Unsupported ship: {ship.PrefabName}");
            }
        }
    }
}