namespace BattleCruisers.Utils
{
    public interface IRandomGenerator
    {
		float RangeFromCenter(float center, float radius);
        float Range(float minInclusive, float maxInclusive);
        int Range(int minInclusive, int maxInclusive);
    }
}
