using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters.BoundsFinders;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters.BoundsFinders
{
    public class PvPVerticalTargetBoundsFinder : TargetBoundsFinder
    {
        public PvPVerticalTargetBoundsFinder(float targetYMarginInM)
            : base(targetXMarginInM: 0, targetYMarginInM: targetYMarginInM)
        {
        }
    }
}
