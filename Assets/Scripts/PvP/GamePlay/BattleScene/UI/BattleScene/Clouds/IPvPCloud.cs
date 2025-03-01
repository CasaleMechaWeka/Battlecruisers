using BattleCruisers.UI.BattleScene.Clouds.Stats;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds
{
    public interface IPvPCloud
    {
        Vector2 Size { get; }
        Vector3 Position { get; set; }

        void Initialise(ICloudStats cloudStats);
    }
}
