using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.UI.Cameras.Helpers.Calculators
{
    public interface ICameraCalculatorSettings
    {
        float CruiserWidthMultiplier { get; }
        float CruiserCameraPositionAdjustmentMultiplier { get; }
        IRange<float> ValidOrthographicSizes { get; }

        /// <summary>
        /// The range of possible x values the user could be able to see.  X values
        /// outside this range should never be seen by the user.
        /// </summary>
        IRange<float> CameraVisibleXRange { get; }

        float WaterProportion { get; }
        float MaxWaterPositionY { get; }

        float ScrollSpeedGradient { get; }
        float ScrollSpeedConstant { get; }
        float ScrollSpeed { get; }
    }
}