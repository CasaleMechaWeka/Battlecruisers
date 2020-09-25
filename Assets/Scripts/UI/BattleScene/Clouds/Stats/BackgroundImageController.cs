using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class BackgroundImageController : MonoBehaviour
    {
        public SpriteRenderer background;

        public void Initialise(IBackgroundImageStats stats, float cameraAspectRatio)
        {
            Assert.IsNotNull(stats);
            Assert.IsNotNull(background);

            if (stats.Sprite == null)
            {
                gameObject.SetActive(false);
                return;
            }
            
            gameObject.SetActive(true);

            transform.position = FindPosition(stats, cameraAspectRatio);
            transform.localScale = new Vector3(stats.Scale.x, stats.Scale.y, 1);
            transform.rotation = Quaternion.Euler(0, 0, stats.ZRotation);

            background.sprite = stats.Sprite;
            background.color = stats.Colour;
            background.flipX = stats.FlipX;
            background.flipY = stats.FlipY;
            background.sortingOrder = stats.OrderInLayer;
        }

        // FELIX  Abstract & test?
        public Vector3 FindPosition(IBackgroundImageStats stats, float cameraAspectRatio)
        {
            // FELIX
            return stats.Position;
        }
    }
}