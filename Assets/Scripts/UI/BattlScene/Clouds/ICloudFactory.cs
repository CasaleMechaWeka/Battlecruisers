using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    // FELIX  Remove :D
    public interface ICloudFactory
    {
        ICloud CreateCloud(Vector3 spawnPosition);
		ICloudStatsExtended CreateCloudStats(ICloudGenerationStats generationStats);
    }
}
