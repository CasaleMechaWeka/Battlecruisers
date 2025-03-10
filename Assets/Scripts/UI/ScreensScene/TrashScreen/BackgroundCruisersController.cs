using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
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
            if (ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.PlayerLoadout.SelectedBodykit == -1)
            {
                playerCruiserImage.sprite = playerCruiser.Sprite;
            }
            else
            {
                Bodykit bodykit = ScreensSceneGod.Instance._prefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.PlayerLoadout.SelectedBodykit));
                playerCruiserImage.sprite = bodykit.BodykitImage;
            }

            playerCruiserImage.transform.localPosition = playerCruiser.TrashTalkScreenPosition;

            if (ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.ID_Bodykit_AIbot == -1)
            {
                enemyCruiserImage.sprite = enemyCruiser.Sprite;
            }
            else
            {
                Bodykit bodykit = ScreensSceneGod.Instance._prefabFactory.GetBodykit(StaticPrefabKeys.BodyKits.GetBodykitKey(ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.ID_Bodykit_AIbot));
                enemyCruiserImage.sprite = bodykit.BodykitImage;
            }

            enemyCruiserImage.transform.localPosition = enemyCruiser.TrashTalkScreenPosition * new Vector2(-1, 1);
        }
    }
}