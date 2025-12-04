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
        private int levelToUnlock = 1; // The highest level to unlock, adjustable in the Inspector

        [Header("Ad Testing")]
        [SerializeField]
        [Tooltip("Link FullScreenAdverts from ScreensScene hierarchy")]
        private FullScreenAdverts fullScreenAdverts;

        [Header("On-Screen Logging")]
        [SerializeField]
        [Tooltip("Optional UI Text element for on-screen messages")]
        private UnityEngine.UI.Text screenMessageText;
        
        private float messageDisplayTime = 5f;
        private float messageTimer = 0f;

        void Start()
        {
            Assert.IsNotNull(buttons);
        }

        void Update()
        {
            // Auto-hide on-screen message after timer expires
            if (messageTimer > 0)
            {
                messageTimer -= Time.deltaTime;
                if (messageTimer <= 0 && screenMessageText != null)
                {
                    screenMessageText.text = "";
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

            // Also show on local text if assigned
            if (screenMessageText != null)
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
            ShowMessage("Admin Panel OPENED - Debug tools available");
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
            ShowMessage("Ad counters RESET! Next battle will trigger ad check.");
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

            // Check if ads are disabled
            if (AdConfigManager.Instance != null && AdConfigManager.Instance.AdsDisabled)
            {
                ShowMessage("Ads are DISABLED via Remote Config. Cannot show ad.", true);
                return;
            }

            ResetAdCounters();
            
            string mode = "UNKNOWN";
            if (AdConfigManager.Instance != null)
            {
                mode = AdConfigManager.Instance.IsTestMode() ? "TEST MODE" : "PRODUCTION";
            }
            
            ShowMessage($"Forcing ad display... Mode: {mode}");
            fullScreenAdverts.ForceShowAd();
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
                statusMsg += "AdConfigManager: NOT FOUND!\n";
            }

            if (fullScreenAdverts != null)
            {
                statusMsg += $"Counter: {fullScreenAdverts.GetAdCounterStatus()}\n";
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
                ShowMessage("ERROR: AdConfigManager not found!", true);
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
                ShowMessage("ERROR: AdConfigManager not found!", true);
            }
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
