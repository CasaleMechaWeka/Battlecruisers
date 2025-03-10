using BattleCruisers.UI.BattleScene.Clouds.Stats;
using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public interface ICloud
    {
        Vector2 Size { get; }
        Vector3 Position { get; set; }

        void Initialise(ICloudStats cloudStats);
    }
}
