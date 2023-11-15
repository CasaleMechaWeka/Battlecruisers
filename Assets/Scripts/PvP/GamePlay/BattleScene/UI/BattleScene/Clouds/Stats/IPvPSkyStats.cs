using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Stats
{
    public interface IPvPSkyStats : IPvPCloudStats
    {
        Material SkyMaterial { get; }
        IPvPMoonStats MoonStats { get; }
    }
}
