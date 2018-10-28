using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public interface ICameraNavigationWheelCalculator
    {
        float FindOrthographicSize(
            float navigationWheelYPosition,
            float navigationWheelPanelHeight,
            IRange<float> validOrthographicSize);

        // FELIX  Use ICameraCalculator.FindCameraYPosition :)
        float FindCameraYPosition(
            float orthographicSize,
            float cameraHeight,
            float waterSurfaceYPosition);

        float FindCameraXPosition(
            float navigationWheelXPosition,
            float navigationWheelPanelWidth,
            IRange<float> validXPosition);
    }
}