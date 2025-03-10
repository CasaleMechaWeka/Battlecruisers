using BattleCruisers.Data;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.Scenes
{
    public class CutsceneSceneGod : MonoBehaviour, IPointerDownHandler
    {
        private ISceneNavigator _sceneNavigator;
        private AudioSourceGroup soundEffects;
        public AudioSource[] audioSources;

        void Start()
        {
            _sceneNavigator = LandingSceneGod.SceneNavigator;

            if (_sceneNavigator == null)
            {
                _sceneNavigator = Substitute.For<ISceneNavigator>();
            }

            AudioSourceBC[] sources = new AudioSourceBC[audioSources.Length];
            for (int i = 0; i < sources.Length; i++)
            {
                sources[i] = new AudioSourceBC(audioSources[i]);
            }
            soundEffects
                = new AudioSourceGroup(
                    ApplicationModelProvider.ApplicationModel.DataProvider.SettingsManager,
                    sources);

            LandingSceneGod.MusicPlayer.PlayCutsceneMusic();
            _sceneNavigator.SceneLoaded(SceneNames.CUTSCENE_SCENE);
        }

/*        private void OnEnable()
        {
            LandingSceneGod.SceneNavigator.SceneLoaded(SceneNames.CUTSCENE_SCENE);
        }*/

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