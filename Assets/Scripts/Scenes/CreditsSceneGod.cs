using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.Scenes
{
    public class CreditsSceneGod : MonoBehaviour, IPointerDownHandler
    {

        void Start()
        {
            LandingSceneGod.MusicPlayer.PlayCreditsMusic();
            SceneNavigator.SceneLoaded(SceneNames.CREDITS_SCENE);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Exit();
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape)
                || Input.GetKeyUp(KeyCode.Space)
                || Input.GetKeyUp(KeyCode.Return))
            {
                Exit();
            }
        }

        private void Exit()
        {
            SceneNavigator.GoToScene(SceneNames.SCREENS_SCENE, true);
        }

        /*        private void OnEnable()
                {
                    LandingSceneGod.SceneNavigator.SceneLoaded(SceneNames.CREDITS_SCENE);
                }*/

    }
}