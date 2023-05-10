using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers.Calculators
{
    public interface IPvPCameraCalculatorSettings
    {
        float CruiserWidthMultiplier { get; }
        float CruiserCameraPositionAdjustmentMultiplier { get; }
        IPvPRange<float> ValidOrthographicSizes { get; }

        /// <summary>
        /// The range of possible x values the user could be able to see.  X values
        /// outside this range should never be seen by the user.
        /// </summary>
        IPvPRange<float> CameraVisibleXRange { get; }

        float WaterProportion { get; }
        float MaxWaterPositionY { get; }

        float ScrollSpeedGradient { get; }
        float ScrollSpeedConstant { get; }
        float ScrollSpeed { get; }
    }
}