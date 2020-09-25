using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public interface IBackgroundImageStats
    {
        Vector2 Scale { get; }
        float ZRotation { get; }
        Vector3 PositionAt4to3 { get; }
        float YPositionAt16to9 { get; }

        // Sprite
        Sprite Sprite { get; }
        Color Colour { get; }
        bool FlipX { get; }
        bool FlipY { get; }
        int OrderInLayer { get; }
    }
}