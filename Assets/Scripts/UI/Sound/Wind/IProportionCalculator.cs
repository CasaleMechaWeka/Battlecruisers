using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.UI.Sound.Wind
{
    public interface IProportionCalculator
    {
        float FindProportion(float value, IRange<float> range);
    }
}