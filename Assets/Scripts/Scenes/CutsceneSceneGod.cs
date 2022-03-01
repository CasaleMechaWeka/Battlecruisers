using BattleCruisers.Utils;
using NSubstitute;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.Scenes
{
    public class CutsceneSceneGod : MonoBehaviour, IPointerDownHandler
    {
        private ISceneNavigator _sceneNavigator;

        void Start()
        {
            _sceneNavigator = LandingSceneGod.SceneNavigator;

            if (_sceneNavigator == null)
            {
                _sceneNavigator = Substitute.For<ISceneNavigator>();
            }

            LandingSceneGod.MusicPlayer.PlayCutsceneMusic();
            _sceneNavigator.SceneLoaded(SceneNames.CUTSCENE_SCENE);
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
            _sceneNavigator.GoToScene(SceneNames.CREDITS_SCENE, true);
        }
    }
}