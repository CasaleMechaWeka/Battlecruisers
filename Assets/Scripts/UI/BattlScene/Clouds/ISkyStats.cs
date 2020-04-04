using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public interface ISkyStats : ICloudStats
    {
        Material SkyMaterial { get; }
    }
}
