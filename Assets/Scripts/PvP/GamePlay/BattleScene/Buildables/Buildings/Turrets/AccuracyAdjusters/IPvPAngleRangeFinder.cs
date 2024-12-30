using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public interface IPvPAngleRangeFinder
    {
        IRange<float> FindFireAngleRange(IRange<float> onTargetRange, float accuracy);
    }
}
