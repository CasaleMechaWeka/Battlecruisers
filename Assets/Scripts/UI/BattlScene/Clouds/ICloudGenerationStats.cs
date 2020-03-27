using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public enum CloudDensity
    {
        Low, Medium, High, VeryHigh
    }

    public enum CloudMovementSpeed
    {
        Slow, Fast
    }

    public interface ICloudGenerationStats
    {
        Rect CloudSpawnArea { get; }
		float CloudDensityAsFraction { get; }
        float CloudHorizontalMovementSpeedInS { get; }
    }
}
