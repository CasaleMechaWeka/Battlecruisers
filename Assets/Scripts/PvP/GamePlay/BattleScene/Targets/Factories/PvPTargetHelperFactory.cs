using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Ships;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Helpers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPTargetHelperFactory : IPvPTargetHelperFactory
    {
        public IPvPTargetRangeHelper CreateShipRangeHelper(IPvPShip ship)
        {
            return new PvPShipRangeHelper(ship);
        }
    }
}
