using UnityEngine;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class BackgroundImageStats : MonoBehaviour, IBackgroundImageStats
    {
        [Header("Transform")]
        public Vector3 position;
        public Vector3 Position => position;

        public Vector2 scale;
        public Vector2 Scale => scale;

        public float zRotation;
        public float ZRotation => zRotation;

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