using BattleCruisers.Utils;
using NSubstitute;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.Scenes
{
    public class CreditsSceneGod : MonoBehaviour, IPointerDownHandler
    {
        private ISceneNavigator _sceneNavigator;

        async void Start()
        {
            _sceneNavigator = LandingSceneGod.SceneNavigator;

            if (_sceneNavigator == null)
            {
                _sceneNavigator = Substitute.For<ISceneNavigator>();
            }

            _sceneNavigator.SceneLoaded(SceneNames.CREDITS_SCENE);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _sceneNavigator.GoToScene(SceneNames.SCREENS_SCENE);
        }
    }
}