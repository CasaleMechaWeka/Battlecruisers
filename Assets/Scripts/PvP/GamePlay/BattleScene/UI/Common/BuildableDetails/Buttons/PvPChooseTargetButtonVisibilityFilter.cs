using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Buttons
{
    public class PvPChooseTargetButtonVisibilityFilter : IPvPFilter<IPvPTarget>
    {
        // AI buildings or cruiser
        public bool IsMatch(IPvPTarget target)
        {
            return
                target != null
                /*    && SynchedServerData.Instance.GetTeam() == Cruisers.Team.LEFT? target.Faction == PvPFaction.Reds : target.Faction == PvPFaction.Blues*/
                && (target.TargetType == PvPTargetType.Buildings
                    || target.TargetType == PvPTargetType.Cruiser);
        }
    }
}
