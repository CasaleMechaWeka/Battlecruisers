using BattleCruisers.Cruisers;

namespace BattleCruisers.Utils.Cameras
{
    public interface ICameraCalculator
    {
        float FindCameraOrthographicSize(ICruiser cruiser);
        float FindCameraYPosition(float desiredOrthographicSize);
        float FindScrollSpeed(float orthographicSize);
	}
}
