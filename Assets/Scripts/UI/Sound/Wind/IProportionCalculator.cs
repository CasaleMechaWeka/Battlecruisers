using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.UI.Sound.Wind
{
    public interface IProportionCalculator
    {
        float FindProprtion(float value, IRange<float> range);
    }
}