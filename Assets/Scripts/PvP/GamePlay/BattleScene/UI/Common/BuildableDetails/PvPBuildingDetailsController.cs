using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Stats;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.Common.BuildableDetails.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public class PvPBuildingDetailsController : PvPItemDetails<IPvPBuilding>
    {
        protected override StatsController<IPvPBuilding> GetStatsController()
        {
            return GetComponentInChildren<PvPBuildingStatsController>();
        }

        public override BuildingVariantDetailController GetBuildingVariantDetailController()
        {
            return GetComponent<BuildingVariantDetailController>();
        }
    }
}
