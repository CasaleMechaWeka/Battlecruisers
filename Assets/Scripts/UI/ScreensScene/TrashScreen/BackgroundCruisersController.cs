using BattleCruisers.Cruisers;
using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class BackgroundCruisersController : MonoBehaviour
    {
        public Image playerCruiserImage, enemyCruiserImage;

        public void Initialise(ICruiser playerCruiser, ICruiser enemyCruiser)
        {
            Helper.AssertIsNotNull(playerCruiserImage, enemyCruiserImage);
            Helper.AssertIsNotNull(playerCruiser, enemyCruiser);

            playerCruiserImage.sprite = playerCruiser.Sprite;
            playerCruiserImage.transform.localPosition = playerCruiser.TrashTalkScreenPosition;

            enemyCruiserImage.sprite = enemyCruiser.Sprite;
            enemyCruiserImage.transform.localPosition = enemyCruiser.TrashTalkScreenPosition * new Vector2(-1, 1);
        }
    }
}