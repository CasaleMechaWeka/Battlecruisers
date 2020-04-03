using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds
{
    public interface ICloudNEW
    {
        Vector2 Size { get; }
        Vector2 Position { get; set; }

        void Initialise(ICloudStats cloudStats);
    }
}
