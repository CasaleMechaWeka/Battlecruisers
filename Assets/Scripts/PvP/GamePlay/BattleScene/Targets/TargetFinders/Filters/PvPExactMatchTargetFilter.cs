using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters
{
    public class PvPExactMatchTargetFilter : IPvPExactMatchTargetFilter
    {
        public IPvPTarget Target { private get; set; }

        public virtual bool IsMatch(IPvPTarget target)
        {
            return ReferenceEquals(Target, target);
        }

        public virtual bool IsMatch(IPvPTarget target, VariantPrefab variant)
        {
            return ReferenceEquals(Target, target);
        }
    }
}
