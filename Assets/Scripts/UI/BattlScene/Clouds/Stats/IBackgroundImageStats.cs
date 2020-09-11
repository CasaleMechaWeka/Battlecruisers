using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public interface IBackgroundImageStats
    {
        // Transform
        Vector3 Position { get; }
        Vector2 Scale { get; }
        float ZRotation { get; }

        // Sprite
        Sprite Sprite { get; }
        Color Colour { get; }
        bool FlipX { get; }
        bool FlipY { get; }
        int OrderInLayer { get; }
    }
}