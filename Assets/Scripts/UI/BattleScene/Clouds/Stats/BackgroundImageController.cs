using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class BackgroundImageController : MonoBehaviour
    {
        public SpriteRenderer background;

        public void Initialise(IBackgroundImageStats stats)
        {
            Assert.IsNotNull(stats);
            Assert.IsNotNull(background);

            if (stats.Sprite == null)
            {
                gameObject.SetActive(false);
                return;
            }
            
            gameObject.SetActive(true);

            transform.position = stats.Position;
            transform.localScale = new Vector3(stats.Scale.x, stats.Scale.y, 1);
            transform.rotation = Quaternion.Euler(0, 0, stats.ZRotation);

            background.sprite = stats.Sprite;
            background.color = stats.Colour;
            background.flipX = stats.FlipX;
            background.flipY = stats.FlipY;
            background.sortingOrder = stats.OrderInLayer;
        }
    }
}