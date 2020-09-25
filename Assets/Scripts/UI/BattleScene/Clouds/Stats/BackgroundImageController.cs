using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class BackgroundImageController : MonoBehaviour
    {
        public SpriteRenderer background;

        private const float RATIO_4_TO_3 = 1.333f;
        private const float RATIO_16_TO_9 = 1.778f;

        public void Initialise(IBackgroundImageStats stats, float cameraAspectRatio)
        {
            Assert.IsNotNull(stats);
            Assert.IsNotNull(background);
            Assert.IsTrue(cameraAspectRatio > 0);

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
            float deltaY = stats.YPositionAt16to9 - stats.PositionAt4to3.y;
            float deltaX = RATIO_16_TO_9 - RATIO_4_TO_3;
            float gradient = deltaY / deltaX;

            float constant = stats.YPositionAt16to9 - (gradient * RATIO_16_TO_9);

            float yAdjustedPosition = gradient * cameraAspectRatio + constant;

            return
                new Vector3(
                    stats.PositionAt4to3.x,
                    yAdjustedPosition,
                    stats.PositionAt4to3.z);
        }
    }
}