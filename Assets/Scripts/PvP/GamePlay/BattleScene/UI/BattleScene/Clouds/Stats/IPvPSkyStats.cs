using BattleCruisers.UI.BattleScene.Clouds.Stats;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Stats
{
    public interface IPvPSkyStats : ICloudStats
    {
        Material SkyMaterial { get; }
        IMoonStats MoonStats { get; }
    }
}
