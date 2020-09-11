using UnityEngine;

namespace BattleCruisers.UI.BattlScene.Clouds.Stats
{
    public class BackgroundImageStats : MonoBehaviour, IBackgroundImageStats
    {
        [Range(1, 25)]
        public int levelNum = 1;
        public int LevelNum => levelNum;

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

        // FELIX  Move :P
        void Start()
        {
            if (sprite == null)
            {
                return;
            }

            RectTransform t = null;

            t.position = position;
            t.localScale = new Vector3(scale.x, scale.y, 1);
            t.rotation = Quaternion.Euler(0, 0, zRotation);

            SpriteRenderer s = null;

            s.sprite = sprite;
            s.color = colour;
            s.flipX = flipX;
            s.flipY = flipY;
            s.sortingOrder = orderInLayer;
        }
    }
}