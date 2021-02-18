using System.Collections.Generic;
using UnityCommon.DataStrctures;

// FELIX  Remove :D
namespace UnityCommon.Utils
{
    public enum ChangeDirection
    {
        Down, Up, Both
    }

    public interface IRandomGenerator
    {
        bool NextBool();
		float RangeFromCenter(float center, float radius);

        float Range(IRange<float> range);
        float Range(float minInclusive, float maxInclusive);

        int Range(IRange<int> range);
        int Range(int minInclusive, int maxInclusive);

        float Randomise(float baseValue, float maxChangeByProportionOfBaseValue, ChangeDirection changeDirection);
        TItem RandomItem<TItem>(IList<TItem> items);
    }
}
