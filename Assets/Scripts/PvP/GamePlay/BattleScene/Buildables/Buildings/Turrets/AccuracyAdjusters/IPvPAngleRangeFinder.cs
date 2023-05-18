using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public interface IPvPAngleRangeFinder
    {
        IPvPRange<float> FindFireAngleRange(IPvPRange<float> onTargetRange, float accuracy);
    }
}
