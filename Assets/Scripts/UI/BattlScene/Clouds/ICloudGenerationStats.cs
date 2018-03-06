using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public interface ICloudGenerationStats
    {
        Rect CloudSpawnArea { get; }
		float CloudDensityAsFraction { get; }
        float CloudHorizontalMovementSpeedInS { get; }
    }
}
