using System.Threading.Tasks;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers.Sprites;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.Clouds.Stats
{
    public class BackgroundImageController : MonoBehaviour
    {

        public SpriteRenderer background;

        public async Task Initialise(BackgroundImageStats stats, float cameraAspectRatio, BackgroundImageCalculator calculator)
        {
            Helper.AssertIsNotNull(stats, calculator);
            Assert.IsNotNull(background);
            Assert.IsTrue(cameraAspectRatio > 0);

            if (stats.SpriteName == null)
            {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            transform.position = calculator.FindPosition(stats, cameraAspectRatio);
            transform.localScale = new Vector3(stats.Scale, stats.Scale, 1);

            background.sprite = await SpriteFetcher.GetSpriteAsync(SpritePaths.BackgroundImagesPath + stats.SpriteName);
            background.color = stats.Colour;
            background.flipX = stats.FlipX;
            background.sortingOrder = stats.OrderInLayer;
        }
    }
}