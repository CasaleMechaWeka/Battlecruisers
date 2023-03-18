using BattleCruisers.Utils;
using NSubstitute;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.Scenes
{
    public class CreditsSceneGod : MonoBehaviour, IPointerDownHandler
    {
        private ISceneNavigator _sceneNavigator;

        void Start()
        {
            _sceneNavigator = LandingSceneGod.SceneNavigator;

            if (_sceneNavigator == null)
            {
                _sceneNavigator = Substitute.For<ISceneNavigator>();
            }
            LandingSceneGod.MusicPlayer.PlayCreditsMusic();
            _sceneNavigator.SceneLoaded(SceneNames.CREDITS_SCENE);
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
            _sceneNavigator.GoToScene(SceneNames.SCREENS_SCENE, true);
        }
    
        private void OnEnable()
        {
            LandingSceneGod.SceneNavigator.SceneLoaded(SceneNames.CREDITS_SCENE);
        }
        
    }
}