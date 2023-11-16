using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Stats;
using BattleCruisers.UI.Common.BuildableDetails;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public class PvPUnitDetailsController : PvPItemDetails<IPvPUnit>
    {
        protected override PvPStatsController<IPvPUnit> GetStatsController()
        {
            return GetComponentInChildren<PvPUnitStatsController>();
        }
        public override PvPUnitVariantDetailController GetUnitVariantDetailController() { return GetComponent<PvPUnitVariantDetailController>(); }
    }
}
