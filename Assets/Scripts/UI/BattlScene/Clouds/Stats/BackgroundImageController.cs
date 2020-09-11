using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class BackgroundImageController : MonoBehaviour
    {
        public SpriteRenderer renderer;

        public void Initialise(IBackgroundImageStats stats)
        {
            Assert.IsNotNull(stats);
            Assert.IsNotNull(renderer);

            if (stats.Sprite == null)
            {
                gameObject.SetActive(false);
                return;
            }
            
            gameObject.SetActive(true);

            transform.position = stats.Position;
            transform.localScale = new Vector3(stats.Scale.x, stats.Scale.y, 1);
            transform.rotation = Quaternion.Euler(0, 0, stats.ZRotation);

            renderer.sprite = stats.Sprite;
            renderer.color = stats.Colour;
            renderer.flipX = stats.FlipX;
            renderer.flipY = stats.FlipY;
            renderer.sortingOrder = stats.OrderInLayer;
        }
    }
}