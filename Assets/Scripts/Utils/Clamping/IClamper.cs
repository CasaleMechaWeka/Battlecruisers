using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Utils.Clamping
{
    public interface IClamper
    {
        float Clamp(float value, IRange<float> validRange);
    }
}