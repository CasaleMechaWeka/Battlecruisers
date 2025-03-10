namespace BattleCruisers.UI.Sound.Wind
{
    public interface IVolumeCalculator
    {
        float FindVolume(float cameraOrthographicSize);
    }
}