using UnityEngine;

namespace BattleCruisers.Utils.DataStrctures
{
    public class OrderedRange : Range<float>
    {
        public OrderedRange(float value1, float value2) 
            : base(Mathf.Min(value1, value2), Mathf.Max(value1, value2))
        {
        }
    }
}
