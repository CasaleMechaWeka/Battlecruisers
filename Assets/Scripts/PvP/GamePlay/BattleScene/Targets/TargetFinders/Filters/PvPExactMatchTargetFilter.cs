using BattleCruisers.Buildables;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters
{
    public class PvPExactMatchTargetFilter : IExactMatchTargetFilter
    {
        public ITarget Target { private get; set; }

        public virtual bool IsMatch(ITarget target)
        {
            return ReferenceEquals(Target, target);
        }

        public virtual bool IsMatch(ITarget target, VariantPrefab variant)
        {
            return ReferenceEquals(Target, target);
        }
    }
}
