namespace BattleCruisers.Utils
{
    public enum ChangeDirection
    {
        Down, Up, Both
    }

    public interface IRandomGenerator
    {
        bool NextBool();
		float RangeFromCenter(float center, float radius);
        float Range(float minInclusive, float maxInclusive);
        int Range(int minInclusive, int maxInclusive);
        float Randomise(float baseValue, float maxChangeByProportionOfBaseValue, ChangeDirection changeDirection);
    }
}
