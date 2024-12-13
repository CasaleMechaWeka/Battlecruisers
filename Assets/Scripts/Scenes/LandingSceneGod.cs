using BattleCruisers.Data;
using BattleCruisers.UI.Loading;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.ScreensScene.Multiplay.ArenaScreen;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
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
using BattleCruisers.Utils.Localisation;
using BattleCruisers.UI;
using Unity.Services.Core;
using System.Net;
using BattleCruisers.Utils.Network;
using BattleCruisers.Utils.Properties;
using UnityEngine.UI;
using TMPro;

#if PLATFORM_ANDROID
using GooglePlayGames.BasicApi;
#endif

#if PLATFORM_IOS
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Extensions;
using AppleAuth.Interfaces;
using AppleAuth.Native;
#endif
using BattleCruisers.UI.ScreensScene.BattleHubScreen;

#if UNITY_EDITOR
using System.Security.Cryptography;
#endif

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

        public GameObject loginPanel, retryPanel;

        public GameObject logos;
        public CanvasGroupButton appleBtn, googleBtn, guestBtn;
        public GameObject spinApple, spinGoogle, spinGuest;
        public GameObject labelApple, labelGoogle, labelGuest;

        public GameObject quitBtn, retryBtn;
        public GameObject spinRetry;
        public GameObject labelRetry;

        public const string AuthProfileCommandLineArg = "-AuthProfile";
        public ILocTable commonStrings;
        public ILocTable hecklesStrings;
        public ILocTable screenSceneStrings;

        public static LandingSceneGod Instance;
        public LoginType loginType = LoginType.None;

        public ErrorMessageHandler messageHandler;

        private INetworkState _currentInternetConnectivity;
        private INetworkState CurrentInternetConnectivity
        {
            get => _currentInternetConnectivity;
            set
            {
                Assert.IsNotNull(value);
                _currentInternetConnectivity = value;
                _internetConnectivity.Value = _currentInternetConnectivity.IsConnected;
            }
        }

#if PLATFORM_ANDROID
        public static IGoogleAuthentication _GoogleAuthentication { get; set; }
#endif
#if PLATFORM_IOS
        public IAppleAuthManager _AppleAuthManager;
        private const string AppleUserIdKey = "AppleUserId";
        private const string AppleUserToken = "AppleUserToken";
#endif


        private SettableBroadcastingProperty<bool> _internetConnectivity = new SettableBroadcastingProperty<bool>(false);
        public IBroadcastingProperty<bool> InternetConnectivity { get; set; }
        public int coinBattleLevelNum = -1;
        private INetworkState ConnectedState = new InternetConnectivity(true);
        private INetworkState DisconnectedState = new InternetConnectivity(false);

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
                Instance = this;
            LogToScreen(Application.platform.ToString());
            messagebox.HideMessage();
            Helper.AssertIsNotNull(landingCanvas, loginPanel, retryPanel, logos, googleBtn, guestBtn, quitBtn, retryBtn);
            Helper.AssertIsNotNull(spinGoogle, spinGuest, spinRetry);
            Helper.AssertIsNotNull(labelGoogle, labelGuest, labelRetry);
            Helper.AssertIsNotNull(messageHandler);
            LogToScreen("Starting Battlecruisers"); // SCREEN START

            //loading loc tables in parallel is about 40-100% faster
            //Starting these tasks here saves ~100 ms avg
            Task<ILocTable> loadCommonStrings = LocTableFactory.Instance.LoadCommonTableAsync();
            Task<ILocTable> loadHeckesStrings = LocTableFactory.Instance.LoadHecklesTableAsync();
            Task<ILocTable> loadScreensSceneStrings = LocTableFactory.Instance.LoadScreensSceneTableAsync();

            IApplicationModel applicationModel = ApplicationModelProvider.ApplicationModel;

            bool startingState = await CheckForInternetConnection();

            if (startingState)
                CurrentInternetConnectivity = ConnectedState;
            else
                CurrentInternetConnectivity = DisconnectedState;
            InternetConnectivity = new BroadcastingProperty<bool>(_internetConnectivity);

            ISoundFetcher soundFetcher = new SoundFetcher();
            AudioSource platformAudioSource = GetComponent<AudioSource>();
            Assert.IsNotNull(platformAudioSource);
            IAudioSource audioSource
                = new MusicVolumeAudioSource(
                    new AudioSourceBC(platformAudioSource),
                    applicationModel.DataProvider.SettingsManager);

            soundPlayer = new SingleSoundPlayer(
                new SoundFetcher(),
                audioSource
                );

            try
            {
                var options = new InitializationOptions();
                options.SetEnvironmentName("production");
                //options.SetEnvironmentName("dev");
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

                if (CurrentInternetConnectivity.IsConnected)
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
                if (InternetConnectivity.Value)
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
            messagebox.Initialize(dataProvider, soundPlayer);
            MusicPlayer = CreateMusicPlayer(dataProvider);
            DontDestroyOnLoad(gameObject);
            SceneNavigator = this;

            await Task.WhenAll(loadCommonStrings, loadHeckesStrings, loadScreensSceneStrings);

            commonStrings = loadCommonStrings.Result;
            hecklesStrings = loadHeckesStrings.Result;
            screenSceneStrings = loadScreensSceneStrings.Result;

            HintProviders hintProviders = new HintProviders(RandomGenerator.Instance, commonStrings);
            _hintProvider = new CompositeHintProvider(hintProviders.BasicHints, hintProviders.AdvancedHints, dataProvider.GameModel, RandomGenerator.Instance);

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

            if (CurrentInternetConnectivity.IsConnected)
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

            retryPanel.SetActive(false);
            labelRetry.SetActive(true);
            spinRetry.SetActive(false);

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
            guestBtn.GetComponentInChildren<TMP_Text>().text = screenSceneStrings.GetString("UI/HomeScreen/PlayButton");

            if (CurrentInternetConnectivity.IsConnected)
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
#elif PLATFORM_ANDROID
            return SystemInfo.deviceUniqueIdentifier.Length > 30 ? SystemInfo.deviceUniqueIdentifier.Substring(0, 30) : SystemInfo.deviceUniqueIdentifier;
#elif PLATFORM_IOS
            return SystemInfo.deviceUniqueIdentifier.Length > 30 ? SystemInfo.deviceUniqueIdentifier.Substring(0, 30) : SystemInfo.deviceUniqueIdentifier;
#elif PLATFORM_STANDALONE_WIN
            return SystemInfo.deviceUniqueIdentifier.Length > 30 ? SystemInfo.deviceUniqueIdentifier.Substring(0, 30) : SystemInfo.deviceUniqueIdentifier;
#endif
        }

#if PLATFORM_ANDROID
        // Google login by button:
        public async void GoogleLogin()
        {
            LogToScreen("Attempting login with Google"); // ON GOOGLE BUTTON PRESS
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                SetInteractable(false);
                spinGoogle.SetActive(true);
                labelGoogle.SetActive(false);
                loginType = LoginType.Google;

                try
                {
                    bool state = await _GoogleAuthentication.Authenticate(SignInInteractivity.CanPromptAlways); // The comments for these enums are actually pretty good!
                    if (state != true)
                    {
                        spinGoogle.SetActive(false);
                        labelGoogle.SetActive(true);
                        SetInteractable(true);
                    }
                }
                catch (Exception ex)
                {
                    LogToScreen("Error while trying to log in with Google"); // IF GOOGLE AUTH FAILS FOR ANY REASON
                    Debug.Log(ex.Message);
                    spinGoogle.SetActive(false);
                    labelGoogle.SetActive(true);
                    SetInteractable(true);
                }
            }
        }

        // Attempt Google signin without user input:
        private async Task GoogleAttemptSilentSigningAsync(ISingleSoundPlayer soundPlayer)
        {
            try
            {
                bool state = await _GoogleAuthentication.Authenticate(SignInInteractivity.NoPrompt);
                if (state != true)
                {
                    ShowSignInScreen();
                    Debug.Log("Google silent signin unsuccessful.");
                }
            }
            catch (Exception ex)
            {
                // if it fails, show the landing buttons:
                ShowSignInScreen();
                Debug.Log(ex.Message);
            }
        }
#endif

#if PLATFORM_IOS
        // Apple login by button:
        private async void AppleLogin()
        {
            LogToScreen("Attempting login with Apple"); // ON APPLE BUTTON PRESS
            if (!AuthenticationService.Instance.IsSignedIn)
            {
                SetInteractable(false);
                spinApple.SetActive(true);
                labelApple.SetActive(false);
                loginType = LoginType.Apple;

                try
                {
                    // Initialize the Apple Auth Manager
                    if (_AppleAuthManager == null)
                    {
                        InitializeAppleAuth();
                    }

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
                            }
                            else
                            {
                                Debug.Log("Sign-in with Apple error. Message: appleIDCredential is null");
                                LogToScreen("Retrieving Apple Id Token failed."); //Localise for prod
                                spinApple.SetActive(false);
                                labelApple.SetActive(true);
                                SetInteractable(true);
                            }
                        },
                        error =>
                        {
                            Debug.Log("Sign-in with Apple error. Message: " + error.ToString());
                            LogToScreen("Login Unsuccessful: " + error.ToString()); //Localise for prod
                            spinApple.SetActive(false);
                            labelApple.SetActive(true);
                            SetInteractable(true);
                        }
                    );
                }
                catch (Exception ex)
                {
                    LogToScreen("Login Exception: " + ex.Message); //Localise for prod
                    Debug.Log(ex.Message);
                    spinApple.SetActive(false);
                    labelApple.SetActive(true);
                    SetInteractable(true);
                }
            }
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
                {
                    InitializeAppleAuth();
                }

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
            if (InternetConnectivity.Value)
            {
                if (!AuthenticationService.Instance.IsSignedIn)
                {
                    SetInteractable(false);
                    spinGuest.SetActive(true);
                    labelGuest.SetActive(false);
                    loginType = LoginType.Anonymous;
                    try
                    {
                        await AuthenticationService.Instance.SignInAnonymouslyAsync();
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.Message);

                        // This only happens if UGS fails (boooooo)
                        LogToScreen("Unity Game Services could not be reached"); // IF UNITY GAME SERVICES FAILS FOR ANY REASON

                        // play without Internet
                        loginType = LoginType.NoInternet;
                        loginPanel.SetActive(false);
                        spinGuest.SetActive(false);
                        labelGuest.SetActive(true);
                        foreach (GameObject i in disableOnSceneTransition)
                            i.SetActive(false);

                        GoToScene(SceneNames.SCREENS_SCENE, true);
                    }
                }
            }
            else
            {
                // play without Internet
                loginType = LoginType.NoInternet;
                loginPanel.SetActive(false);
                spinGuest.SetActive(false);
                labelGuest.SetActive(true);
                foreach (GameObject i in disableOnSceneTransition)
                {
                    i.SetActive(false);
                }
                GoToScene(SceneNames.SCREENS_SCENE, true);
            }
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
            loginType = LoginType.None;
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

        //void Update()
        //{
        //    if (!isUpdatingInternetConnectivity)
        //    {
        //        isUpdatingInternetConnectivity = true;
        //        iUpdateInternetConnectivity();
        //    }
        //}

        public void Update()
        {
#if PLATFORM_IOS
            if (_AppleAuthManager != null)
            {
                _AppleAuthManager.Update();
            }
#endif
        }

        async void iUpdateInternetConnectivity()
        {
            await Task.Delay(5000);

            if (this == null)
                return;
            bool currentState = await CheckForInternetConnection();
            if (this == null)
                return;

            if (currentState)
                CurrentInternetConnectivity = ConnectedState;
            else
                CurrentInternetConnectivity = DisconnectedState;
        }

        public void OnRetry()
        {
            if (loginType == LoginType.Anonymous)
                AnonymousLogin();
#if PLATFORM_ANDROID
            if (loginType == LoginType.Google)
                GoogleLogin();
#elif PLATFORM_IOS
            if (loginType == LoginType.Apple)
                AppleLogin();
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

#if PLATFORM_IOS

#endif

        public enum LoginType { Google, Apple, Anonymous, NoInternet, None }
    }
}
