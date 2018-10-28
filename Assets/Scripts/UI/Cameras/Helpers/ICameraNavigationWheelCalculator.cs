using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public interface ICameraNavigationWheelCalculator
    {
        float FindOrthographicSize(
            float navigationWheelYPosition,
            float navigationWheelPanelHeight,
            IRange<float> validOrthographicSize);

        float FindCameraYPosition(
            float orthographicSize,
            float cameraHeight,
            float waterSurfaceYPosition);

        // FELIX
        IRange<float> FindValidNavigationWheelXPositions(
            float navigationWheelYPosition);

        // FELIX
        IRange<float> FindValidCameraXPositions(
            float cameraYPosition);

        float FindCameraXPosition(
            float navigationWheelXPosition,
            float navigationWheelPanelWidth,
            IRange<float> validXPosition);
    }
}