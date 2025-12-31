using BattleCruisers.Data;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BattleCruisers.Scenes
{
    public class CutsceneSceneGod : MonoBehaviour, IPointerDownHandler
    {
        private AudioSourceGroup soundEffects;
        public AudioSource[] audioSources;

        void Start()
        {
            AudioSourceBC[] sources = new AudioSourceBC[audioSources.Length];
            for (int i = 0; i < sources.Length; i++)
            {
                sources[i] = new AudioSourceBC(audioSources[i]);
            }
            soundEffects
                = new AudioSourceGroup(
                    DataProvider.SettingsManager,
                    sources);

            LandingSceneGod.MusicPlayer.PlayCutsceneMusic();
            SceneNavigator.SceneLoaded(SceneNames.CUTSCENE_SCENE);
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
            SceneNavigator.GoToScene(SceneNames.CREDITS_SCENE, true);
        }
    }
}