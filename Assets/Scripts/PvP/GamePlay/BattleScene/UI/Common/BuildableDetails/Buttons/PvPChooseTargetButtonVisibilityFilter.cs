using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.Utils;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Buttons
{
    public class PvPChooseTargetButtonVisibilityFilter : IFilter<ITarget>
    {
        // AI buildings or cruiser
        public bool IsMatch(ITarget target)
        {
            return
                target != null
                /*    && SynchedServerData.Instance.GetTeam() == Cruisers.Team.LEFT? target.Faction == Faction.Reds : target.Faction == Faction.Blues*/
                && (target.TargetType == TargetType.Buildings
                    || target.TargetType == TargetType.Cruiser);
        }

        public bool IsMatch(ITarget target, VariantPrefab variant)
        {
            return
                target != null
                /* && target.Faction == Faction.Reds*/
                && (target.TargetType == TargetType.Buildings
                    || target.TargetType == TargetType.Cruiser);
        }
    }
}
