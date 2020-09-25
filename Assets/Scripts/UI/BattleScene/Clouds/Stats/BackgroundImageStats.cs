using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class BackgroundImageStats : MonoBehaviour, IBackgroundImageStats
    {
        public Vector2 scale;
        public Vector2 Scale => scale;

        public float zRotation;
        public float ZRotation => zRotation;

        [Header("Position for 4:3 aspect ratio")]
        public Vector3 positionAt4to3;
        public Vector3 PositionAt4to3 => positionAt4to3;

        [Header("Y position for 16:9 aspect ratio")]
        public float yPositionAt16to9;
        public float YPositionAt16to9 => yPositionAt16to9;

        [Header("Sprite (leave empty if no background for this level)")]
        public Sprite sprite;
        public Sprite Sprite => sprite;

        public Color colour;
        public Color Colour => colour;

        public bool flipX = false;
        public bool FlipX => flipX;

        public bool flipY = false;
        public bool FlipY => flipY;

        public int orderInLayer;
        public int OrderInLayer => orderInLayer;
    }
}