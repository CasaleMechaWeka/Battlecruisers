using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Buttons
{
    public class PvPDeleteButtonVisibilityFilter : IPvPFilter<IPvPTarget>
    {
        // Player building
        public bool IsMatch(IPvPTarget target)
        {
            return
                target != null
                /*     && SynchedServerData.Instance.GetTeam() == Cruisers.Team.LEFT? target.Faction == PvPFaction.Blues : target.Faction == PvPFaction.Reds*/
                && target.TargetType == PvPTargetType.Buildings
                && target.IsInScene;
        }


        public bool IsMatch(IPvPTarget target, VariantPrefab variant)
        {
            return
                target != null
        /*        && target.Faction == Faction.Blues*/
                && target.TargetType == PvPTargetType.Buildings
                && target.IsInScene;
        }
    }
}
