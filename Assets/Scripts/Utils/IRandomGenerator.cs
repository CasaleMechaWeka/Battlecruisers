using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Utils
{
    public enum ChangeDirection
    {
        Down, Up, Both
    }

    public interface IRandomGenerator
    {
        // Random value between 0 and 1 inclusive.
        float Value { get; }

        bool NextBool();
		float RangeFromCenter(float center, float radius);
        float Range(IRange<float> range);
        float Range(float minInclusive, float maxInclusive);
        int Range(int minInclusive, int maxInclusive);
        float Randomise(float baseValue, float maxChangeByProportionOfBaseValue, ChangeDirection changeDirection);
    }
}
