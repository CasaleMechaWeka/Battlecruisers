using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public class PvPBuildingDetailsController : PvPItemDetails<IPvPBuilding>
    {
        protected override PvPStatsController<IPvPBuilding> GetStatsController()
        {
            return GetComponentInChildren<PvPBuildingStatsController>();
        }
    }
}
