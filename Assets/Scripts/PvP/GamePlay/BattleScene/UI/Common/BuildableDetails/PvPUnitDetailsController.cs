using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Stats;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Common.BuildableDetails.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public class PvPUnitDetailsController : PvPItemDetails<IPvPUnit>
    {
        protected override StatsController<IPvPUnit> GetStatsController()
        {
            return GetComponentInChildren<PvPUnitStatsController>();
        }
        public override UnitVariantDetailController GetUnitVariantDetailController() { return GetComponent<UnitVariantDetailController>(); }
    }
}
