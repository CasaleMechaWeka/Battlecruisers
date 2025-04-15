using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.UI;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;

namespace BattleCruisers.Scenes
{
    public class InterstitialSceneGod : MonoBehaviour
    {
        public CanvasGroupButton nextButton;
        [SerializeField]
        private AudioSource _uiAudioSource;
        private ISingleSoundPlayer _soundPlayer;
        public GameObject[] screens;
        [Header("The number of the screen that comes up when running this scene.")]
        public int TestingScreen = 1;
        public GameObject musicePlayerForTesting;
        void Start()
        {

            GameModel gm = DataProvider.GameModel;

            musicePlayerForTesting.SetActive(false);
            LandingSceneGod.MusicPlayer.PlayCutsceneMusic();
            for (int i = 0; i < LevelStages.STAGE_STARTS.Length; i++)
            {
                screens[i].SetActive(false);
                if (gm.SelectedLevel - 1 == LevelStages.STAGE_STARTS[i])
                    screens[i].SetActive(true);
            }

            _soundPlayer
                = new SingleSoundPlayer(
                    new EffectVolumeAudioSource(
                        new AudioSourceBC(_uiAudioSource), 1));


            nextButton.Initialise(_soundPlayer, Done);
            SceneNavigator.SceneLoaded(SceneNames.STAGE_INTERSTITIAL_SCENE);
        }

        /*        private void OnEnable()
                {
                    LandingSceneGod.SceneNavigator.SceneLoaded(SceneNames.STAGE_INTERSTITIAL_SCENE);
                }*/

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Return))
                Done();
        }

        private void Done()
        {
            SceneNavigator.GoToScene(SceneNames.SCREENS_SCENE, true);
        }
    }
}
