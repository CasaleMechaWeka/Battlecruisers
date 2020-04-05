using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public interface ISkyStats : ICloudStats
    {
        Material SkyMaterial { get; }
    }
}
