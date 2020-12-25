using BattleCruisers.Utils;
using NSubstitute;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.Scenes
{
    public class CreditsSceneGod : MonoBehaviour, IPointerDownHandler
    {
        async void Start()
        {
            ISceneNavigator sceneNavigator = LandingSceneGod.SceneNavigator;

            if (sceneNavigator == null)
            {
                sceneNavigator = Substitute.For<ISceneNavigator>();
            }

            sceneNavigator.SceneLoaded(SceneNames.CREDITS_SCENE);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Yo");
        }
    }
}