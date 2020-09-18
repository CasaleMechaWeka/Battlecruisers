using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BattleCruisers.UI.ScreensScene.TrashScreen
{
    public class BackgroundCruisersController : MonoBehaviour
    {
        public Image playerCruiserImage, enemyCruiserImage;

        public void Initialise(Sprite playerCruiser, Sprite enemyCruiser)
        {
            Helper.AssertIsNotNull(playerCruiserImage, enemyCruiserImage);
            Helper.AssertIsNotNull(playerCruiser, enemyCruiser);

            playerCruiserImage.sprite = playerCruiser;
            enemyCruiserImage.sprite = enemyCruiser;
        }
    }
}