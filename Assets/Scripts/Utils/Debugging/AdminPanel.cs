using BattleCruisers.Ads;
using BattleCruisers.Buildables;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene;
using BattleCruisers.Scenes;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.Utils.Debugging
{
    public class AdminPanel : CheaterBase, IPointerClickHandler
    {
        public GameObject buttons;

        [SerializeField]
        private int levelToUnlock = 31; // The highest level to unlock, adjustable in the Inspector

        [Header("Ad Testing")]
        [SerializeField]
        [Tooltip("Link FullScreenAdverts from ScreensScene hierarchy")]
        private FullScreenAdverts fullScreenAdverts;

        [Header("On-Screen Logging")]
        [SerializeField]
        [Tooltip("Optional UI Text element for on-screen messages (fallback if overlay is not set)")]
        private UnityEngine.UI.Text screenMessageText;
        [SerializeField]
        [Tooltip("Optional panel to show overlay messages (set active to show/hide)")]
        private GameObject overlayPanel;
        [SerializeField]
        [Tooltip("Optional text inside the overlay panel")]
        private UnityEngine.UI.Text overlayText;
        
        private float messageDisplayTime = 5f;
        private float messageTimer = 0f;

        void Start()
        {
            Assert.IsNotNull(buttons);
            
            // Hide admin UI by default (admin button itself is controlled by ENABLE_CHEATS)
            buttons.SetActive(false);
            
            if (overlayPanel != null)
            {
                overlayPanel.SetActive(false);
            }
        }

        void Update()
        {
            // Auto-hide message after timer expires (overlay preferred, screen fallback)
            if (messageTimer > 0)
            {
                messageTimer -= Time.deltaTime;
                if (messageTimer <= 0)
                {
                    if (overlayText != null)
                    {
                        overlayText.text = "";
                    }
                    if (overlayPanel != null)
                    {
                        overlayPanel.SetActive(false);
                    }
                    else if (screenMessageText != null)
                    {
                        screenMessageText.text = "";
                    }
                }
            }
        }

        /// <summary>
        /// Show message on screen and in logs
        /// </summary>
        private void ShowMessage(string message, bool isError = false)
        {
            // Log to Unity console
            if (isError)
            {
                Debug.LogError($"[AdminPanel] {message}");
            }
            else
            {
                Debug.Log($"[AdminPanel] {message}");
            }

            // Try to show on LandingSceneGod if available
            if (LandingSceneGod.Instance != null)
            {
                LandingSceneGod.Instance.LogToScreen($"[Admin] {message}");
            }

            // Prefer overlay; fallback to screen text if overlay not assigned
            if (overlayText != null)
            {
                overlayText.text = message;
                messageTimer = messageDisplayTime;
                if (overlayPanel != null)
                {
                    overlayPanel.SetActive(true);
                }
            }
            else if (screenMessageText != null)
            {
                screenMessageText.text = message;
                messageTimer = messageDisplayTime;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (buttons.activeSelf)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        private void Show()
        {
            buttons.SetActive(true);
            ShowPlayerStatus();
        }

        public void Hide()
        {
            buttons.SetActive(false);
        }

        public void UnlockEverything()
        {
            ShowMessage("UNLOCKING EVERYTHING...");
            
            // Levels
            int levelCount = 0;
            foreach (Level level in StaticData.Levels)
            {
                DataProvider.GameModel.AddCompletedLevel(new CompletedLevel(level.Num, Difficulty.Normal));
                levelCount++;
            }

            int sideQuestCount = 0;
            foreach (SideQuestData sideQuest in StaticData.SideQuests)
            {
                DataProvider.GameModel.AddCompletedSideQuest(new CompletedLevel(sideQuest.SideLevelNum, Difficulty.Normal));
                sideQuestCount++;
            }

            // Hulls
            int hullCount = 0;
            foreach (HullKey hull in StaticData.HullKeys)
            {
                if (!DataProvider.GameModel.UnlockedHulls.Contains(hull))
                {
                    DataProvider.GameModel.AddUnlockedHull(hull);
                    hullCount++;
                }
            }

            // Buildings
            int buildingCount = 0;
            foreach (BuildingKey building in StaticData.BuildingKeys)
            {
                if (!DataProvider.GameModel.UnlockedBuildings.Contains(building))
                {
                    DataProvider.GameModel.AddUnlockedBuilding(building);
                    buildingCount++;
                }
            }

            // Units
            int unitCount = 0;
            foreach (UnitKey unit in StaticData.UnitKeys)
            {
                if (!DataProvider.GameModel.UnlockedUnits.Contains(unit))
                {
                    DataProvider.GameModel.AddUnlockedUnit(unit);
                    unitCount++;
                }
            }

            DataProvider.GameModel.HasAttemptedTutorial = true;

            // If never played a level, need to set last battle result
            if (DataProvider.GameModel.LastBattleResult == null)
            {
                DataProvider.GameModel.LastBattleResult = new BattleResult(levelNum: 1, wasVictory: false);
            }

            DataProvider.SaveGame();

            ShowMessage($"UNLOCKED: {levelCount} levels, {sideQuestCount} side quests, {hullCount} hulls, {buildingCount} buildings, {unitCount} units. RESTART GAME!");
        }

        public void ResetToState()
        {
            ShowMessage($"RESETTING to level {levelToUnlock}...");
            
            List<int> levelsToUnlock = Enumerable.Range(1, levelToUnlock).ToList();

            // Mark tutorial as complete
            DataProvider.GameModel.HasAttemptedTutorial = true;

            // Unlock specified levels
            foreach (int levelNum in levelsToUnlock)
            {
                Level level = StaticData.Levels.FirstOrDefault(l => l.Num == levelNum);
                if (level != null)
                {
                    DataProvider.GameModel.AddCompletedLevel(new CompletedLevel(level.Num, Difficulty.Normal));
                }
                else
                {
                    SideQuestData sideQuest = StaticData.SideQuests.FirstOrDefault(sq => sq.SideLevelNum == levelNum);
                    if (sideQuest != null)
                    {
                        DataProvider.GameModel.AddCompletedSideQuest(new CompletedLevel(sideQuest.SideLevelNum, Difficulty.Normal));
                    }
                }
            }

            // Ensure last battle result is set
            if (DataProvider.GameModel.LastBattleResult == null)
            {
                DataProvider.GameModel.LastBattleResult = new BattleResult(levelNum: 1, wasVictory: false);
            }

            DataProvider.SaveGame();

            ShowMessage($"RESET complete! Levels 1-{levelToUnlock} unlocked. RESTART GAME!");
        }

        public void Reset()
        {
            ShowMessage("FULL RESET - Deleting all save data...");
            DataProvider.Reset();
            ShowMessage("RESET COMPLETE! All data deleted. RESTART GAME!");
        }

        public void AddMoney()
        {
            long coinsBefore = DataProvider.GameModel.Coins;
            long creditsBefore = DataProvider.GameModel.Credits;

            DataProvider.GameModel.Coins += 16;
            DataProvider.GameModel.Credits += 32;
            DataProvider.SaveGame();

            ShowMessage($"COINS: {coinsBefore} → {DataProvider.GameModel.Coins} (+16); CREDITS: {creditsBefore} → {DataProvider.GameModel.Credits} (+32)");
        }

        public void RemoveMoney()
        {
            long coinsBefore = DataProvider.GameModel.Coins;
            long creditsBefore = DataProvider.GameModel.Credits;

            DataProvider.GameModel.Coins = System.Math.Max(0, DataProvider.GameModel.Coins - 15);
            DataProvider.GameModel.Credits = System.Math.Max(0, DataProvider.GameModel.Credits - 30);
            DataProvider.SaveGame();

            ShowMessage($"COINS: {coinsBefore} → {DataProvider.GameModel.Coins} (-15); CREDITS: {creditsBefore} → {DataProvider.GameModel.Credits} (-30)");
        }

        private DeadBuildableCounter CreateDamageCounter(long totalDamage, int destroyedCount)
        {
            var counter = new DeadBuildableCounter();
            
            // Handle zero case: no damage and no units destroyed
            if (destroyedCount == 0 || totalDamage == 0)
            {
                return counter;
            }

            int perUnit = (int)(totalDamage / destroyedCount);
            if (perUnit < 1) perUnit = 1;

            for (int i = 0; i < destroyedCount; i++)
            {
                counter.AddDeadBuildable(perUnit);
            }

            return counter;
        }

        private DeadBuildableCounter CreateTimeCounter(float seconds)
        {
            var counter = new DeadBuildableCounter();
            counter.AddPlayedTime(seconds);
            return counter;
        }

        public void SimulatePvEWin()
        {
            ApplicationModel.Mode = GameMode.Campaign;
            DataProvider.GameModel.SelectedLevel = 31;

            // Ensure at least 30 levels completed (NumOfLevelsCompleted is read-only, computed from completed levels count)
            for (int i = 1; i <= 30; i++)
            {
                if (DataProvider.GameModel.NumOfLevelsCompleted < i)
                {
                    DataProvider.GameModel.AddCompletedLevel(new CompletedLevel(i, Difficulty.Normal));
                }
            }

            BattleSceneGod.deadBuildables = new Dictionary<TargetType, DeadBuildableCounter>
            {
                { TargetType.Aircraft, CreateDamageCounter(0, 0) },
                { TargetType.Ships, CreateDamageCounter(0, 0) },
                { TargetType.Cruiser, CreateDamageCounter(6300, 1) },
                { TargetType.Buildings, CreateDamageCounter(4500, 10) },
                { TargetType.PlayedTime, CreateTimeCounter(115f) }
            };

            SceneNavigator.GoToScene(SceneNames.DESTRUCTION_SCENE, true);
        }

        public void SimulatePvPWin()
        {
            ApplicationModel.Mode = GameMode.PvP_1VS1;

            PvPBattleSceneGodTunnel.isDisconnected = 0;
            PvPBattleSceneGodTunnel.OpponentQuit = false;
            PvPBattleSceneGodTunnel.PlayerACruiserType = HullType.Bullshark;
            PvPBattleSceneGodTunnel.PlayerBCruiserType = HullType.Eagle;
            PvPBattleSceneGodTunnel.EnemyCruiserType = PvPBattleSceneGodTunnel.PlayerBCruiserType;

            PvPBattleSceneGodTunnel._levelTimeInSeconds = 180f;
            PvPBattleSceneGodTunnel._aircraftVal = 8500;
            PvPBattleSceneGodTunnel._shipsVal = 12000;
            PvPBattleSceneGodTunnel._cruiserVal = 7500;
            PvPBattleSceneGodTunnel._buildingsVal = 9200;
            PvPBattleSceneGodTunnel._totalDestroyed = new long[4] { 15, 8, 1, 12 };

            SceneNavigator.GoToScene(SceneNames.PvP_DESTRUCTION_SCENE, true);
        }

        public void SimulatePvPLoss()
        {
            ApplicationModel.Mode = GameMode.PvP_1VS1;

            PvPBattleSceneGodTunnel.isDisconnected = 0;
            PvPBattleSceneGodTunnel.OpponentQuit = false;
            PvPBattleSceneGodTunnel.PlayerACruiserType = HullType.Bullshark;
            PvPBattleSceneGodTunnel.PlayerBCruiserType = HullType.Eagle;
            PvPBattleSceneGodTunnel.EnemyCruiserType = PvPBattleSceneGodTunnel.PlayerBCruiserType;

            PvPBattleSceneGodTunnel._levelTimeInSeconds = 150f;
            PvPBattleSceneGodTunnel._aircraftVal = 3200;
            PvPBattleSceneGodTunnel._shipsVal = 5500;
            PvPBattleSceneGodTunnel._cruiserVal = 0;
            PvPBattleSceneGodTunnel._buildingsVal = 4100;
            PvPBattleSceneGodTunnel._totalDestroyed = new long[4] { 8, 4, 0, 7 };

            SceneNavigator.GoToScene(SceneNames.PvP_DESTRUCTION_SCENE, true);
        }

        /// <summary>
        /// Toggle between Premium and Free edition for ad testing
        /// </summary>
        public void TogglePremiumEdition()
        {
            DataProvider.GameModel.PremiumEdition = !DataProvider.GameModel.PremiumEdition;
            DataProvider.SaveGame();

            string status = DataProvider.GameModel.PremiumEdition ? "PREMIUM (no ads)" : "FREE (ads enabled)";
            ShowMessage($"Edition: {status}");

            // Log to Firebase for tracking
            if (FirebaseAnalyticsManager.Instance != null)
            {
                FirebaseAnalyticsManager.Instance.SetUserProperty("is_premium", DataProvider.GameModel.PremiumEdition.ToString());
            }
        }

        /// <summary>
        /// Reset ad counters to force next ad to show
        /// </summary>
        public void ResetAdCounters()
        {
            PlayerPrefs.DeleteKey("AdCounterKey");
            PlayerPrefs.DeleteKey("LastAdShowTime");
            PlayerPrefs.Save();
            ShowMessage("Ad frequency counter reset to 0. Cooldown timer cleared. Next ShouldShowAds() check will pass frequency/cooldown gates.");
        }

        /// <summary>
        /// Show an interstitial immediately if already loaded
        /// </summary>
        public void ShowInterstitialIfReady()
        {
            if (AppLovinManager.Instance == null)
            {
                ShowMessage("ERROR: AppLovinManager missing! Add a GameObject with AppLovinManager component to LandingScene.unity as a child of SceneGod (uses DontDestroyOnLoad).", true);
                return;
            }

            if (!AppLovinManager.Instance.IsInterstitialReady())
            {
                ShowMessage("Interstitial not ready yet. Use 'Show Ad Status' and wait for Interstitial Ready: True.", true);
                return;
            }

            ShowMessage("Showing interstitial (ready).");
            AppLovinManager.Instance.ShowInterstitial();
        }

        /// <summary>
        /// Show a rewarded ad and grant currency when reward is received
        /// </summary>
        public void ShowRewardedAndGrant()
        {
            Debug.Log("[AdminPanel] ShowRewardedAndGrant - enter");
            if (AppLovinManager.Instance == null)
            {
                ShowMessage("ERROR: AppLovinManager missing! Add a GameObject with AppLovinManager component to LandingScene.unity as a child of SceneGod (uses DontDestroyOnLoad).", true);
                Debug.Log("[AdminPanel] ShowRewardedAndGrant - AppLovinManager null");
                return;
            }

            if (!AppLovinManager.Instance.IsRewardedAdReady())
            {
                ShowMessage("Rewarded ad not ready yet. Use 'Show Ad Status' and wait for Rewarded Ready: True.", true);
                Debug.Log("[AdminPanel] ShowRewardedAndGrant - rewarded not ready");
                return;
            }

            // Grant on reward callback
            System.Action rewardHandler = null;
            rewardHandler = () =>
            {
                Debug.Log("[AdminPanel] ShowRewardedAndGrant - reward callback fired");
                GrantRewardedAdCurrency();
                AppLovinManager.Instance.OnRewardedAdRewarded -= rewardHandler;
            };

            AppLovinManager.Instance.OnRewardedAdRewarded += rewardHandler;

            var (coins, credits) = AdConfigManager.Instance?.GetRewardAmountsForPlayer() ?? (500, 4500);
            Debug.Log($"[AdminPanel] ShowRewardedAndGrant - show rewarded (coins:{coins}, credits:{credits})");
            ShowMessage($"Showing rewarded ad... Reward: {coins} coins, {credits} credits");
            AppLovinManager.Instance.ShowRewardedAd();
        }

        /// <summary>
        /// Show comprehensive player status information
        /// </summary>
        public void ShowPlayerStatus()
        {
            string playerName = DataProvider.GameModel?.PlayerName ?? "Unknown";
            string edition = DataProvider.GameModel.PremiumEdition ? "PREMIUM" : "FREE";
            string adStatus = AdConfigManager.HasEverWatchedRewardedAd() ? "ADWATCHER" : "VIRGIN";
            int level = DataProvider.GameModel.NumOfLevelsCompleted;
            long coins = DataProvider.GameModel.Coins;
            long credits = DataProvider.GameModel.Credits;
            
            var (nextCoins, nextCredits) = AdConfigManager.Instance?.GetRewardAmountsForPlayer() ?? (0, 0);
            
            string status = $"=== PLAYER STATUS ===\n" +
                            $"Name: {playerName}\n" +
                            $"Edition: {edition}\n" +
                            $"Ad Status: {adStatus}\n" +
                            $"Level: {level}\n" +
                            $"Coins: {coins} | Credits: {credits}\n" +
                            $"Next Reward: {nextCoins} coins, {nextCredits} credits\n" +
                            $"Interstitials: {(AdConfigManager.Instance?.InterstitialAdsEnabled ?? false ? "ON" : "OFF")}\n" +
                            $"Rewarded Ads: {(AdConfigManager.Instance?.RewardedAdsEnabled ?? false ? "ON" : "OFF")}";
            
            ShowMessage(status);
        }

        /// <summary>
        /// Toggle between Virgin (never watched ad) and Adwatcher (has watched ad)
        /// </summary>
        public void ToggleAdWatcherStatus()
        {
            if (AdConfigManager.HasEverWatchedRewardedAd())
            {
                // Currently Adwatcher -> Reset to Virgin
                AdConfigManager.ResetAdWatcherStatus();
                var (coins, credits) = AdConfigManager.Instance?.GetRewardAmountsForPlayer() ?? (500, 4500);
                ShowMessage($"Ad Status: VIRGIN (reset)\nNext reward: {coins} coins, {credits} credits");
            }
            else
            {
                // Currently Virgin -> Set to Adwatcher  
                AdConfigManager.MarkRewardedAdWatched();
                var (coins, credits) = AdConfigManager.Instance?.GetRewardAmountsForPlayer() ?? (20, 1200);
                ShowMessage($"Ad Status: ADWATCHER (set)\nNext reward: {coins} coins, {credits} credits");
            }
        }

        /// <summary>
        /// Helper method to grant rewarded ad currency and log changes
        /// </summary>
        private async void GrantRewardedAdCurrency()
        {
            // Get reward amounts based on first-time vs returning
            var (coins, credits) = AdConfigManager.Instance?.GetRewardAmountsForPlayer() ?? (500, 4500);

            // Store initial values
            long coinsBefore = DataProvider.GameModel.Coins;
            long creditsBefore = DataProvider.GameModel.Credits;

            Debug.Log($"[AdminPanel] Granting {coins} coins, {credits} credits");
            Debug.Log($"[AdminPanel] Before: Coins={coinsBefore}, Credits={creditsBefore}");

            // Mark as watched (only on first time)
            if (!AdConfigManager.HasEverWatchedRewardedAd())
            {
                AdConfigManager.MarkRewardedAdWatched();
                Debug.Log("[AdminPanel] Player marked as ADWATCHER");
            }

            // Grant rewards
            DataProvider.GameModel.Coins += coins;
            DataProvider.GameModel.Credits += credits;

            Debug.Log($"[AdminPanel] After: Coins={DataProvider.GameModel.Coins}, Credits={DataProvider.GameModel.Credits}");

            // Save locally first
            DataProvider.SaveGame();

            // CRITICAL: Sync rewarded currency changes to cloud immediately to prevent CloudLoad from overwriting
            try
            {
                await DataProvider.SyncCoinsToCloud();
                await DataProvider.SyncCreditsToCloud();
                Debug.Log("[AdminPanel] Currency synced to cloud");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[AdminPanel] Failed to sync to cloud: {e.Message}");
            }

            Debug.Log("[AdminPanel] Game saved locally");

            ShowMessage($"REWARDED! Coins: {coinsBefore} → {DataProvider.GameModel.Coins} (+{coins}); Credits: {creditsBefore} → {DataProvider.GameModel.Credits} (+{credits})");
        }

        /// <summary>
        /// Simulate a successfully watched rewarded ad (for testing reward grant logic)
        /// </summary>
        public void RewardedAdWatched()
        {
            GrantRewardedAdCurrency();
        }

        /// <summary>
        /// Simulate a failed/offline rewarded ad (shows joke ad fallback)
        /// </summary>
        public void RewardedAdOffline()
        {
            ShowMessage("REWARDED AD OFFLINE - No reward granted. Showing joke ad fallback.", true);
            
            Debug.Log("[AdminPanel] Simulating offline/interrupted rewarded ad - showing fallback joke ad");
            
            // Show the default/joke ad panel
            if (fullScreenAdverts != null)
            {
                fullScreenAdverts.defaultAd.UpdateImage();
                fullScreenAdverts.gameObject.SetActive(true);
            }
            else
            {
                ShowMessage("ERROR: FullScreenAdverts not linked! Assign in Inspector.", true);
            }
        }

        /// <summary>
        /// Show current ad configuration status
        /// </summary>
        public void ShowAdStatus()
        {
            bool isPremium = DataProvider.GameModel.PremiumEdition;
            int levelsCompleted = DataProvider.GameModel.NumOfLevelsCompleted;

            string statusMsg = "=== AD STATUS ===\n";
            statusMsg += $"Edition: {(isPremium ? "PREMIUM" : "FREE")}\n";
            statusMsg += $"Levels: {levelsCompleted}\n";

            if (AdConfigManager.Instance != null)
            {
                var config = AdConfigManager.Instance;
                statusMsg += $"Mode: {(config.AdsDisabled ? "DISABLED" : (config.AdsAreLive ? "PRODUCTION" : "TEST"))}\n";
                statusMsg += $"MinLevel: {config.MinimumLevelForAds}\n";
                statusMsg += $"Frequency: {config.GetAdFrequencyForPlayer(levelsCompleted)}\n";
                statusMsg += $"Cooldown: {config.AdCooldownMinutes}min\n";
                statusMsg += $"Veteran: {config.IsVeteranPlayer(levelsCompleted)}\n";
            }
            else
            {
                statusMsg += "AdConfigManager: MISSING - Add to scene!\n";
            }

            if (AppLovinManager.Instance != null)
            {
                bool interstitialReady = AppLovinManager.Instance.IsInterstitialReady();
                bool rewardedReady = AppLovinManager.Instance.IsRewardedAdReady();
                statusMsg += $"Interstitial Ready: {interstitialReady}\n";
                statusMsg += $"Rewarded Ready: {rewardedReady}\n";
            }
            else
            {
                statusMsg += "AppLovinManager: MISSING - Add to scene!\n";
            }

            if (fullScreenAdverts != null)
            {
                string counterStatus = fullScreenAdverts.GetAdCounterStatus();
                // Parse and reformat counter status for clarity
                if (counterStatus.Contains("Counter:"))
                {
                    string[] parts = counterStatus.Split(',');
                    if (parts.Length >= 1)
                    {
                        string counterPart = parts[0].Replace("Counter:", "").Trim();
                        if (counterPart.Contains("/"))
                        {
                            string[] counterVals = counterPart.Split('/');
                            if (counterVals.Length == 2 && int.TryParse(counterVals[0], out int current) && int.TryParse(counterVals[1], out int total))
                            {
                                statusMsg += $"Ad Counter: {current} of {total} battles until next ad (resets after ad shown)\n";
                            }
                            else
                            {
                                statusMsg += $"Ad Counter: {counterPart}\n";
                            }
                        }
                        else
                        {
                            statusMsg += $"Ad Counter: {counterPart}\n";
                        }
                    }
                    if (parts.Length >= 2)
                    {
                        statusMsg += $"Last Ad: {parts[1].Replace("Last:", "").Trim()}\n";
                    }
                }
                else
                {
                    statusMsg += $"Counter: {counterStatus}\n";
                }
            }

            ShowMessage(statusMsg);
        }

        /// <summary>
        /// Test Firebase Analytics by sending a test event
        /// </summary>
        public void TestFirebaseAnalytics()
        {
            if (FirebaseAnalyticsManager.Instance != null)
            {
                FirebaseAnalyticsManager.Instance.LogEvent("admin_test_event", new Dictionary<string, object>
                {
                    { "test_param", "test_value" },
                    { "timestamp", System.DateTime.Now.ToString() },
                    { "levels_completed", DataProvider.GameModel.NumOfLevelsCompleted },
                    { "is_premium", DataProvider.GameModel.PremiumEdition }
                });
                ShowMessage("Firebase test event SENT! Check DebugView in Firebase Console.");
            }
            else
            {
                ShowMessage("ERROR: FirebaseAnalyticsManager not found!", true);
            }
        }

        /// <summary>
        /// Refresh Ad Config from Unity Remote Config
        /// </summary>
        public async void RefreshAdConfig()
        {
            ShowMessage("Refreshing Ad Config from Unity Remote Config...");
            
            if (AdConfigManager.Instance != null)
            {
                await AdConfigManager.Instance.RefreshConfigAsync();
                ShowMessage($"Config refreshed! {AdConfigManager.Instance.GetStatusString()}");
            }
            else
            {
                ShowMessage("ERROR: AdConfigManager missing! Add a GameObject with AdConfigManager component to LandingScene.unity as a child of SceneGod (uses DontDestroyOnLoad).", true);
            }
        }


        /// <summary>
        /// Show detailed Remote Config values
        /// </summary>
        public void ShowRemoteConfigDetails()
        {
            if (AdConfigManager.Instance != null)
            {
                var snapshot = AdConfigManager.Instance.GetConfigSnapshot();
                string details = "=== REMOTE CONFIG ===\n";
                foreach (var kvp in snapshot)
                {
                    details += $"{kvp.Key}: {kvp.Value}\n";
                }
                ShowMessage(details);
            }
            else
            {
                ShowMessage("ERROR: AdConfigManager missing! Add a GameObject with AdConfigManager component to LandingScene.unity as a child of SceneGod (uses DontDestroyOnLoad).", true);
            }
        }

        /// <summary>
        /// Show MAX Mediation Debugger UI (for testing/debugging ad fill issues)
        /// </summary>
        public void ShowMediationDebugger()
        {
            if (AppLovinManager.Instance == null)
            {
                ShowMessage("ERROR: AppLovinManager missing! Add a GameObject with AppLovinManager component to LandingScene.unity as a child of SceneGod (uses DontDestroyOnLoad).", true);
                return;
            }

            // Check if SDK is initialized by trying to show debugger
            // AppLovinManager.ShowMediationDebugger() already checks isInitialized internally
            ShowMessage("Opening MAX Mediation Debugger... Check ad unit status, network fill rates, and error codes.");
            AppLovinManager.Instance.ShowMediationDebugger();
        }

        /// <summary>
        /// Show current coin and credit values
        /// </summary>
        public void ShowEconomyStatus()
        {
            long coins = DataProvider.GameModel.Coins;
            long credits = DataProvider.GameModel.Credits;
            
            Debug.Log($"[AdminPanel] Economy Status - Coins: {coins}, Credits: {credits}");
            
            ShowMessage($"=== ECONOMY STATUS ===\nCoins: {coins}\nCredits: {credits}");
        }

        private static string RelevantLogExportPath => Path.Combine(Application.persistentDataPath, "admin_relevant_logs.txt");

        /// <summary>
        /// Export condensed recent log lines (errors first) and show a preview in the panel.
        /// </summary>
        public void ShowRelevantLogs()
        {
            const int maxLines = 60;
            string sourcePath = null;

            List<string> lines = new List<string>();

            try
            {
                sourcePath = Application.consoleLogPath;
                if (!string.IsNullOrEmpty(sourcePath) && File.Exists(sourcePath))
                {
                    lines.AddRange(File.ReadAllLines(sourcePath));
                }
            }
            catch
            {
                // Ignore read issues; will fall back.
            }

            // Fallback to battle log if console log is unavailable.
            if (lines.Count == 0 && File.Exists(BattleLogPath))
            {
                sourcePath = BattleLogPath;
                lines.AddRange(File.ReadAllLines(BattleLogPath));
            }

            if (lines.Count == 0)
            {
                ShowMessage("No logs found to export.", true);
                return;
            }

            // Take only the most recent chunk to keep things light.
            int startIndex = System.Math.Max(0, lines.Count - 600);
            IEnumerable<string> recent = lines.Skip(startIndex);

            var errors = new List<string>();
            var warnings = new List<string>();
            var infos = new List<string>();

            foreach (string line in recent.Reverse())
            {
                string normalized = Regex.Replace(line, @"\s+", " ").Trim();
                if (string.IsNullOrEmpty(normalized))
                {
                    continue;
                }

                string lower = normalized.ToLowerInvariant();
                if (lower.Contains("exception") || lower.Contains("error"))
                {
                    errors.Add(normalized);
                }
                else if (lower.Contains("warn"))
                {
                    warnings.Add(normalized);
                }
                else
                {
                    infos.Add(normalized);
                }

                if (errors.Count + warnings.Count + infos.Count >= maxLines * 3)
                {
                    break;
                }
            }

            var condensed = new List<string>();
            foreach (string entry in errors)
            {
                if (condensed.Count >= maxLines) break;
                condensed.Add(entry);
            }
            foreach (string entry in warnings)
            {
                if (condensed.Count >= maxLines) break;
                condensed.Add(entry);
            }
            foreach (string entry in infos)
            {
                if (condensed.Count >= maxLines) break;
                condensed.Add(entry);
            }

            string exportText = string.Join("\n", condensed);
            string exportPath = RelevantLogExportPath;
            bool writeSucceeded = false;

#if UNITY_EDITOR
            // Prefer writing next to the project for easy access in Editor.
            string editorPath = Path.Combine(Application.dataPath, "..", "AdminRelevantLogs.txt");
            try
            {
                File.WriteAllText(editorPath, exportText);
                exportPath = editorPath;
                writeSucceeded = true;
                UnityEditor.EditorGUIUtility.systemCopyBuffer = exportText;
            }
            catch
            {
                // Fall back below.
            }
#endif

            if (!writeSucceeded)
            {
                try
                {
                    File.WriteAllText(exportPath, exportText);
                    writeSucceeded = true;
                }
                catch
                {
                    try
                    {
                        exportPath = Path.Combine(Path.GetTempPath(), "admin_relevant_logs.txt");
                        File.WriteAllText(exportPath, exportText);
                        writeSucceeded = true;
                    }
                    catch
                    {
                        exportPath = "write failed";
                    }
                }
            }

            string preview = condensed.Count > 0 ? string.Join(" | ", condensed.Take(3)) : "No condensed lines";
            ShowMessage($"Relevant logs ({condensed.Count} lines) from {sourcePath} -> {exportPath}\nPreview: {preview}");
        }

        /// <summary>
        /// Test cloud save operation with comprehensive status display
        /// </summary>
        public async void TestCloudSave()
        {
            ShowMessage("Testing Cloud Save...");
            
            // Capture initial state
            bool cloudSaveDisabled = DataProvider.SettingsManager.CloudSaveDisabled;
            bool unityServicesInitialized = UnityServices.State == ServicesInitializationState.Initialized;
            bool isAuthenticated = AuthenticationService.Instance != null && AuthenticationService.Instance.IsSignedIn;
            long coinsBefore = DataProvider.GameModel.Coins;
            long creditsBefore = DataProvider.GameModel.Credits;
            
            // Build status message
            StringBuilder status = new StringBuilder();
            status.AppendLine("=== CLOUD SAVE TEST ===");
            status.AppendLine($"Cloud Save Disabled: {(cloudSaveDisabled ? "Yes" : "No")}");
            status.AppendLine($"Unity Services: {(unityServicesInitialized ? "Initialized" : "Not Initialized")}");
            status.AppendLine($"Authenticated: {(isAuthenticated ? "Yes" : "No")}");
            status.AppendLine($"Before: Coins={coinsBefore}, Credits={creditsBefore}");
            
            // Attempt cloud save
            try
            {
                await DataProvider.CloudSave();
                
                long coinsAfter = DataProvider.GameModel.Coins;
                long creditsAfter = DataProvider.GameModel.Credits;
                
                status.AppendLine("Result: SUCCESS");
                status.AppendLine($"After: Coins={coinsAfter}, Credits={creditsAfter}");
                
                if (cloudSaveDisabled)
                {
                    status.AppendLine("Note: Cloud save disabled - saved locally only");
                }
                else if (!unityServicesInitialized || !isAuthenticated)
                {
                    status.AppendLine("Note: Cloud not ready - saved locally only");
                }
                else
                {
                    status.AppendLine("Note: Saved to cloud successfully");
                }
            }
            catch (System.Exception ex)
            {
                status.AppendLine($"Result: FAILED");
                status.AppendLine($"Error: {ex.Message}");
                status.AppendLine($"Stack: {ex.StackTrace}");
            }
            
            ShowMessage(status.ToString());
        }

        /// <summary>
        /// Test cloud load operation with comprehensive status display
        /// </summary>
        public async void TestCloudLoad()
        {
            ShowMessage("Testing Cloud Load...");
            
            // Capture initial state
            bool cloudSaveDisabled = DataProvider.SettingsManager.CloudSaveDisabled;
            bool unityServicesInitialized = UnityServices.State == ServicesInitializationState.Initialized;
            bool isAuthenticated = AuthenticationService.Instance != null && AuthenticationService.Instance.IsSignedIn;
            long coinsBefore = DataProvider.GameModel.Coins;
            long creditsBefore = DataProvider.GameModel.Credits;
            long lifetimeScoreBefore = DataProvider.GameModel.LifetimeDestructionScore;
            
            // Build status message
            StringBuilder status = new StringBuilder();
            status.AppendLine("=== CLOUD LOAD TEST ===");
            status.AppendLine($"Cloud Save Disabled: {(cloudSaveDisabled ? "Yes" : "No")}");
            status.AppendLine($"Unity Services: {(unityServicesInitialized ? "Initialized" : "Not Initialized")}");
            status.AppendLine($"Authenticated: {(isAuthenticated ? "Yes" : "No")}");
            status.AppendLine($"Before: Coins={coinsBefore}, Credits={creditsBefore}");
            status.AppendLine($"Local LifetimeScore: {lifetimeScoreBefore}");
            
            // Attempt cloud load
            try
            {
                if (cloudSaveDisabled)
                {
                    status.AppendLine("Result: SKIPPED (Cloud save disabled)");
                    status.AppendLine("After: No changes (cloud load skipped)");
                }
                else
                {
                    await DataProvider.CloudLoad();
                    
                    long coinsAfter = DataProvider.GameModel.Coins;
                    long creditsAfter = DataProvider.GameModel.Credits;
                    long lifetimeScoreAfter = DataProvider.GameModel.LifetimeDestructionScore;
                    
                    status.AppendLine("Result: SUCCESS");
                    status.AppendLine($"After: Coins={coinsAfter}, Credits={creditsAfter}");
                    status.AppendLine($"Local LifetimeScore: {lifetimeScoreAfter}");
                    
                    // Determine what happened
                    if (lifetimeScoreAfter > lifetimeScoreBefore)
                    {
                        status.AppendLine($"Cloud LifetimeScore: {lifetimeScoreBefore} → {lifetimeScoreAfter}");
                        status.AppendLine("Result: Cloud overwrote local");
                    }
                    else if (lifetimeScoreAfter < lifetimeScoreBefore)
                    {
                        status.AppendLine($"Cloud LifetimeScore: {lifetimeScoreAfter} < Local {lifetimeScoreBefore}");
                        status.AppendLine("Result: Local overwrote cloud");
                    }
                    else
                    {
                        status.AppendLine("Result: Scores equal - no overwrite");
                    }
                    
                    long coinsChange = coinsAfter - coinsBefore;
                    long creditsChange = creditsAfter - creditsBefore;
                    if (coinsChange != 0 || creditsChange != 0)
                    {
                        status.AppendLine($"Currency Change: Coins {(coinsChange >= 0 ? "+" : "")}{coinsChange}, Credits {(creditsChange >= 0 ? "+" : "")}{creditsChange}");
                    }
                }
            }
            catch (System.Exception ex)
            {
                status.AppendLine($"Result: FAILED");
                status.AppendLine($"Error: {ex.Message}");
                status.AppendLine($"Stack: {ex.StackTrace}");
            }
            
            ShowMessage(status.ToString());
        }

        #region Exos (Captains)

        /// <summary>
        /// Unlock all captain exos (0-50, where 0 is Charlie)
        /// </summary>
        public void UnlockExos()
        {
            ShowMessage("UNLOCKING ALL EXOS (Captains)...");
            
            int totalCaptains = StaticData.Captains.Count;
            int unlocked = 0;
            
            for (int i = 0; i < totalCaptains; i++)
            {
                if (!DataProvider.GameModel.PurchasedExos.Contains(i))
                {
                    DataProvider.GameModel.AddExo(i);
                    unlocked++;
                }
            }
            
            DataProvider.SaveGame();
            
            string captainName = StaticData.Captains[0].NameStringKeyBase;
            ShowMessage($"EXOS UNLOCKED: {unlocked} new captains added (total: {DataProvider.GameModel.PurchasedExos.Count}/{totalCaptains}). Current captain: {DataProvider.GameModel.PlayerLoadout.CurrentCaptain.PrefabName}");
        }

        /// <summary>
        /// Reset exos to just Charlie (captain index 0)
        /// </summary>
        public void ResetExos()
        {
            ShowMessage("RESETTING EXOS to Charlie only...");
            
            // Clear all exos
            List<int> currentExos = DataProvider.GameModel.PurchasedExos.ToList();
            foreach (int exoId in currentExos)
            {
                if (exoId != 0) // Keep Charlie (index 0)
                {
                    DataProvider.GameModel.RemoveExo(exoId);
                }
            }
            
            // Ensure Charlie is in the list
            if (!DataProvider.GameModel.PurchasedExos.Contains(0))
            {
                DataProvider.GameModel.AddExo(0);
            }
            
            // Reset current captain to Charlie (CaptainExo000)
            DataProvider.GameModel.PlayerLoadout.CurrentCaptain = new CaptainExoKey("CaptainExo000");
            
            DataProvider.SaveGame();
            
            ShowMessage($"EXOS RESET: Only Charlie remains. Current captain set to CaptainExo000. Total exos: {DataProvider.GameModel.PurchasedExos.Count}");
        }

        #endregion

        #region Heckles

        /// <summary>
        /// Unlock all heckles (0-278, 279 total)
        /// </summary>
        public void UnlockHeckles()
        {
            ShowMessage("UNLOCKING ALL HECKLES...");
            
            int totalHeckles = StaticData.Heckles.Count;
            int unlocked = 0;
            
            for (int i = 0; i < totalHeckles; i++)
            {
                if (!DataProvider.GameModel.PurchasedHeckles.Contains(i))
                {
                    DataProvider.GameModel.AddHeckle(i);
                    unlocked++;
                }
            }
            
            DataProvider.SaveGame();
            
            ShowMessage($"HECKLES UNLOCKED: {unlocked} new heckles added (total: {DataProvider.GameModel.PurchasedHeckles.Count}/{totalHeckles})");
        }

        /// <summary>
        /// Reset heckles to random 3 (like initial game state)
        /// </summary>
        public void ResetHeckles()
        {
            ShowMessage("RESETTING HECKLES to random 3...");
            
            // Clear all heckles
            List<int> currentHeckles = DataProvider.GameModel.PurchasedHeckles.ToList();
            foreach (int heckleId in currentHeckles)
            {
                DataProvider.GameModel.RemoveHeckle(heckleId);
            }
            
            // Generate 3 random unique heckles (same logic as Loadout.UnlockedHeckles())
            List<int> newHeckles = new List<int>();
            int numHecklesUnlocked = 3;
            int maxHeckleIndex = 279; // 0-278
            
            while (newHeckles.Count < numHecklesUnlocked)
            {
                int randomHeckle = Random.Range(0, maxHeckleIndex);
                if (!newHeckles.Contains(randomHeckle))
                {
                    newHeckles.Add(randomHeckle);
                    DataProvider.GameModel.AddHeckle(randomHeckle);
                }
            }
            
            // Update CurrentHeckles in loadout to use the new random heckles
            DataProvider.GameModel.PlayerLoadout.CurrentHeckles.Clear();
            DataProvider.GameModel.PlayerLoadout.CurrentHeckles.AddRange(newHeckles);
            
            DataProvider.SaveGame();
            
            ShowMessage($"HECKLES RESET: Random 3 heckles selected: [{string.Join(", ", newHeckles)}]. Total purchased: {DataProvider.GameModel.PurchasedHeckles.Count}");
        }

        #endregion

        #region Bodykits

        /// <summary>
        /// Unlock all bodykits (all except Trident Prototype for free edition, all for premium)
        /// Bodykit 0 = Trident Prototype (premium only, cost 999999)
        /// </summary>
        public void UnlockBodykits()
        {
            bool isPremium = DataProvider.GameModel.PremiumEdition;
            ShowMessage($"UNLOCKING BODYKITS... (Edition: {(isPremium ? "PREMIUM" : "FREE")})");
            
            int totalBodykits = StaticData.Bodykits.Count;
            int unlocked = 0;
            int skipped = 0;
            
            for (int i = 0; i < totalBodykits; i++)
            {
                // Skip Trident Prototype (index 0) for free edition
                if (i == 0 && !isPremium)
                {
                    skipped++;
                    continue;
                }
                
                if (!DataProvider.GameModel.PurchasedBodykits.Contains(i))
                {
                    DataProvider.GameModel.AddBodykit(i);
                    unlocked++;
                }
            }
            
            DataProvider.SaveGame();
            
            string skipMsg = skipped > 0 ? $" (Trident Prototype skipped - premium only)" : "";
            ShowMessage($"BODYKITS UNLOCKED: {unlocked} new bodykits added{skipMsg}. Total: {DataProvider.GameModel.PurchasedBodykits.Count}/{totalBodykits}");
        }

        /// <summary>
        /// Reset bodykits (none for free edition, just Trident Prototype for premium)
        /// </summary>
        public void ResetBodykits()
        {
            bool isPremium = DataProvider.GameModel.PremiumEdition;
            ShowMessage($"RESETTING BODYKITS... (Edition: {(isPremium ? "PREMIUM" : "FREE")})");
            
            // Clear all bodykits
            List<int> currentBodykits = DataProvider.GameModel.PurchasedBodykits.ToList();
            foreach (int bodykitId in currentBodykits)
            {
                DataProvider.GameModel.RemoveBodykit(bodykitId);
            }
            
            // For premium edition, add Trident Prototype (index 0)
            if (isPremium)
            {
                DataProvider.GameModel.AddBodykit(0);
            }
            
            // Reset selected bodykit to none (-1)
            DataProvider.GameModel.PlayerLoadout.SelectedBodykit = -1;
            
            DataProvider.SaveGame();
            
            string result = isPremium 
                ? "Trident Prototype (index 0) added. Selected bodykit reset to none."
                : "All bodykits removed. Selected bodykit reset to none.";
            ShowMessage($"BODYKITS RESET: {result} Total: {DataProvider.GameModel.PurchasedBodykits.Count}");
        }

        #endregion

        #region Variants

        /// <summary>
        /// Unlock all variants for buildings and units
        /// </summary>
        public void UnlockVariants()
        {
            ShowMessage("UNLOCKING ALL VARIANTS...");
            
            int totalVariants = StaticData.Variants.Count;
            int unlocked = 0;
            
            for (int i = 0; i < totalVariants; i++)
            {
                if (!DataProvider.GameModel.PurchasedVariants.Contains(i))
                {
                    DataProvider.GameModel.AddVariant(i);
                    unlocked++;
                }
            }
            
            DataProvider.SaveGame();
            
            ShowMessage($"VARIANTS UNLOCKED: {unlocked} new variants added (total: {DataProvider.GameModel.PurchasedVariants.Count}/{totalVariants})");
        }

        /// <summary>
        /// Reset variants (remove all purchased variants and clear selected variants)
        /// </summary>
        public void ResetVariants()
        {
            ShowMessage("RESETTING VARIANTS...");
            
            // Clear all purchased variants
            List<int> currentVariants = DataProvider.GameModel.PurchasedVariants.ToList();
            int removed = currentVariants.Count;
            
            foreach (int variantId in currentVariants)
            {
                DataProvider.GameModel.RemoveVariant(variantId);
            }
            
            // Clear selected variants in loadout
            DataProvider.GameModel.PlayerLoadout.SelectedVariants.Clear();
            
            DataProvider.SaveGame();
            
            ShowMessage($"VARIANTS RESET: {removed} variants removed. Selected variants cleared. Total: {DataProvider.GameModel.PurchasedVariants.Count}");
        }

        #endregion

        #region Battle Logging

        private static string BattleLogPath => Path.Combine(Application.persistentDataPath, "battle_log.txt");

        /// <summary>
        /// Log PvE battle values to a text file for debugging destruction scene simulations.
        /// Call from DestructionSceneGod.PopulateScreen() or Start().
        /// </summary>
        public static void LogPvEBattleValues()
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine($"=== PvE BATTLE LOG === {System.DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine($"Mode: {ApplicationModel.Mode}");
                sb.AppendLine($"SelectedLevel: {DataProvider.GameModel.SelectedLevel}");
                sb.AppendLine($"NumOfLevelsCompleted: {DataProvider.GameModel.NumOfLevelsCompleted}");
                sb.AppendLine();

                if (BattleSceneGod.deadBuildables != null)
                {
                    sb.AppendLine("--- deadBuildables ---");
                    foreach (var kvp in BattleSceneGod.deadBuildables)
                    {
                        sb.AppendLine($"  {kvp.Key}: Damage={kvp.Value.GetTotalDamageInCredits()}, Destroyed={kvp.Value.GetTotalDestroyed()}, Time={kvp.Value.GetPlayedTime():F2}s");
                    }
                }
                else
                {
                    sb.AppendLine("deadBuildables: NULL");
                }

                sb.AppendLine();
                sb.AppendLine($"enemyCruiserSprite: {(BattleSceneGod.enemyCruiserSprite != null ? BattleSceneGod.enemyCruiserSprite.name : "NULL")}");
                sb.AppendLine($"enemyCruiserName: {BattleSceneGod.enemyCruiserName ?? "NULL"}");
                sb.AppendLine();
                sb.AppendLine("--- Suggested SimulatePvEWin values ---");
                
                if (BattleSceneGod.deadBuildables != null)
                {
                    long aircraft = BattleSceneGod.deadBuildables.ContainsKey(TargetType.Aircraft) ? BattleSceneGod.deadBuildables[TargetType.Aircraft].GetTotalDamageInCredits() : 0;
                    long ships = BattleSceneGod.deadBuildables.ContainsKey(TargetType.Ships) ? BattleSceneGod.deadBuildables[TargetType.Ships].GetTotalDamageInCredits() : 0;
                    long cruiser = BattleSceneGod.deadBuildables.ContainsKey(TargetType.Cruiser) ? BattleSceneGod.deadBuildables[TargetType.Cruiser].GetTotalDamageInCredits() : 0;
                    long buildings = BattleSceneGod.deadBuildables.ContainsKey(TargetType.Buildings) ? BattleSceneGod.deadBuildables[TargetType.Buildings].GetTotalDamageInCredits() : 0;
                    float time = BattleSceneGod.deadBuildables.ContainsKey(TargetType.PlayedTime) ? BattleSceneGod.deadBuildables[TargetType.PlayedTime].GetPlayedTime() : 0;
                    
                    int aircraftCount = (int)(BattleSceneGod.deadBuildables.ContainsKey(TargetType.Aircraft) ? BattleSceneGod.deadBuildables[TargetType.Aircraft].GetTotalDestroyed() : 0);
                    int shipsCount = (int)(BattleSceneGod.deadBuildables.ContainsKey(TargetType.Ships) ? BattleSceneGod.deadBuildables[TargetType.Ships].GetTotalDestroyed() : 0);
                    int buildingsCount = (int)(BattleSceneGod.deadBuildables.ContainsKey(TargetType.Buildings) ? BattleSceneGod.deadBuildables[TargetType.Buildings].GetTotalDestroyed() : 0);
                    
                    sb.AppendLine($"  CreateDamageCounter({aircraft}, {aircraftCount}) // Aircraft");
                    sb.AppendLine($"  CreateDamageCounter({ships}, {shipsCount}) // Ships");
                    sb.AppendLine($"  CreateDamageCounter({cruiser}, 1) // Cruiser");
                    sb.AppendLine($"  CreateDamageCounter({buildings}, {buildingsCount}) // Buildings");
                    sb.AppendLine($"  CreateTimeCounter({time:F1}f) // PlayedTime");
                }

                sb.AppendLine();
                sb.AppendLine("========================================");
                sb.AppendLine();

                File.AppendAllText(BattleLogPath, sb.ToString());
                Debug.Log($"[AdminPanel] Battle values logged to: {BattleLogPath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[AdminPanel] Failed to log battle values: {e.Message}");
            }
        }

        /// <summary>
        /// Log PvP battle values to a text file for debugging destruction scene simulations.
        /// Call from PvPDestructionSceneGod.Start() after values are populated.
        /// </summary>
        public static void LogPvPBattleValues()
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine($"=== PvP BATTLE LOG === {System.DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine($"Mode: {ApplicationModel.Mode}");
                sb.AppendLine($"isDisconnected: {PvPBattleSceneGodTunnel.isDisconnected}");
                sb.AppendLine($"OpponentQuit: {PvPBattleSceneGodTunnel.OpponentQuit}");
                sb.AppendLine();

                sb.AppendLine("--- Tunnel Static Values ---");
                sb.AppendLine($"  _levelTimeInSeconds: {PvPBattleSceneGodTunnel._levelTimeInSeconds:F2}");
                sb.AppendLine($"  _aircraftVal: {PvPBattleSceneGodTunnel._aircraftVal}");
                sb.AppendLine($"  _shipsVal: {PvPBattleSceneGodTunnel._shipsVal}");
                sb.AppendLine($"  _cruiserVal: {PvPBattleSceneGodTunnel._cruiserVal}");
                sb.AppendLine($"  _buildingsVal: {PvPBattleSceneGodTunnel._buildingsVal}");
                sb.AppendLine($"  PlayerACruiserType: {PvPBattleSceneGodTunnel.PlayerACruiserType}");
                sb.AppendLine($"  PlayerBCruiserType: {PvPBattleSceneGodTunnel.PlayerBCruiserType}");
                sb.AppendLine($"  EnemyCruiserType: {PvPBattleSceneGodTunnel.EnemyCruiserType}");

                if (PvPBattleSceneGodTunnel._totalDestroyed != null)
                {
                    sb.AppendLine($"  _totalDestroyed: [{string.Join(", ", PvPBattleSceneGodTunnel._totalDestroyed)}]");
                }
                else
                {
                    sb.AppendLine("  _totalDestroyed: NULL");
                }

                sb.AppendLine();
                sb.AppendLine("--- Suggested SimulatePvPWin/Loss values ---");
                sb.AppendLine($"  PvPBattleSceneGodTunnel.isDisconnected = {PvPBattleSceneGodTunnel.isDisconnected};");
                sb.AppendLine($"  PvPBattleSceneGodTunnel._levelTimeInSeconds = {PvPBattleSceneGodTunnel._levelTimeInSeconds:F1}f;");
                sb.AppendLine($"  PvPBattleSceneGodTunnel._aircraftVal = {PvPBattleSceneGodTunnel._aircraftVal};");
                sb.AppendLine($"  PvPBattleSceneGodTunnel._shipsVal = {PvPBattleSceneGodTunnel._shipsVal};");
                sb.AppendLine($"  PvPBattleSceneGodTunnel._cruiserVal = {PvPBattleSceneGodTunnel._cruiserVal};");
                sb.AppendLine($"  PvPBattleSceneGodTunnel._buildingsVal = {PvPBattleSceneGodTunnel._buildingsVal};");
                
                if (PvPBattleSceneGodTunnel._totalDestroyed != null)
                {
                    sb.AppendLine($"  PvPBattleSceneGodTunnel._totalDestroyed = new long[4] {{ {string.Join(", ", PvPBattleSceneGodTunnel._totalDestroyed)} }};");
                }

                sb.AppendLine();
                sb.AppendLine("========================================");
                sb.AppendLine();

                File.AppendAllText(BattleLogPath, sb.ToString());
                Debug.Log($"[AdminPanel] PvP battle values logged to: {BattleLogPath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[AdminPanel] Failed to log PvP battle values: {e.Message}");
            }
        }

        /// <summary>
        /// Save AppLovin logs and clear the battle log file
        /// </summary>
        public void ClearBattleLog()
        {
            try
            {
                // First, save AppLovin logs for support debugging
                if (AppLovinLogCollector.Instance != null)
                {
                    AppLovinLogCollector.Instance.SaveLogToFile();
                    ShowMessage("AppLovin logs saved to Downloads!\nNow clearing battle log...");
                }
                
                // Then clear the battle log
                if (File.Exists(BattleLogPath))
                {
                    File.Delete(BattleLogPath);
                    ShowMessage($"Battle log cleared: {BattleLogPath}");
                }
                else
                {
                    ShowMessage("No battle log file found to clear.");
                }
            }
            catch (System.Exception e)
            {
                ShowMessage($"Failed to clear log: {e.Message}", true);
            }
        }

        /// <summary>
        /// Show the battle log file path
        /// </summary>
        public void ShowBattleLogPath()
        {
            ShowMessage($"Battle log path:\n{BattleLogPath}");
        }

        /// <summary>
        /// Force show rewarded ad offer for debugging
        /// </summary>
        public void ForceShowRewardedAdOffer()
        {
            var destructionGod = UnityEngine.Object.FindObjectOfType<BattleCruisers.Scenes.DestructionSceneGod>();
            if (destructionGod != null)
            {
                ShowMessage("Forcing rewarded ad offer...");
                destructionGod.GetType().GetMethod("ShowRewardedAdOffer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.Invoke(destructionGod, null);
            }
            else
            {
                ShowMessage("ERROR: No DestructionSceneGod found in scene");
            }
        }

        /// <summary>
        /// Force show rewarded ad button directly (bypass all conditions)
        /// </summary>
        public void ForceShowRewardedAdButton()
        {
            var destructionGod = UnityEngine.Object.FindObjectOfType<BattleCruisers.Scenes.DestructionSceneGod>();
            if (destructionGod != null)
            {
                var buttonField = destructionGod.GetType().GetField("rewardedAdButton", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var button = buttonField?.GetValue(destructionGod) as UnityEngine.GameObject;
                if (button != null)
                {
                    button.SetActive(true);
                    ShowMessage("Forced rewarded ad button to show");
                }
                else
                {
                    ShowMessage("ERROR: rewardedAdButton field not found or null");
                }
            }
            else
            {
                ShowMessage("ERROR: No DestructionSceneGod found in scene");
            }
        }

        /// <summary>
        /// Check AppLovinManager status
        /// </summary>
        public void CheckAppLovinStatus()
        {
            var appLovin = BattleCruisers.Ads.AppLovinManager.Instance;
            if (appLovin != null)
            {
                var status = $"AppLovin Status:\n";
                status += $"Initialized: {appLovin.GetType().GetField("isInitialized", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(appLovin) ?? "unknown"}\n";
                status += $"Interstitial Ready: {appLovin.IsInterstitialReady()}\n";
                status += $"Rewarded Ready: {appLovin.IsRewardedAdReady()}\n";
                status += $"Test Mode: {BattleCruisers.Ads.AdConfigManager.Instance?.IsTestMode() ?? false}\n";
                status += $"Ads Disabled: {BattleCruisers.Ads.AdConfigManager.Instance?.AdsDisabled ?? true}\n";
                status += $"Rewarded Ads Enabled: {BattleCruisers.Ads.AdConfigManager.Instance?.RewardedAdsEnabled ?? false}";
                ShowMessage(status);
            }
            else
            {
                ShowMessage("ERROR: AppLovinManager.Instance is null");
            }
        }
        
        /// <summary>
        /// Show detailed AppLovin debug info
        /// </summary>
        public void ShowAppLovinDebugInfo()
        {
            if (AppLovinManager.Instance == null)
            {
                ShowMessage("ERROR: AppLovinManager missing!", true);
                return;
            }

            string info = AppLovinManager.Instance.GetDebugInfo();
            ShowMessage($"=== APPLOVIN DEBUG ===\n{info}");
        }

        /// <summary>
        /// Test kill switch UI visibility
        /// </summary>
        public void TestKillSwitchUI()
        {
            var appLovin = BattleCruisers.Ads.AppLovinManager.Instance;
            if (appLovin != null)
            {
                // Use reflection to access the killSwitchCanvas field
                var canvasField = appLovin.GetType().GetField("killSwitchCanvas", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                var canvas = canvasField?.GetValue(appLovin) as UnityEngine.Canvas;
                
                if (canvas != null)
                {
                    bool wasActive = canvas.gameObject.activeSelf;
                    canvas.gameObject.SetActive(!wasActive);
                    
                    var textField = appLovin.GetType().GetField("killSwitchTimerText", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    var text = textField?.GetValue(appLovin) as UnityEngine.UI.Text;
                    if (text != null)
                    {
                        text.text = "TEST MODE - Kill switch visible?";
                    }
                    
                    ShowMessage($"Kill Switch UI: {(wasActive ? "Hidden" : "Shown")}\nCanvas SortingOrder: {canvas.sortingOrder}\nActive: {canvas.gameObject.activeSelf}\nEnabled: {canvas.enabled}");
                }
                else
                {
                    ShowMessage("ERROR: Kill switch canvas is NULL!");
                }
            }
            else
            {
                ShowMessage("ERROR: AppLovinManager.Instance is null");
            }
        }

        #endregion
    }
}
