using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public interface ICloudFactory
    {
        ICloud CreateCloud(Vector2 spawnPosition);
		ICloudStats CreateCloudStats(ICloudGenerationStats generationStats);
    }
}
