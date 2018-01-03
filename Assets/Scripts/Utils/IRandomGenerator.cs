namespace BattleCruisers.Utils
{
    public interface IRandomGenerator
    {
		float RangeFromCenter(float center, float radius);
        float Range(float minInclusive, float maxInclusive);
    }
}
