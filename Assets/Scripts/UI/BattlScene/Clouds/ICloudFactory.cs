using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public interface ICloudFactory
    {
        ICloud CreateCloud(Vector3 spawnPosition);
		ICloudStatsExtended CreateCloudStats(ICloudGenerationStats generationStats);
    }
}
