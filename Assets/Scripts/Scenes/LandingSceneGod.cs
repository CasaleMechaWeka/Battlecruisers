using BattleCruisers.Data;
using BattleCruisers.UI.Loading;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.UI;
using Unity.Services.Core;
using System.Net;
using BattleCruisers.Utils.Fetchers.Cache;
using UnityEngine.UI;
using TMPro;

#if PLATFORM_ANDROID
using GooglePlayGames.BasicApi;
using BattleCruisers.Utils.Network;
#endif

#if PLATFORM_IOS
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Extensions;
using AppleAuth.Interfaces;
using AppleAuth.Native;
#endif
using BattleCruisers.UI.ScreensScene.BattleHubScreen;

using System.Security.Cryptography;

namespace BattleCruisers.Scenes
{
    public class LandingSceneGod : MonoBehaviour, ISceneNavigator
    {
        public Text onscreenLogging;

        private string _lastSceneLoaded;
        private IHintProvider _hintProvider;

        [Header("For testing")]
        public bool testCreditsScene = false;
        public bool displayOnscreenLogs;

        [Header("For testing")]
        public bool testCutScene = false;

        public static ISceneNavigator SceneNavigator { get; private set; }
        public static IMusicPlayer MusicPlayer { get; private set; }

        public ISingleSoundPlayer soundPlayer;
        public static string LoadingScreenHint { get; private set; }

        public GameObject landingCanvas;

        public GameObject loginPanel;

        public GameObject logos;
        public CanvasGroupButton appleBtn, googleBtn, guestBtn;
        public GameObject spinApple, spinGoogle, spinGuest;
        public GameObject labelApple, labelGoogle, labelGuest;

        public const string AuthProfileCommandLineArg = "-AuthProfile";

        public static LandingSceneGod Instance;

        public ErrorMessageHandler messageHandler;
        public bool HasInternetConnection { get; private set; }

#if PLATFORM_ANDROID
        public static GoogleAuthentication _GoogleAuthentication { get; set; }
#endif
#if PLATFORM_IOS
        public IAppleAuthManager _AppleAuthManager;
        private const string AppleUserIdKey = "AppleUserId";
        private const string AppleUserToken = "AppleUserToken";
#endif

        public int coinBattleLevelNum = -1;

        public MessageBox messagebox;

        public List<GameObject> disableOnSceneTransition;
        public void LogToScreen(string log)
        {
            if (displayOnscreenLogs)
            {
                onscreenLogging.text = log;
            }
        }

        async void Start()
        {
#if PLATFORM_ANDROID
            Application.targetFrameRate = 60;
#endif

            if (Instance == null)
            {
                Instance = this;
            }

            LogToScreen(Application.platform.ToString());
            messagebox.HideMessage();
            Helper.AssertIsNotNull(landingCanvas, loginPanel, logos, googleBtn, guestBtn);
            Helper.AssertIsNotNull(spinGoogle, spinGuest);
            Helper.AssertIsNotNull(labelGoogle, labelGuest);
            Helper.AssertIsNotNull(messageHandler);
            LogToScreen("Starting Battlecruisers"); // SCREEN START

            //loading loc tables in parallel is about 40-100% faster
            //Starting these tasks here saves ~100 ms avg
            _ = LocTableCache.LoadTableAsync(TableName.COMMON);
            _ = LocTableCache.LoadTableAsync(TableName.HECKLES);
            _ = LocTableCache.LoadTableAsync(TableName.SCREENS_SCENE);

            _ = PrefabCache.CreatePrefabCacheAsync();       //starting this here instead of in ScreensSceneGod saves ~2s

            HasInternetConnection = await CheckForInternetConnection();

            AudioSource platformAudioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(platformAudioSource);
            IAudioSource audioSource
                = new MusicVolumeAudioSource(
                    new AudioSourceBC(platformAudioSource),
                    DataProvider.SettingsManager);

            soundPlayer = new SingleSoundPlayer(audioSource);

            try
            {
                var options = new InitializationOptions();
                options.SetEnvironmentName("production");
                //options.SetEnvironmentName("dev");
                string profile = GetProfile();
                if (profile.Length > 0)
                {
                    try
                    {
                        /*{LocalProfileTool.LocalProfileSuffix}*/
                        options.SetProfile(profile);
                    }
                    catch (Exception e)
                    {
                        Debug.Log(e.ToString());
                    }
                }

                if (HasInternetConnection)
                {
                    await UnityServices.InitializeAsync(options);
                }
#if UNITY_EDITOR
                if (ParrelSync.ClonesManager.IsClone())
                {
                    // When using a ParrelSync clone, switch to a different authentication profile to force the clone
                    // to sign in as a different anonymous user account.
                    string customArgument = ParrelSync.ClonesManager.GetArgument();
                    AuthenticationService.Instance.SwitchProfile($"Clone_{customArgument}_Profile");
                }
#endif
            }
            catch (Exception e)
            {
                // do nothing
                Debug.Log(e.Message);
                // messageHandler.ShowMessage("Please check Internet connection!");
            }

            messagebox.Initialize(soundPlayer);
            MusicPlayer = CreateMusicPlayer();
            DontDestroyOnLoad(gameObject);
            SceneNavigator = this;

            HintProviders hintProviders = new HintProviders();
            _hintProvider = new CompositeHintProvider(hintProviders.BasicHints, hintProviders.AdvancedHints, DataProvider.GameModel);

            try
            {
                // add event handlers to authentication
                AuthenticationService.Instance.SignedIn += SignedIn;
                AuthenticationService.Instance.SignedOut += SignedOut;
                AuthenticationService.Instance.Expired += Expired;
                AuthenticationService.Instance.SignInFailed += SignFailed;
            }
            catch
            {
                LogToScreen("Auth events failed the register"); // ONLINE SERVICES UNAVAILABLE
                Debug.LogError("Auth events failed the register");
            }

            if (HasInternetConnection)
            {
#if PLATFORM_ANDROID
                _GoogleAuthentication = new GoogleAuthentication();
                _GoogleAuthentication.InitializePlayGamesLogin();
                //await GoogleAttemptSilentSigningAsync(soundPlayer);
                ShowSignInScreen();

#elif PLATFORM_IOS
                InitializeAppleAuth();

                // If at any point we receive a credentials revoked notification, we delete the stored User ID
                _AppleAuthManager.SetCredentialsRevokedCallback(result =>
                {
                    Debug.Log("Received revoked callback " + result);
                    PlayerPrefs.DeleteKey(AppleUserIdKey);
                    PlayerPrefs.DeleteKey(AppleUserToken);
                });

                // If we have an Apple User Id available, get the credential status for it
                if (PlayerPrefs.HasKey(AppleUserIdKey))
                {
                    var storedAppleUserId = PlayerPrefs.GetString(AppleUserIdKey);
                    CheckCredentialStatusForUserId(storedAppleUserId);
                }
                // If we do not have an stored Apple User Id, attempt a quick login
                else
                {
                    //Attempt Apple Quick Login
                    AppleQuickLogin();
                }
#else
                ShowSignInScreen();
#endif
            }
            else
            {
                ShowSignInScreen();
                LogToScreen("No internet, continue offline"); // NO INTERNET
            }
        }

        private void ShowSignInScreen()
        {
            landingCanvas.SetActive(true);
            loginPanel.SetActive(true);

            spinGuest.SetActive(false);
            spinApple.SetActive(false);
            spinGoogle.SetActive(false);

            labelGoogle.SetActive(true);
            labelApple.SetActive(true);
            labelGuest.SetActive(true);

            googleBtn.gameObject.SetActive(false);
            appleBtn.gameObject.SetActive(false);
            guestBtn.gameObject.SetActive(false);

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

            guestBtn.Initialise(soundPlayer, AnonymousLogin);
            guestBtn.gameObject.SetActive(true);
            // we probably want to await the task to finish for performance reasons
            guestBtn.GetComponentInChildren<TMP_Text>().text = LocTableCache.ScreensSceneTable.GetString("UI/HomeScreen/PlayButton");

            if (HasInternetConnection)
            {
#if PLATFORM_IOS
                appleBtn.Initialise(soundPlayer, AppleLogin);
                appleBtn.gameObject.SetActive(true);
#elif PLATFORM_ANDROID
                googleBtn.Initialise(soundPlayer, GoogleLogin);
                googleBtn.gameObject.SetActive(true);
#elif PLATFORM_STANDALONE_WIN
                AnonymousLogin();
#endif
            }
            else
            {
                AnonymousLogin();
            }

            LogToScreen("All assets loaded"); // ALL ASSETS LOADED
        }

        private void InitializeAppleAuth()
        {
#if PLATFORM_IOS
            var deserializer = new PayloadDeserializer();
            _AppleAuthManager = new AppleAuthManager(deserializer);
            Debug.Log("Apple Auth Initialized.");
#endif
        }

        void SetInteractable(bool interactable)
        {
            googleBtn.GetComponent<CanvasGroupButton>().enabled = interactable;
            appleBtn.GetComponent<CanvasGroupButton>().enabled = interactable;
            guestBtn.GetComponent<CanvasGroupButton>().enabled = interactable;
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
#elif PLATFORM_ANDROID || PLATFORM_IOS || PLATFORM_STANDALONE_WIN
            return SystemInfo.deviceUniqueIdentifier.Length > 30 ? SystemInfo.deviceUniqueIdentifier.Substring(0, 30) : SystemInfo.deviceUniqueIdentifier;
#endif
        }

#if PLATFORM_ANDROID
        // Google login by button:
        public async void GoogleLogin()
        {
            LogToScreen("Attempting login with Google"); // ON GOOGLE BUTTON PRESS
            if (AuthenticationService.Instance.IsSignedIn)  // already signed in
                return;

            SetInteractable(false);
            labelGoogle.SetActive(false);
            spinGoogle.SetActive(true);

            try
            {
                bool authSuccessful = await _GoogleAuthentication.Authenticate(SignInInteractivity.CanPromptAlways); // The comments for these enums are actually pretty good!
                if (authSuccessful)
                    return;
            }
            catch (Exception ex)
            {
                LogToScreen("Error while trying to log in with Google"); // IF GOOGLE AUTH FAILS FOR ANY REASON
                Debug.Log(ex.Message);
            }

            labelGoogle.SetActive(true);
            spinGoogle.SetActive(false);
            SetInteractable(true);
        }
#endif

#if PLATFORM_IOS
        // Apple login by button:
        private async void AppleLogin()
        {
            LogToScreen("Attempting login with Apple"); // ON APPLE BUTTON PRESS
            if (AuthenticationService.Instance.IsSignedIn)  // already signed in
                return;

            SetInteractable(false);
            spinApple.SetActive(true);
            labelApple.SetActive(false);
            loginType = LoginType.Apple;

            try
            {
                // Initialize the Apple Auth Manager
                if (_AppleAuthManager == null)
                    InitializeAppleAuth();

                // Set the login arguments
                var loginArgs = new AppleAuthLoginArgs(LoginOptions.None);
                Debug.Log("####### AppleLogin Args assigned.");

                // Perform the login
                _AppleAuthManager.LoginWithAppleId(
                    loginArgs,
                    credential =>
                    {
                        var appleIDCredential = credential as IAppleIDCredential;
                        if (appleIDCredential != null)
                        {
                            var idToken = Encoding.UTF8.GetString(
                                appleIDCredential.IdentityToken,
                                0,
                                appleIDCredential.IdentityToken.Length);
                            Debug.Log("Sign-in with Apple successfully done. IDToken: " + idToken);
                            LogToScreen("Sign-in success."); //Localise for prod
                            PlayerPrefs.SetString(AppleUserIdKey, credential.User);
                            PlayerPrefs.SetString(AppleUserToken, idToken);
                            PlayerPrefs.Save();
                            HandleAppleSignIn(appleIDCredential);
                            return;
                        }
                        else
                        {
                            Debug.Log("Sign-in with Apple error. Message: appleIDCredential is null");
                            LogToScreen("Retrieving Apple Id Token failed."); //Localise for prod
                        }
                    },
                    error =>
                    {
                        Debug.Log("Sign-in with Apple error. Message: " + error.ToString());
                        LogToScreen("Login Unsuccessful: " + error.ToString()); //Localise for prod
                    }
                );
            }
            catch (Exception ex)
            {
                LogToScreen("Login Exception: " + ex.Message); //Localise for prod
                Debug.Log(ex.Message);
            }

            spinApple.SetActive(false);
            labelApple.SetActive(true);
            SetInteractable(true);
        }

        // Attempt Apple signin without user input:
        private void AppleQuickLogin()
        {
            var quickLoginArgs = new AppleAuthQuickLoginArgs();
            Debug.Log("####### QuickLogin Args Set.");

            // Quick login should succeed if the credential was authorized before and not revoked
            try
            {
                // Initialize the Apple Auth Manager
                if (_AppleAuthManager == null)
                    InitializeAppleAuth();

                _AppleAuthManager.QuickLogin(
                    quickLoginArgs,
                    credential =>
                    {
                        // If it's an Apple credential, save the user ID, for later logins
                        var appleIDCredential = credential as IAppleIDCredential;
                        if (appleIDCredential != null)
                        {
                            var idToken = Encoding.UTF8.GetString(
                                    appleIDCredential.IdentityToken,
                                    0,
                                    appleIDCredential.IdentityToken.Length);
                            PlayerPrefs.SetString(AppleUserIdKey, credential.User);
                            PlayerPrefs.SetString(AppleUserToken, idToken);
                            PlayerPrefs.Save();
                            HandleAppleSignIn(appleIDCredential);
                        }
                    },
                    error =>
                    {
                        // If Quick Login fails, we should show the normal sign in with apple menu, to allow for a normal Sign In with apple
                        var authorizationErrorCode = error.GetAuthorizationErrorCode();
                        ShowSignInScreen();
                    });
            }
            catch (Exception ex)
            {
                Debug.Log("Apple Quick Login failed, Error: " + ex.Message);
                ShowSignInScreen();
            }
        }

        // Used by QuickLogin to process the AppleIDCredential
        // Separate from SignInWithAppleAsync() to support Apple's different login flows
        private async void HandleAppleSignIn(IAppleIDCredential credential)
        {
            try
            {
                var idToken = Encoding.UTF8.GetString(credential.IdentityToken);
                await SignInWithAppleAsync(idToken);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error while processing Apple Sign-in: " + ex.Message);
                ShowSignInScreen();
            }
        }

        // Sign in a returning player or create new player
        // Does the Unity AuthenticationService part of sign-in.
        private async Task SignInWithAppleAsync(string idToken)
        {
            try
            {
                await AuthenticationService.Instance.SignInWithAppleAsync(idToken);
                Debug.Log("####### Sign-in was successful.");
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogError("####### Authentication Error: " + ex.Message);
                ShowSignInScreen();
            }
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogError("####### Request Error: " + ex.Message);
                ShowSignInScreen();
            }
        }

        // Apple-specific ID check
        private void CheckCredentialStatusForUserId(string appleUserId)
        {
            // If there is an apple ID available, we should check the credential state
            _AppleAuthManager.GetCredentialState(
            appleUserId,
            state =>
            {
                switch (state)
                {
                    // If it's authorized, login with that user id
                    case CredentialState.Authorized:
                        Debug.Log("####### Credential found.");
                        if (PlayerPrefs.HasKey(AppleUserToken))
                        {
                            var storedAppleUserToken = PlayerPrefs.GetString(AppleUserToken);
                            SignInWithAppleAsync(storedAppleUserToken);
                        }
                        else
                        {
                            ShowSignInScreen();
                        }
                        return;
                    // If it was revoked, or not found, we need a new sign in with apple attempt
                    // Discard previous apple user id
                    case CredentialState.Revoked:
                    case CredentialState.NotFound:
                        Debug.Log("####### Credential not found or revoked.");
                        PlayerPrefs.DeleteKey(AppleUserIdKey);
                        PlayerPrefs.DeleteKey(AppleUserToken);
                        ShowSignInScreen();
                        return;
                }
            },
            error =>
            {
                var authorizationErrorCode = error.GetAuthorizationErrorCode();
                Debug.LogWarning("Error while trying to get credential state " + authorizationErrorCode.ToString() + " " + error.ToString());
                ShowSignInScreen();
            });
        }
#endif

        // Guest login by button:
        public async void AnonymousLogin()
        {

            if (!(HasInternetConnection && !AuthenticationService.Instance.IsSignedIn))
                return;

            SetInteractable(false);
            labelGuest.SetActive(false);
            spinGuest.SetActive(true);

            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                return;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);

                // This only happens if UGS fails (boooooo)
                LogToScreen("Unity Game Services could not be reached"); // IF UNITY GAME SERVICES FAILS FOR ANY REASON
            }

            // play without Internet
            loginPanel.SetActive(false);
            labelGuest.SetActive(true);
            spinGuest.SetActive(false);
            foreach (GameObject i in disableOnSceneTransition)
                i.SetActive(false);
            GoToScene(SceneNames.SCREENS_SCENE, true);
        }

        private void SignFailed(RequestFailedException exception)
        {
            SetInteractable(true);
            spinGuest.SetActive(false);
            spinApple.SetActive(false);
            spinGoogle.SetActive(false);
            labelGoogle.SetActive(true);
            labelApple.SetActive(true);
            labelGuest.SetActive(true);
        }

        void SignedIn()
        {
            SetInteractable(true);
            loginPanel.SetActive(false);
            spinGuest.SetActive(false);
            spinApple.SetActive(false);
            spinGoogle.SetActive(false);
            labelGoogle.SetActive(true);
            labelApple.SetActive(true);
            labelGuest.SetActive(true);
            foreach (GameObject i in disableOnSceneTransition)
            {
                i.SetActive(false);
            }
            GoToScene(SceneNames.SCREENS_SCENE, true);
            Debug.Log("=====> PlayerInfo --->" + AuthenticationService.Instance.PlayerId);
        }

        void SignedOut()
        {
            Debug.Log("===> You're signed Out to UGS!!!");
        }
        void Expired()
        {
            Debug.Log("===> You're Expired from UGS!!!");
        }

        private IMusicPlayer CreateMusicPlayer()
        {
            AudioSource platformAudioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(platformAudioSource);
            IAudioSource audioSource
                = new MusicVolumeAudioSource(
                    new AudioSourceBC(platformAudioSource),
                    DataProvider.SettingsManager);

            return
                new MusicPlayer(
                    new SingleSoundPlayer(audioSource));
        }

        public void GoToScene(string sceneName, bool stopMusic)
        {
            string hint = null;
            if (sceneName == SceneNames.BATTLE_SCENE
                && !ApplicationModel.IsTutorial)
            {
                hint = _hintProvider.GetHint();
            }
            if (sceneName == SceneNames.PvP_BOOT_SCENE && !ApplicationModel.IsTutorial)
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
                if (MatchmakingScreenController.Instance != null)
                    MatchmakingScreenController.Instance.Destroy();
            }
            else
            {
                if (LoadingScreenController.Instance != null)
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

        public void Update()
        {
#if PLATFORM_IOS
            if (_AppleAuthManager != null)
            {
                _AppleAuthManager.Update();
            }
#endif
        }

        public void OnQuit()
        {
            Application.Quit();
        }

        void OnDestroy()
        {
            AuthenticationService.Instance.SignOut();
            AuthenticationService.Instance.SignedIn -= SignedIn;
            AuthenticationService.Instance.SignedOut -= SignedOut;
            AuthenticationService.Instance.SignInFailed -= SignFailed;
            AuthenticationService.Instance.Expired -= Expired;
        }

        public static async Task<bool> CheckForInternetConnection(int timeoutMs = 10000, string url = "https://www.google.com")
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.KeepAlive = false;
                request.Timeout = timeoutMs;
                using (await request.GetResponseAsync())
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
