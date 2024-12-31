using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Buttons
{
    public class PvPDeleteButtonVisibilityFilter : IFilter<IPvPTarget>
    {
        // Player building
        public bool IsMatch(IPvPTarget target)
        {
            return
                target != null
                /*     && SynchedServerData.Instance.GetTeam() == Cruisers.Team.LEFT? target.Faction == PvPFaction.Blues : target.Faction == PvPFaction.Reds*/
                && target.TargetType == TargetType.Buildings
                && target.IsInScene;
        }


        public bool IsMatch(IPvPTarget target, VariantPrefab variant)
        {
            return
                target != null
                /*        && target.Faction == Faction.Blues*/
                && target.TargetType == TargetType.Buildings
                && target.IsInScene;
        }
    }
}
