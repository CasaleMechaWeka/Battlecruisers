using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public interface IAngleRangeFinder
    {
        IRange<float> FindFireAngleRange(IRange<float> onTargetRange, float accuracy);
    }
}
