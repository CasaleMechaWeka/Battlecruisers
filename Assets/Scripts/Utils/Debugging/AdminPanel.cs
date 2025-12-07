using BattleCruisers.Ads;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using System.Collections.Generic;
using System.Linq;
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

        public void DvorakHotkeys()
        {
            ShowMessage("Setting Dvorak hotkeys...");
            
            HotkeysModel hotkeys = DataProvider.GameModel.Hotkeys;

            // Navigation
            hotkeys.PlayerCruiser = KeyCode.Semicolon;
            hotkeys.Overview = KeyCode.Q;
            hotkeys.EnemyCruiser = KeyCode.J;

            // Building categories
            hotkeys.Factories = KeyCode.A;
            hotkeys.Defensives = KeyCode.O;
            hotkeys.Offensives = KeyCode.E;
            hotkeys.Tacticals = KeyCode.U;
            hotkeys.Ultras = KeyCode.I;

            // Factories
            hotkeys.DroneStation = KeyCode.Quote;
            hotkeys.AirFactory = KeyCode.Comma;
            hotkeys.NavalFactory = KeyCode.Period;

            // Defensives
            hotkeys.ShipTurret = KeyCode.Quote;
            hotkeys.AirTurret = KeyCode.Comma;
            hotkeys.Mortar = KeyCode.Period;
            hotkeys.SamSite = KeyCode.P;
            hotkeys.TeslaCoil = KeyCode.Y;

            // Offensives
            hotkeys.Artillery = KeyCode.Quote;
            hotkeys.Railgun = KeyCode.Comma;
            hotkeys.RocketLauncher = KeyCode.Period;

            // Tacticals
            hotkeys.Shield = KeyCode.Quote;
            hotkeys.Booster = KeyCode.Comma;
            hotkeys.StealthGenerator = KeyCode.Period;
            hotkeys.SpySatellite = KeyCode.P;
            hotkeys.ControlTower = KeyCode.Y;

            // Ultras
            hotkeys.Deathstar = KeyCode.Quote;
            hotkeys.NukeLauncher = KeyCode.Comma;
            hotkeys.Ultralisk = KeyCode.Period;
            hotkeys.KamikazeSignal = KeyCode.P;
            hotkeys.Broadsides = KeyCode.Y;

            // Aircraft
            hotkeys.Bomber = KeyCode.Quote;
            hotkeys.Gunship = KeyCode.Comma;
            hotkeys.Fighter = KeyCode.Period;

            // Ships
            hotkeys.AttackBoat = KeyCode.Quote;
            hotkeys.Frigate = KeyCode.Comma;
            hotkeys.Destroyer = KeyCode.Period;
            hotkeys.Archon = KeyCode.P;

            DataProvider.SaveGame();
            ShowMessage("Dvorak hotkeys SET! Check settings.");
        }

        public void AddMoney()
        {
            long before = DataProvider.GameModel.Coins;
            DataProvider.GameModel.Coins += 1000;
            DataProvider.SaveGame();
            ShowMessage($"COINS: {before} → {DataProvider.GameModel.Coins} (+1000)");
        }

        public void RemoveMoney()
        {
            long before = DataProvider.GameModel.Coins;
            DataProvider.GameModel.Coins -= 1000;
            if (DataProvider.GameModel.Coins < 0)
            {
                DataProvider.GameModel.Coins = 0;
            }
            DataProvider.SaveGame();
            ShowMessage($"COINS: {before} → {DataProvider.GameModel.Coins} (-1000)");
        }

        public void Add5kOnBounty()
        {
            long before = DataProvider.GameModel.Bounty;
            DataProvider.GameModel.Bounty += 5000;
            DataProvider.SaveGame();
            ShowMessage($"BOUNTY: {before} → {DataProvider.GameModel.Bounty} (+5000)");
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
        /// Force show an ad immediately (for testing AppLovin MAX integration)
        /// </summary>
        public void ForceShowAd()
        {
            if (fullScreenAdverts == null)
            {
                ShowMessage("ERROR: FullScreenAdverts not linked! Assign in Inspector.", true);
                return;
            }

            // Check if AppLovinManager is missing
            if (AppLovinManager.Instance == null)
            {
                ShowMessage("ERROR: AppLovinManager missing! Add a GameObject with AppLovinManager component to LandingScene.unity as a child of SceneGod (uses DontDestroyOnLoad).", true);
                return;
            }

            // Check if AdConfigManager is missing
            if (AdConfigManager.Instance == null)
            {
                ShowMessage("ERROR: AdConfigManager missing! Add a GameObject with AdConfigManager component to LandingScene.unity as a child of SceneGod (uses DontDestroyOnLoad).", true);
                return;
            }

            // Check if ads are disabled
            if (AdConfigManager.Instance.AdsDisabled)
            {
                ShowMessage("Ads are DISABLED via Remote Config. Cannot show ad.", true);
                return;
            }

            ResetAdCounters();
            
            string mode = AdConfigManager.Instance.IsTestMode() ? "TEST" : "PRODUCTION";
            
            ShowMessage($"Resetting ad counters and attempting to show interstitial ad... Mode: {mode}");
            fullScreenAdverts.ForceShowAd();
            ShowMessage("Interstitial ad request sent. If no ad appears, check Mediation Debugger for fill status.");
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
            if (AppLovinManager.Instance == null)
            {
                ShowMessage("ERROR: AppLovinManager missing! Add a GameObject with AppLovinManager component to LandingScene.unity as a child of SceneGod (uses DontDestroyOnLoad).", true);
                return;
            }

            if (!AppLovinManager.Instance.IsRewardedAdReady())
            {
                ShowMessage("Rewarded ad not ready yet. Use 'Show Ad Status' and wait for Rewarded Ready: True.", true);
                return;
            }

            // Grant on reward callback
            System.Action rewardHandler = null;
            rewardHandler = () =>
            {
                GrantRewardedAdCurrency();
                AppLovinManager.Instance.OnRewardedAdRewarded -= rewardHandler;
            };

            AppLovinManager.Instance.OnRewardedAdRewarded += rewardHandler;

            var (coins, credits) = AdConfigManager.Instance?.GetRewardAmountsForPlayer() ?? (500, 4500);
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
        private void GrantRewardedAdCurrency()
        {
            // Get reward amounts based on first-time vs returning
            var (coins, credits) = AdConfigManager.Instance?.GetRewardAmountsForPlayer() ?? (500, 4500);

            // Store initial values
            long coinsBefore = DataProvider.GameModel.Coins;
            long creditsBefore = DataProvider.GameModel.Credits;

            AdDebugLogger.Instance?.Log($"[AdminPanel] Granting {coins} coins, {credits} credits");
            AdDebugLogger.Instance?.Log($"[AdminPanel] Before: Coins={coinsBefore}, Credits={creditsBefore}");

            // Mark as watched (only on first time)
            if (!AdConfigManager.HasEverWatchedRewardedAd())
            {
                AdConfigManager.MarkRewardedAdWatched();
                AdDebugLogger.Instance?.Log("[AdminPanel] Player marked as ADWATCHER");
            }

            // Grant rewards
            DataProvider.GameModel.Coins += coins;
            DataProvider.GameModel.Credits += credits;
            
            AdDebugLogger.Instance?.Log($"[AdminPanel] After: Coins={DataProvider.GameModel.Coins}, Credits={DataProvider.GameModel.Credits}");
            
            // Save game
            DataProvider.SaveGame();
            AdDebugLogger.Instance?.Log("[AdminPanel] Game saved");

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
            
            AdDebugLogger.Instance?.Log($"[AdminPanel] Economy Status - Coins: {coins}, Credits: {credits}");
            
            ShowMessage($"=== ECONOMY STATUS ===\nCoins: {coins}\nCredits: {credits}");
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
    }
}
