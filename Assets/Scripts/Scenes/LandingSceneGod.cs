using BattleCruisers.Data;
using BattleCruisers.UI.Loading;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.Analytics;
using Unity.Services.Authentication;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.UI;
using UnityEngine.Networking;
using Unity.Services.Core;
using System.Net;


#if UNITY_EDITOR
using System.Security.Cryptography;
#endif

namespace BattleCruisers.Scenes
{

    public class LandingSceneGod : MonoBehaviour, ISceneNavigator
    {
        private string _lastSceneLoaded;
        private IHintProvider _hintProvider;

        [Header("For testing")]
        public bool testCreditsScene = false;

        [Header("For testing")]
        public bool testCutScene = false;

        public static ISceneNavigator SceneNavigator { get; private set; }
        public static IMusicPlayer MusicPlayer { get; private set; }
        public static string LoadingScreenHint { get; private set; }

        public GameObject landingCanvas;

        public GameObject loginPanel, retryPanel;

        public GameObject logos;
        public CanvasGroupButton googleBtn, guestBtn;
        public GameObject spinGoogle, spinGuest;
        public GameObject labelGoogle, labelGuest;

        public GameObject quitBtn, retryBtn;
        public GameObject spinRetry;
        public GameObject labelRetry;

        public const string AuthProfileCommandLineArg = "-AuthProfile";
        public ILocTable commonStrings;

        public static LandingSceneGod Instance;
        public LoginType loginType = LoginType.None;

        public ErrorMessageHandler messageHandler;



        async void Start()
        {
            Helper.AssertIsNotNull(landingCanvas, loginPanel, retryPanel, logos, googleBtn, guestBtn, quitBtn, retryBtn);
            Helper.AssertIsNotNull(spinGoogle, spinGuest, spinRetry);
            Helper.AssertIsNotNull(labelGoogle, labelGuest, labelRetry);
            Helper.AssertIsNotNull(messageHandler);
            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;

            if (Instance == null)
                Instance = this;

            ISoundFetcher soundFetcher = new SoundFetcher();
            AudioSource platformAudioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(platformAudioSource);
            IAudioSource audioSource
                = new MusicVolumeAudioSource(
                    new AudioSourceBC(platformAudioSource),
                    applicationModel.DataProvider.SettingsManager);

            ISingleSoundPlayer soundPlayer = new SingleSoundPlayer(
                new SoundFetcher(),
                audioSource
                );

            googleBtn.Initialise(soundPlayer, GoogleLogin);
            guestBtn.Initialise(soundPlayer, AnonymousLogin);


            landingCanvas.SetActive(true);
            loginPanel.SetActive(true);

            spinGuest.SetActive(false);
            spinGoogle.SetActive(false);

            labelGoogle.SetActive(true);
            labelGuest.SetActive(true);

            googleBtn.gameObject.SetActive(false);
            guestBtn.gameObject.SetActive(false);

            retryPanel.SetActive(false);
            labelRetry.SetActive(true);
            spinRetry.SetActive(false);

            googleBtn.Initialise();

            try
            {
                var options = new InitializationOptions();
                options.SetEnvironmentName("production");

                var profile = GetProfile();

                if (profile.Length > 0)
                {
                    try
                    {
                        /*{LocalProfileTool.LocalProfileSuffix}*/
                        options.SetProfile($"{profile}");
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.ToString());
                    }
                }

                await UnityServices.InitializeAsync(options);

#if UNITY_EDITOR
                if (ParrelSync.ClonesManager.IsClone())
                {
                    // When using a ParrelSync clone, switch to a different authentication profile to force the clone
                    // to sign in as a different anonymous user account.
                    string customArgument = ParrelSync.ClonesManager.GetArgument();
                    AuthenticationService.Instance.SwitchProfile($"Clone_{customArgument}_Profile");
                }
#endif

                // initiailise google login 
                InitializePlayGamesLogin();

                if (HasConnection())
                {
                    List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
                }

            }
            catch (ConsentCheckException e)
            {
                // do nothing
                Debug.Log(e.Message);
                // messageHandler.ShowMessage("Please check Internet connection!");
            }

            IDataProvider dataProvider = applicationModel.DataProvider;
            MusicPlayer = CreateMusicPlayer(dataProvider);

            DontDestroyOnLoad(gameObject);
            SceneNavigator = this;
            commonStrings = await LocTableFactory.Instance.LoadCommonTableAsync();

            HintProviders hintProviders = new HintProviders(RandomGenerator.Instance, commonStrings);
            _hintProvider = new CompositeHintProvider(hintProviders.BasicHints, hintProviders.AdvancedHints, dataProvider.GameModel, RandomGenerator.Instance);


            //below is code to localise the logo
            string locName = LocalizationSettings.SelectedLocale.name;
            Transform[] ts = logos.GetComponentsInChildren<Transform>(includeInactive: true);
            foreach (Transform t in ts)
            {
                if (t.gameObject.name == locName)
                {
                    t.gameObject.SetActive(true);
                    break;
                }
            }


            // add event handlers to authentication
            AuthenticationService.Instance.SignedIn += SignedIn;
            AuthenticationService.Instance.SignedOut += SignedOut;
            AuthenticationService.Instance.Expired += Expired;
            AuthenticationService.Instance.SignInFailed += SignFailed;

            // should be enabled after completion initialization
            googleBtn.gameObject.SetActive(true);
            guestBtn.gameObject.SetActive(true);
        }

        void SetInteractable(bool interactable)
        {
            googleBtn.GetComponent<CanvasGroupButton>().enabled = interactable;
            guestBtn.GetComponent<CanvasGroupButton>().enabled = interactable;
        }

        void SetSpin(GameObject obj, bool spinable)
        {
            obj.SetActive(spinable);
        }
        private string GetProfile()
        {
            var arguments = Environment.GetCommandLineArgs();
            for (int i = 0; i < arguments.Length; i++)
            {
                if (arguments[i] == AuthProfileCommandLineArg)
                {
                    var profileId = arguments[i + 1];
                    return profileId;
                }
            }

#if UNITY_EDITOR

            // When running in the Editor make a unique ID from the Application.dataPath.
            // This will work for cloning projects manually, or with Virtual Projects.
            // Since only a single instance of the Editor can be open for a specific
            // dataPath, uniqueness is ensured.
            var hashedBytes = new MD5CryptoServiceProvider()
                .ComputeHash(Encoding.UTF8.GetBytes(Application.dataPath));
            Array.Resize(ref hashedBytes, 16);
            return new Guid(hashedBytes).ToString("N").Length > 30 ? new Guid(hashedBytes).ToString("N").Substring(0, 30) : new Guid(hashedBytes).ToString("N");
#else
            return "";
#endif
        }

        public void GoogleLogin()
        {
            Debug.Log("===> trying to login with Google");
            loginType = LoginType.Google;
            LoginGoogle();
        }

        public async void AnonymousLogin()
        {
            if (HasConnection())
            {
                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    SetInteractable(false);
                    SetSpin(spinGuest, true);
                    labelGuest.SetActive(false);
                    loginType = LoginType.Anonymous;
                    try
                    {
                        await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);
                        //    messageHandler.ShowMessage("Please check Internet connection!");
                    }
                }
            }
            else
            {
                // play without Internet
                landingCanvas.SetActive(false);
                loginPanel.SetActive(false);
                GoToScene(SceneNames.SCREENS_SCENE, true);
            }
        }

        private void SignFailed(RequestFailedException exception)
        {
            SetInteractable(true);
            SetSpin(spinGuest, false);
            SetSpin(spinGoogle, false);
            labelGoogle.SetActive(true);
            labelGuest.SetActive(true);
            loginType = LoginType.None;
        }

        void SignedIn()
        {
            landingCanvas.SetActive(false);
            loginPanel.SetActive(false);
            GoToScene(SceneNames.SCREENS_SCENE, true);
        }

        void SignedOut()
        {
            Debug.Log("===> You're signed Out to UGS!!!");
        }
        void Expired()
        {
            Debug.Log("===> You're Expired from UGS!!!");
        }

        private IMusicPlayer CreateMusicPlayer(IDataProvider dataProvider)
        {
            AudioSource platformAudioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(platformAudioSource);
            IAudioSource audioSource
                = new MusicVolumeAudioSource(
                    new AudioSourceBC(platformAudioSource),
                    dataProvider.SettingsManager);

            return
                new MusicPlayer(
                    new SingleSoundPlayer(
                        new SoundFetcher(),
                        audioSource));
        }

        public void GoToScene(string sceneName, bool stopMusic)
        {
            string hint = null;
            if (sceneName == SceneNames.BATTLE_SCENE
                && !ApplicationModelProvider.ApplicationModel.IsTutorial)
            {
                hint = _hintProvider.GetHint();
            }
            if (sceneName == SceneNames.PvP_BOOT_SCENE && !ApplicationModelProvider.ApplicationModel.IsTutorial)
            {
                // should be replace with PvP
                hint = _hintProvider.GetHint();
            }

            LoadingScreenHint = hint;

            if (MusicPlayer != null && stopMusic)
                MusicPlayer.Stop();

            StartCoroutine(LoadSceneWithLoadingScreen(sceneName));
        }

        private IEnumerator LoadSceneWithLoadingScreen(string sceneName)
        {
            Logging.LogMethod(Tags.SCENE_NAVIGATION);

            _lastSceneLoaded = null;
            if (sceneName == SceneNames.PvP_BOOT_SCENE)
            {
                yield return LoadScene(SceneNames.PvP_INITIALIZE_SCENE, LoadSceneMode.Single);
            }
            else
            {
                yield return LoadScene(SceneNames.LOADING_SCENE, LoadSceneMode.Single);
            }


            yield return LoadScene(sceneName, LoadSceneMode.Additive);

            Logging.Log(Tags.SCENE_NAVIGATION, "Wait for my custom setup for:  " + sceneName);

            while (_lastSceneLoaded != sceneName)
            {
                float waitIntervalInS = 0.1f;
                Logging.Verbose(Tags.SCENE_NAVIGATION, $"Loading {sceneName}  waiting another: {waitIntervalInS}s");
                yield return new WaitForSeconds(waitIntervalInS);
            }
            Logging.Log(Tags.SCENE_NAVIGATION, "Finished loading:  " + sceneName);

            // Hide loading scene.  Don't unload, because that destroys all prefabs that have been loaded :P

            if (sceneName == SceneNames.PvP_BOOT_SCENE)
            {
                MatchmakingScreenController.Instance.Destroy();
            }
            else
            {
                LoadingScreenController.Instance.Destroy();
            }
        }

        private IEnumerator LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        {
            Logging.Log(Tags.SCENE_NAVIGATION, "Start loading:  " + sceneName);
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);

            while (!loadOperation.isDone)
            {
                Logging.Verbose(Tags.SCENE_NAVIGATION, $"Loading {sceneName}  progress: {loadOperation.progress}");
                yield return null;
            }

            Logging.Log(Tags.SCENE_NAVIGATION, "Finished loading:  " + sceneName);
        }

        public void SceneLoaded(string sceneName)
        {
            Logging.Log(Tags.SCENE_NAVIGATION, sceneName);
            _lastSceneLoaded = sceneName;
        }


        // commented by Sava, not sure why this code block here

        /*        void update()
                {
                    transform.localPosition = Camera.main.gameObject.transform.localPosition;
                    Debug.Log("you are calling me here!!!");
                }*/


        // Google Auth
        void InitializePlayGamesLogin()
        {
            var config = new PlayGamesClientConfiguration.Builder()
                .RequestIdToken()
                .Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
        }

        void LoginGoogle()
        {
            SetInteractable(false);
            SetSpin(spinGoogle, true);
            labelGoogle.SetActive(false);
            Social.localUser.Authenticate(OnGoogleLogin);
        }

        async void OnGoogleLogin(bool success)
        {
            if (success)
            {
                Debug.Log("Login with Google Done, IdToken: " + ((PlayGamesLocalUser)Social.localUser).GetIdToken());
                await SignInWithGoogleAsync(((PlayGamesLocalUser)Social.localUser).GetIdToken());
            }
            else
            {
                Debug.Log("Unsuccessful login");
                SetInteractable(true);
                SetSpin(spinGoogle, false);
                labelGoogle.SetActive(true);
            }
        }

        public void OnRetry()
        {
            if (loginType == LoginType.Anonymous)
                AnonymousLogin();
            if (loginType == LoginType.Google)
                GoogleLogin();
        }

        public void OnQuit()
        {
            Application.Quit();
        }

        async Task SignInWithGoogleAsync(string idToken)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithGoogleAsync(idToken);
                Debug.Log("SignIn is successful.");
            }
            catch (Unity.Services.Authentication.AuthenticationException ex)
            {
                Debug.LogException(ex);
            }
            catch (RequestFailedException ex)
            {
                Debug.LogException(ex);
            }
        }

        void OnDestroy()
        {
            AuthenticationService.Instance.SignOut();
            AuthenticationService.Instance.SignedIn -= SignedIn;
            AuthenticationService.Instance.SignedOut -= SignedOut;
            AuthenticationService.Instance.SignInFailed -= SignFailed;
            AuthenticationService.Instance.Expired -= Expired;
        }

        public static bool HasConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = new WebClient().OpenRead("https://unity.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public enum LoginType { Google, Apple, Anonymous, None }
    }


}
