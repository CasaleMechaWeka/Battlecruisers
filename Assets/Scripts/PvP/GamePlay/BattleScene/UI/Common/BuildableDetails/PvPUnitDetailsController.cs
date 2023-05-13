using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public class PvPUnitDetailsController : PvPItemDetails<IPvPUnit>
    {
        protected override PvPStatsController<IPvPUnit> GetStatsController()
        {
            return GetComponentInChildren<PvPUnitStatsController>();
        }
    }
}
