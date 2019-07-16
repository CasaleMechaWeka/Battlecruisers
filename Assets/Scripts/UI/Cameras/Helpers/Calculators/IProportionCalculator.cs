using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.UI.Cameras.Helpers.Calculators
{
    public interface IProportionCalculator
    {
        float FindProportionalValue(float proportion, IRange<float> valueRange);
        float FindProportion(float value, IRange<float> valueRange);
    }
}