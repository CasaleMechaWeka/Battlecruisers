using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Stats;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails
{
    public class PvPCruiserDetailsController : PvPItemDetails<IPvPCruiser>
    {
        protected override PvPStatsController<IPvPCruiser> GetStatsController()
        {
            return GetComponentInChildren<PvPCruiserStatsController>();
        }
    }
}