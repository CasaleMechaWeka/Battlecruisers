using BattleCruisers.Network.Multiplay.Matchplay.Client;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.UI;
using BattleCruisers.Network.Multiplay.ApplicationLifecycle;
using BattleCruisers.Network.Multiplay.Scenes;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.BattleHubScreen;

public class PrivateMatchmakingPanel : MonoBehaviour
{
    [SerializeField] CanvasGroupButton HostButton;
    [SerializeField] CanvasGroupButton JoinButton;


    [SerializeField] GameObject HostPage;
    [SerializeField] GameObject JoinPage;


    [SerializeField] CanvasGroupButton BattleButton;

    [SerializeField] Text text;
    [SerializeField] GameObject LobbyCode;
    [SerializeField] TMP_Text lobbyCodeText;
    [SerializeField] GameObject idHighlight;
    [SerializeField] AnimationClip idAnim;
    [SerializeField] CanvasGroupButton idButton;
    [SerializeField] Text battleButtonText;
    [SerializeField] InputField lobbyCodeInput;
    [SerializeField] GameObject connectingOverlay;
    public Image backgroundImage;

    private ArenaSelectPanelScreenController _cachedArenaPanel;

    Lobby lobby;
    public bool isHosting;
    int lastKnownPlayerCount = 0;
    bool gameStarted = false;
    Lobby clientLobby; // CLIENT: track lobby for polling player count
    public bool isWaitingInLobby = false; // CLIENT: true when polling lobby, waiting for match start
    bool isCancelled = false; // Set to true when CancelHosting called, prevents Start*Lobby from running

    void Awake()
    {
        if (BattleButton == null) Debug.LogError("BattleButton not assigned");
        if (battleButtonText == null) Debug.LogError("battleButtonText not assigned");
        if (lobbyCodeText == null) Debug.LogError("lobbyCodeText not assigned");
        if (lobbyCodeInput == null) Debug.LogError("lobbyCodeInput not assigned");

        _cachedArenaPanel = FindFirstObjectByType<ArenaSelectPanelScreenController>();

        if (battleButtonText != null)
        {
            battleButtonText.text = GetBattleButtonTextForCurrentState();
        }

        MatchplayNetworkClient.OnConnectionFailed += HandleConnectionFailed;
    }

    public void Initialise(SingleSoundPlayer soundPlayer)
    {
        LobbyCode.SetActive(false);
        HostButton.Initialise(soundPlayer, HostClicked);
        JoinButton.Initialise(soundPlayer, JoinClicked);
        BattleButton.Initialise(soundPlayer, BattleClicked);
        idButton.Initialise(soundPlayer, CopyCode);

        battleButtonText.text = GetBattleButtonTextForCurrentState();

        lobbyCodeInput.onEndEdit.AddListener((code) => {
            if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.Return) ||
                UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.KeypadEnter))
            {
                OnLobbyCodeInput(code);
            }
        });

        if (connectingOverlay != null) connectingOverlay.SetActive(false);
    }

    public void SetArenaBackground(GameObject arenaPrefab, int arenaIndex = 0)
    {
        if (backgroundImage != null)
        {
            foreach (Transform child in backgroundImage.transform)
            {
                Destroy(child.gameObject);
            }

            if (arenaPrefab != null)
            {
                GameObject arenaVisual = Instantiate(arenaPrefab, backgroundImage.transform);
                arenaVisual.transform.localScale = Vector3.one;
                arenaVisual.transform.localPosition = Vector3.zero;

                // Show only the selected arena child, hide others
                for (int i = 0; i < arenaVisual.transform.childCount; i++)
                {
                    arenaVisual.transform.GetChild(i).gameObject.SetActive(i == arenaIndex);
                }
            }
        }
    }

    void OnEnable()
    {
        if (LobbyCode != null)
            LobbyCode.SetActive(false);

        if (!isJoining && !isWaitingInLobby && !isHosting)
        {
            TakeControlOfBattleButton();
        }
    }
    void TakeControlOfBattleButton()
    {
        if (BattleButton != null)
        {
            if (_cachedArenaPanel != null && _cachedArenaPanel.loadingSpinner != null)
            {
                _cachedArenaPanel.loadingSpinner.SetActive(false);
            }

            BattleButton.gameObject.SetActive(true);
            battleButtonText.text = GetBattleButtonTextForCurrentState();
        }
        else
        {
            Debug.LogError("TakeControlOfBattleButton - BattleButton is null!");
        }
    }

    string GetBattleButtonTextForCurrentState()
    {
        // If actively hosting or waiting in lobby as client, always show CANCEL
        if (isHosting || isWaitingInLobby)
        {
            return "CANCEL";
        }

        // Otherwise, determine text based on which page is active
        if (HostPage != null && HostPage.activeSelf)
        {
            return "HOST";
        }
        else if (JoinPage != null && JoinPage.activeSelf)
        {
            return "JOIN";
        }
        // Future: LobbyPage would return "READY"
        // Future: SpectatePage would return "SPECTATE" or hide button entirely
        else
        {
            // Fallback for unknown state
            Debug.LogWarning("GetBattleButtonTextForCurrentState - no page active, defaulting to HOST");
            return "HOST";
        }
    }

    void ReleaseControlOfBattleButton()
    {
        if (BattleButton != null && !isHosting)
        {
            battleButtonText.text = "BATTLE";
        }
    }

    bool isJoining = false;

    public void OnLobbyCodeInput(string code)
    {
        _ = OnLobbyCodeInputAsync(code).ContinueWith(t =>
        {
            if (t.IsFaulted)
                Debug.LogError($"OnLobbyCodeInput failed: {t.Exception}");
        });
    }

    async System.Threading.Tasks.Task OnLobbyCodeInputAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code)) return;

        if (isJoining)
        {
            Debug.LogWarning($"Already joining lobby - ignoring duplicate call");
            return;
        }

        isJoining = true;

        ArenaSelectPanelScreenController.PrivateMatch = true;
        isWaitingInLobby = true;
        isCancelled = false;
        battleButtonText.text = "CANCEL";

        if (connectingOverlay != null) connectingOverlay.SetActive(true);

        if (LandingSceneGod.Instance != null)
        {
            LandingSceneGod.Instance.DisableAuthNavigation();
        }

        if (ApplicationController.Instance == null)
        {
            Debug.LogError("ApplicationController.Instance is NULL! Cannot join lobby.");
            if (connectingOverlay != null) connectingOverlay.SetActive(false);
            isWaitingInLobby = false;
            battleButtonText.text = "JOIN";
            return;
        }

        try
        {
            clientLobby = await PvPBootManager.Instance.JoinLobbyByCode(code);

            if (clientLobby == null)
            {
                Debug.LogError("Lobby not found");
                if (connectingOverlay != null) connectingOverlay.SetActive(false);
                isWaitingInLobby = false;
                battleButtonText.text = "JOIN";
                LandingSceneGod.Instance.messagebox.ShowMessage("Lobby not found. Please check the code and try again.");
                return;
            }

            PvPBootManager.Instance.LobbyServiceFacade.BeginTracking();
            StartCoroutine(ClientLobbyPoll());
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to join lobby: {ex.Message}");
            if (connectingOverlay != null) connectingOverlay.SetActive(false);
            isWaitingInLobby = false;
            battleButtonText.text = "JOIN";
            LandingSceneGod.Instance.messagebox.ShowMessage($"Failed to join lobby: {ex.Message}");
        }
        finally
        {
            isJoining = false;
            if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject == lobbyCodeInput.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
            if (lobbyCodeInput != null)
            {
                lobbyCodeInput.interactable = true;
            }
        }
    }

    private void HandleConnectionFailed()
    {
        if (connectingOverlay != null) connectingOverlay.SetActive(false);
        isWaitingInLobby = false;
        battleButtonText.text = JoinPage.activeSelf ? "JOIN" : "HOST";
        lobbyCodeInput.interactable = true;
        LandingSceneGod.Instance.messagebox.ShowMessage("Failed to connect to the lobby. Please check the code and try again.");
    }
    private void OnHostButtonClicked(object sender, EventArgs e)
    {
        _ = OnHostButtonClickedAsync(sender, e).ContinueWith(t =>
        {
            if (t.IsFaulted)
                Debug.LogError($"OnHostButtonClicked failed: {t.Exception}");
        });
    }

    private async System.Threading.Tasks.Task OnHostButtonClickedAsync(object sender, EventArgs e)
    {
        if (text != null) text.text = "HOST";
        HostPage.SetActive(true);
        JoinPage.SetActive(false);

        if (isWaitingInLobby)
        {
            CancelHosting();
            return;
        }

        if (!isHosting)
        {
            battleButtonText.text = "HOST";
        }

        if (isHosting || !AuthenticationService.Instance.IsSignedIn)
        {
            return;
        }

        DataProvider.GameModel.GameMap = (_cachedArenaPanel != null) ? _cachedArenaPanel.IndexCurrentArena : 0;

        PvPBattleSceneGodTunnel.isCost = false;
        PvPBattleCompletionHandler._isCompleted = false;

        if (LandingSceneGod.MusicPlayer != null)
            LandingSceneGod.MusicPlayer.Stop();

        ArenaSelectPanelScreenController.PrivateMatch = true;

        if (LandingSceneGod.Instance != null)
        {
            LandingSceneGod.Instance.DisableAuthNavigation();
        }

        if (ApplicationController.Instance == null)
        {
            Debug.LogError("ApplicationController.Instance is NULL! Cannot create lobby.");
            if (connectingOverlay != null) connectingOverlay.SetActive(false);
            return;
        }

        isHosting = true;
        isCancelled = false;
        lastKnownPlayerCount = 0;

        string selectedMap = (_cachedArenaPanel != null) ? _cachedArenaPanel.IndexCurrentArena.ToString() : "0";
        lobby = await PvPBootManager.Instance.CreateLobby(selectedMap, false);

        if (lobby == null)
        {
            Debug.LogError("Failed to create lobby");
            isHosting = false;
            battleButtonText.text = "HOST";
            if (connectingOverlay != null) connectingOverlay.SetActive(false);
            return;
        }

        LobbyCode.SetActive(true);
        lobbyCodeText.text = lobby.LobbyCode;
        battleButtonText.text = "CANCEL";

        PvPBootManager.Instance.LobbyServiceFacade.BeginTracking();
        StartCoroutine(LobbyLoop());
    }
    private void OnJoinButtonClicked(object sender, EventArgs e)
    {
        if (text != null) text.text = "JOIN";
        HostPage.SetActive(false);
        JoinPage.SetActive(true);
        battleButtonText.text = "JOIN";

        if (isHosting)
        {
            CancelHosting();
        }
    }
    private void OnBattleButtonClicked(object sender, EventArgs e)
    {
        if (isHosting || isWaitingInLobby)
        {
            CancelHosting();
            return;
        }

        if (HostPage != null && HostPage.activeSelf)
        {
            OnHostButtonClicked(this, EventArgs.Empty);
        }
        else if (JoinPage != null && JoinPage.activeSelf)
        {
            string code = lobbyCodeInput.text?.Trim().ToUpper();
            if (string.IsNullOrEmpty(code))
            {
                LandingSceneGod.Instance.messagebox.ShowMessage("Please enter a lobby code");
                return;
            }
            OnLobbyCodeInput(code);
        }
    }
    public void CancelHosting()
    {
        _ = CancelHostingAsync().ContinueWith(t =>
        {
            if (t.IsFaulted)
                Debug.LogError($"CancelHosting failed: {t.Exception}");
        });
    }

    async System.Threading.Tasks.Task CancelHostingAsync()
    {
        isCancelled = true;
        StopAllCoroutines();

        if (PvPBootManager.Instance?.LobbyServiceFacade != null)
        {
            await PvPBootManager.Instance.LobbyServiceFacade.EndTracking();
        }

        if (lobby != null && !string.IsNullOrEmpty(lobby.Id))
        {
            try
            {
                await PvPBootManager.Instance.LobbyServiceFacade.DeleteLobbyAsync(lobby.Id);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to delete lobby: {ex.Message}");
            }
        }

        lobby = null;
        clientLobby = null;
        lastKnownPlayerCount = 0;
        gameStarted = false;
        isWaitingInLobby = false;
        isCancelled = false;

        if (ApplicationController.Instance?.NetworkManager != null && ApplicationController.Instance.NetworkManager.IsListening)
        {
            ApplicationController.Instance.NetworkManager.Shutdown();
        }

        LobbyCode.SetActive(false);
        isHosting = false;

        if (connectingOverlay != null) connectingOverlay.SetActive(false);

        if (JoinPage != null && JoinPage.activeSelf)
        {
            battleButtonText.text = "JOIN";
        }
        else
        {
            battleButtonText.text = "HOST";
        }

        ArenaSelectPanelScreenController.PrivateMatch = false;
    }
    // Wrappers to reuse existing handlers with CanvasGroupButton.Initialise(Action)
    private void HostClicked() { OnHostButtonClicked(this, EventArgs.Empty); }
    private void JoinClicked() { OnJoinButtonClicked(this, EventArgs.Empty); }
    private void BattleClicked()
    {
        OnBattleButtonClicked(this, EventArgs.Empty);
    }

    public void CopyCode()
    {
        UniClipboard.SetText(lobby.LobbyCode);
        StartCoroutine(AnimateCopy());
    }


    IEnumerator AnimateCopy()
    {
        idHighlight.SetActive(true);
        yield return new WaitForSeconds(idAnim.length);
        idHighlight.SetActive(false);
    }

    public void OnDestroy()
    {
        HostButton.Clicked -= OnHostButtonClicked;
        JoinButton.Clicked -= OnJoinButtonClicked;
        BattleButton.Clicked -= OnBattleButtonClicked;
        MatchplayNetworkClient.OnConnectionFailed -= HandleConnectionFailed;
    }

    IEnumerator LobbyLoop()
    {
        while (lobby != null)
        {
            yield return new WaitForSeconds(0.1f);

            Lobby liveLobby = PvPBootManager.Instance?.LobbyServiceFacade?.CurrentUnityLobby;

            if (liveLobby != null)
            {
                int currentPlayerCount = liveLobby.Players.Count;

                if (currentPlayerCount != lastKnownPlayerCount)
                {
                    lastKnownPlayerCount = currentPlayerCount;

                    if (currentPlayerCount == 2 && connectingOverlay != null)
                    {
                        connectingOverlay.SetActive(true);
                    }
                }

                if (isHosting && currentPlayerCount == 2 && !gameStarted && !isCancelled)
                {
                    gameStarted = true;

                    if (PrivateMatchmakingController.Instance != null)
                    {
                        if (PrivateMatchmakingController.Instance.backgroundMusic != null && PrivateMatchmakingController.Instance.backgroundMusic.isPlaying)
                            PrivateMatchmakingController.Instance.backgroundMusic.Stop();

                        if (PrivateMatchmakingController.Instance.enemyFoundMusic != null)
                            PrivateMatchmakingController.Instance.enemyFoundMusic.Play();

                        if (PrivateMatchmakingController.Instance.backButton != null)
                        {
                            PrivateMatchmakingController.Instance.backButton.gameObject.SetActive(false);
                        }
                    }

                    if (BattleButton != null)
                    {
                        BattleButton.gameObject.SetActive(false);
                    }

                    if (PrivateMatchmakingController.Instance != null)
                    {
                        Debug.Log("PVP: HOST path - before ReEnableBattleSceneGameObjects");
                        PrivateMatchmakingController.Instance.ReEnableBattleSceneGameObjects();
                        Debug.Log("PVP: HOST path - after ReEnableBattleSceneGameObjects");
                    }

                    Debug.Log("PVP: HOST path - before StartHostLobby");
                    PvPBootManager.Instance.ConnectionManager.StartHostLobby(DataProvider.GameModel.PlayerName);
                    Debug.Log("PVP: HOST path - after StartHostLobby");

                    StopCoroutine(nameof(LobbyLoop));
                    yield break;
                }

                if (isCancelled)
                {
                    yield break;
                }
            }
        }
    }

    IEnumerator ClientLobbyPoll()
    {
        bool hasUpdatedBackground = false;

        while (!gameStarted)
        {
            yield return new WaitForSeconds(0.1f);

            Lobby liveLobby = PvPBootManager.Instance?.LobbyServiceFacade?.CurrentUnityLobby;
            if (liveLobby != null)
            {
                int playerCount = liveLobby.Players.Count;
                string relayCode = (liveLobby.Data.ContainsKey("RelayJoinCode") && !string.IsNullOrEmpty(liveLobby.Data["RelayJoinCode"].Value)) ? liveLobby.Data["RelayJoinCode"].Value : "null";
                bool hasRelayCode = relayCode != "null";

                if (playerCount == 2 && !hasUpdatedBackground && liveLobby.Data.ContainsKey("GameMap"))
                {
                    if (int.TryParse(liveLobby.Data["GameMap"].Value, out int hostMapIndex))
                    {
                        if (PrivateMatchmakingController.Instance != null && PrivateMatchmakingController.Instance.arenaBackgroundPrefab != null)
                        {
                            SetArenaBackground(PrivateMatchmakingController.Instance.arenaBackgroundPrefab, hostMapIndex);
                            hasUpdatedBackground = true;
                        }
                    }
                }

                if (playerCount == 2 && hasRelayCode && !isCancelled)
                {
                    gameStarted = true;

                    if (PrivateMatchmakingController.Instance != null)
                    {
                        if (PrivateMatchmakingController.Instance.backgroundMusic != null && PrivateMatchmakingController.Instance.backgroundMusic.isPlaying)
                            PrivateMatchmakingController.Instance.backgroundMusic.Stop();

                        if (PrivateMatchmakingController.Instance.enemyFoundMusic != null)
                            PrivateMatchmakingController.Instance.enemyFoundMusic.Play();

                        if (PrivateMatchmakingController.Instance.backButton != null)
                        {
                            PrivateMatchmakingController.Instance.backButton.gameObject.SetActive(false);
                        }
                    }

                    if (BattleButton != null)
                    {
                        BattleButton.gameObject.SetActive(false);
                    }

                    if (PrivateMatchmakingController.Instance != null)
                    {
                        Debug.Log("PVP: CLIENT path - before ReEnableBattleSceneGameObjects");
                        PrivateMatchmakingController.Instance.ReEnableBattleSceneGameObjects();
                        Debug.Log("PVP: CLIENT path - after ReEnableBattleSceneGameObjects");
                    }

                    Debug.Log("PVP: CLIENT path - before StartClientLobby");
                    PvPBootManager.Instance.ConnectionManager.StartClientLobby(DataProvider.GameModel.PlayerName);
                    Debug.Log("PVP: CLIENT path - after StartClientLobby");

                    StopCoroutine(nameof(ClientLobbyPoll));

                    yield break;
                }

                if (isCancelled)
                {
                    yield break;
                }
            }
        }
    }
    public async System.Threading.Tasks.Task CancelHostingFromExternalAsync()
    {
        if (isHosting)
        {
            await CancelHostingAsync();
        }
    }
}