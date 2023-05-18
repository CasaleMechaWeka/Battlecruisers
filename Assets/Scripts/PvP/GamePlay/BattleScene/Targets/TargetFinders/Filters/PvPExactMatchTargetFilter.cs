using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters
{
    public class PvPExactMatchTargetFilter : IPvPExactMatchTargetFilter
    {
        public IPvPTarget Target { private get; set; }

        public virtual bool IsMatch(IPvPTarget target)
        {
            return ReferenceEquals(Target, target);
        }
    }
}
