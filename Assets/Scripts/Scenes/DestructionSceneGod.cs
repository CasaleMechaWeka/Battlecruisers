using BattleCruisers.Buildables;
using BattleCruisers.Data;
using BattleCruisers.PostBattleScreen;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Ads;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Scenes
{
    public class DestructionSceneGod : MonoBehaviour
    {
        [SerializeField]
        private Text screenTitle;

        public DestructionCard[] destructionCards;
        public CanvasGroupButton nextButton;
        public CanvasGroupButton skipButton;
        [SerializeField]
        private AudioSource _uiAudioSource;
        private SingleSoundPlayer _soundPlayer;
        public Text million, billion, trillion, quadrillion;

        private long[] destructionValues;

        [SerializeField]
        private AnimationClip cardRevealAnim;
        [SerializeField]
        private TextMeshProUGUI damageCausedValueText; //<-- Damage totals are TextMeshPro for better formatting. Everything else could be too, but that takes effort.
        [SerializeField]
        private Text timeValueText;
        [SerializeField]
        private TextMeshProUGUI allTimeDamageValueText;
        [SerializeField]
        private Text scoreText;
        [SerializeField]
        private Text rankText;
        [SerializeField]
        private Text rankNumber;
        [SerializeField]
        private Image rankGraphic;
        [SerializeField]
        private Text modalOldRankText;
        [SerializeField]
        private Text modalNewRankText;
        [SerializeField]
        private Image modalOldRankGraphic;
        [SerializeField]
        private Image modalNewRankGraphic;

        // Damage values to interpolate damaged caused with:
        private long aircraftVal;
        private long shipsVal;
        private long cruiserVal;
        private long buildingsVal;
        private long allTimeVal;
        private long prevAllTimeVal;

        // Time value to interpolate with:
        private float levelTimeInSeconds;

        // Level and XP tracking:
        [SerializeField]
        private Slider levelBar;
        private int nextLevelXP;
        private int currentXP;
        private long levelScore;
        private int rank;
        public DestructionRanker ranker;

        [SerializeField]
        private GameObject levelUpModal;
        [SerializeField]
        private AnimationClip levelUpModalAnim;
        private float modalPeriod; // length of levelUpModalAnim

        private float timeStep; // used as the basis for all WaitForSeconds() returns
        private float stepPeriod;
        [SerializeField]
        private int steps;

        // values to control scores and rewards:
        private int scoreDivider;
        private int creditDivider;
        private int coin1Threshold;
        private int coin2Threshold;
        private int coin3Threshold;
        private int coin4Threshold;
        private int coin5Threshold;
        private int creditMax;

        // rewards panel parent
        [SerializeField]
        private GameObject rewardsCounter;

        // coins variables:
        private int coinsToAward;
        [SerializeField]
        private GameObject coinsCounter;
        [SerializeField]
        private Text coinsText;

        // credits variables:
        private long creditsToAward;
        [SerializeField]
        private GameObject creditsCounter;
        [SerializeField]
        private Text creditsText;

        // nukes variables:
        private int nukesToAward;
        [SerializeField]
        private GameObject nukesCounter;
        [SerializeField]
        private Text nukesText;

        // Rewarded ad variables:
        [Header("Rewarded Ads")]
        [SerializeField]
        private GameObject rewardedAdButton;
        [SerializeField]
        private Text rewardedAdButtonText;
        [SerializeField]
        private Text rewardedAdCoinsText; // Text object showing coins offer
        [SerializeField]
        private Text rewardedAdCreditsText; // Text object showing credits offer
        [SerializeField]
        private float rewardedAdOfferDuration = 5f; // How long to show the button
        
        private bool watchedRewardedAd = false;

        async void Start()
        {
            LandingSceneGod.MusicPlayer.PlayVictoryMusic();
            PrefabFactory.ClearPool();

            _soundPlayer
                = new SingleSoundPlayer(
                    new EffectVolumeAudioSource(
                        new AudioSourceBC(_uiAudioSource), 1));

            nextButton.Initialise(_soundPlayer, Done);
            skipButton.Initialise(_soundPlayer, SkipAnim);
            SceneNavigator.SceneLoaded(SceneNames.DESTRUCTION_SCENE);

            // Log battle values for debugging simulations
            BattleCruisers.Utils.Debugging.AdminPanel.LogPvEBattleValues();

            // Hide rewarded ad button by default
            if (rewardedAdButton != null)
            {
                rewardedAdButton.SetActive(false);
            }

            // Ensure AppLovinManager exists
            if (AppLovinManager.Instance == null)
            {
                Debug.LogWarning("[Rewards] AppLovinManager not found, creating one...");
                GameObject adsObj = new GameObject("AppLovinManager");
                adsObj.AddComponent<AppLovinManager>();
            }

            // Register rewarded ad callbacks
            if (AppLovinManager.Instance != null)
            {
                AppLovinManager.Instance.OnRewardedAdRewarded += OnRewardedAdCompleted;
                AppLovinManager.Instance.OnRewardedAdClosed += OnRewardedAdClosed;
                AppLovinManager.Instance.OnRewardedAdShowFailed += OnRewardedAdFailed;
            }

            // Populate screen:
            if (BattleSceneGod.deadBuildables != null)
            {
                // real values:

                StaticData.GameConfigs.TryGetValue("scoredivider", out scoreDivider);
                StaticData.GameConfigs.TryGetValue("creditdivider", out creditDivider);
                StaticData.GameConfigs.TryGetValue("coin1threshold", out coin1Threshold);
                StaticData.GameConfigs.TryGetValue("coin2threshold", out coin2Threshold);
                StaticData.GameConfigs.TryGetValue("coin3threshold", out coin3Threshold);
                StaticData.GameConfigs.TryGetValue("coin4threshold", out coin4Threshold);
                StaticData.GameConfigs.TryGetValue("coin5threshold", out coin5Threshold);
                StaticData.GameConfigs.TryGetValue("creditmax", out creditMax);

                PopulateScreen();
            }
            else
            {
                // fake values if the screen is being launched for testing purposes:

                scoreDivider = 10;
                creditDivider = 100;
                coin1Threshold = 1000;
                coin2Threshold = 2000;
                coin3Threshold = 3000;
                coin4Threshold = 4000;
                coin5Threshold = 5000;
                creditMax = 1250;

                PopulateScreenFake();
            }

            // Start animating:
            StartCoroutine(AnimateScreen());
        }

        /*        private void OnEnable()
                {
                    LandingSceneGod.SceneNavigator.SceneLoaded(SceneNames.DESTRUCTION_SCENE);
                }*/

        // Gets all the GameModel vars.
        // Turns them into local vars for frequently-used values,
        // Populates text fields on the screen with those values,
        // Calculates and awards any rewards.
        private void PopulateScreen()
        {
            // Get some values from GameModel and its friends:
            allTimeVal = DataProvider.GameModel.LifetimeDestructionScore;
            levelTimeInSeconds = BattleSceneGod.deadBuildables[TargetType.PlayedTime].GetPlayedTime();

            aircraftVal = BattleSceneGod.deadBuildables[TargetType.Aircraft].GetTotalDamageInCredits();
            shipsVal = BattleSceneGod.deadBuildables[TargetType.Ships].GetTotalDamageInCredits();
            cruiserVal = BattleSceneGod.deadBuildables[TargetType.Cruiser].GetTotalDamageInCredits();
            buildingsVal = BattleSceneGod.deadBuildables[TargetType.Buildings].GetTotalDamageInCredits();

            // this seemed like the easiest way to store the values, so their indices match the destructionCards array:
            destructionValues = new long[] { aircraftVal, shipsVal, cruiserVal, buildingsVal };

            for (int i = 0; i < destructionCards.Length; i++)
            {
                destructionCards[i].destructionValue.text = FormatNumber(destructionValues[i]);
                destructionCards[i].numberOfUnitsDestroyed.text = i == 2 ? "1" : "" + BattleSceneGod.deadBuildables[(TargetType)i].GetTotalDestroyed();
            }

            destructionCards[2].image.sprite = BattleSceneGod.enemyCruiserSprite;
            // destructionCards[2].description.text = BattleSceneGod.enemyCruiserName;

            //### Screen Setup ###

            // Turn cards off by default:
            for (int i = 0; i < destructionCards.Length; i++)
            {
                destructionCards[i].gameObject.SetActive(false);
            }

            // Turn off Rewards Counter by default:
            rewardsCounter.SetActive(false);

            // Turn off Level Up Modal by default:
            levelUpModal.SetActive(false);

            timeStep = cardRevealAnim.length;
            modalPeriod = levelUpModalAnim.length;

            // Set value texts:
            damageCausedValueText.text = "0";
            timeValueText.text = "00:00";
            prevAllTimeVal = allTimeVal - (aircraftVal + shipsVal + cruiserVal + buildingsVal);
            allTimeDamageValueText.text = FormatNumber(prevAllTimeVal);
            scoreText.text = "";

            // Set starting rank values:
            rank = DestructionRanker.CalculateRank(prevAllTimeVal);

            currentXP = (int)DestructionRanker.CalculateXpToNextLevel(prevAllTimeVal);
            nextLevelXP = (int)DestructionRanker.CalculateLevelXP(rank);
            rankNumber.text = FormatRankNumber(rank);
            rankText.text = ranker.destructionRanks[rank].transform.Find("RankNameText").GetComponent<Text>().text; // UGLY looking Find + Get
            rankGraphic.sprite = ranker.destructionRanks[rank].transform.Find("RankImage").GetComponent<Image>().sprite; // UGLY looking Find + Get

            if (LandingSceneGod.Instance.coinBattleLevelNum == -2)
            {
                ApplicationModel.Mode = GameMode.CoinBattle;
                LandingSceneGod.Instance.coinBattleLevelNum = -1;
            }

            // Campaign specific reward handling; only reward on first completion:
            if (ApplicationModel.Mode == GameMode.Campaign && DataProvider.GameModel.SelectedLevel == DataProvider.GameModel.NumOfLevelsCompleted + 1)
            {
                CalculateRewards();
            }
            // Everything else:
            else if (ApplicationModel.Mode != GameMode.Skirmish && ApplicationModel.Mode != GameMode.Campaign)
            {
                CalculateRewards();
            }

            // Set XP bar current/max values:
            if (DestructionRanker.CalculateRank(allTimeVal) == ranker.destructionRanks.Length - 1)
            {
                levelBar.maxValue = 1;
                levelBar.value = 1;
            }
            else
            {
                levelBar.maxValue = nextLevelXP;
                levelBar.value = currentXP;
            }

            // From here on out, the screen shouldn't be needing to GET any GameModel variables,
            // so we can give the player all their points and coins now.
            // That way if there's a crash or anything before the animation completes, they still get credit.
            UpdateGameModelVals();

            skipButton.gameObject.SetActive(true);
        }

        // Duplicate of PopulateScreen(), but with fake numbers.
        // Turns them into local vars for frequently-used values,
        // Populates text fields on the screen with those values.
        // This should not be displayed to real users.
        // DOES NOT awards any rewards.
        private void PopulateScreenFake()
        {
            long randomVal = 10000;

            allTimeVal = randomVal;
            levelTimeInSeconds = UnityEngine.Random.Range(300, 600);
            aircraftVal = randomVal / UnityEngine.Random.Range(4, 6);
            shipsVal = randomVal / UnityEngine.Random.Range(4, 6);
            cruiserVal = randomVal / UnityEngine.Random.Range(4, 6);
            buildingsVal = randomVal / UnityEngine.Random.Range(4, 6);

            nukesToAward = UnityEngine.Random.Range(0, 10);

            // this seemed like the easiest way to store the values, so their indices match the destructionCards array:
            destructionValues = new long[] { aircraftVal, shipsVal, cruiserVal, buildingsVal };

            for (int i = 0; i < destructionCards.Length; i++)
            {
                destructionCards[i].destructionValue.text = FormatNumber(destructionValues[i]);
                destructionCards[i].numberOfUnitsDestroyed.text = i == 2 ? "1" : "" + UnityEngine.Random.Range(1, 100).ToString();
            }
            //### Screen Setup ###

            // Turn cards off by default:
            for (int i = 0; i < destructionCards.Length; i++)
            {
                destructionCards[i].gameObject.SetActive(false);
            }

            // Turn off Rewards Counter by default:
            rewardsCounter.SetActive(false);

            // Turn off Level Up Modal by default:
            levelUpModal.SetActive(false);

            timeStep = cardRevealAnim.length;
            modalPeriod = levelUpModalAnim.length;

            // Set value texts:
            damageCausedValueText.text = "0";
            timeValueText.text = "00:00";
            prevAllTimeVal = allTimeVal - (aircraftVal + shipsVal + cruiserVal + buildingsVal);
            allTimeDamageValueText.text = FormatNumber(prevAllTimeVal);
            scoreText.text = "";

            // Set starting rank values:
            rank = DestructionRanker.CalculateRank(prevAllTimeVal);

            currentXP = (int)DestructionRanker.CalculateXpToNextLevel(prevAllTimeVal);
            nextLevelXP = (int)DestructionRanker.CalculateLevelXP(rank);
            rankNumber.text = FormatRankNumber(rank);
            rankText.text = ranker.destructionRanks[rank].transform.Find("RankNameText").GetComponent<Text>().text; // UGLY looking Find + Get
            rankGraphic.sprite = ranker.destructionRanks[rank].transform.Find("RankImage").GetComponent<Image>().sprite; // UGLY looking Find + Get

            CalculateRewards();

            screenTitle.text = "Debug Mode";
        }

        IEnumerator AnimateScreen()
        {
            yield return new WaitForSeconds(1.0f);

            // Enable a card, interpolate its total into the Damage Total, add XP to bar, repeat
            long damageRunningTotal = 0;

            for (int i = 0; i < destructionCards.Length; i++)
            {
                // Enable a card (playing its animation in the process):
                destructionCards[i].gameObject.SetActive(true);

                // Interpolate the Damage Caused value, from the current running total to that + the card's damage value
                // by the specified number of steps. Steps are divided over time:
                yield return StartCoroutine(InterpolateDamageValue(damageRunningTotal, damageRunningTotal + destructionValues[i], steps));
                damageRunningTotal += destructionValues[i];
                //yield return new WaitForSeconds(timeStep); // wait for destruction card reveal anim to finish before proceeding

                // Increase XP counter:

                // TODO: Check if it's max rank already

                int xpToAdd = Convert.ToInt32(destructionValues[i]);
                int xpRunningTotal = currentXP;

                // If the bar would fill up, it needs some special handling.
                if (xpToAdd + currentXP >= nextLevelXP)
                {
                    // Only deal with it if the player isn't max rank:
                    if (DestructionRanker.CalculateRank(allTimeVal) < ranker.destructionRanks.Length - 1)
                    {
                        while (xpToAdd > 0)
                        {
                            if (xpToAdd + currentXP > nextLevelXP)
                            {
                                yield return StartCoroutine(InterpolateXPBar(xpRunningTotal, nextLevelXP, steps, stepPeriod));

                                // Update rank text elements (on screen and in following modal):
                                rank++;
                                // in modal
                                string oldRankText = rankText.text;
                                string newRankText = ranker.destructionRanks[rank].transform.Find("RankNameText").GetComponent<Text>().text; // UGLY looking Find + Get
                                Sprite oldRankImage = rankGraphic.sprite;
                                Sprite newRankImage = ranker.destructionRanks[rank].transform.Find("RankImage").GetComponent<Image>().sprite; // UGLY looking Find + Get
                                modalOldRankText.text = oldRankText;
                                modalNewRankText.text = newRankText;
                                modalOldRankGraphic.sprite = oldRankImage;
                                modalNewRankGraphic.sprite = newRankImage;

                                yield return StartCoroutine(DisplayRankUpModal(modalPeriod));

                                // in screen
                                rankText.text = newRankText;
                                rankNumber.text = FormatRankNumber(rank);
                                rankGraphic.sprite = newRankImage;

                                xpToAdd -= (nextLevelXP - xpRunningTotal);
                                xpRunningTotal = 0;
                                currentXP = 0;

                                // Get the next level's XP and overwrite the nextLevelXP var
                                rank++;
                                nextLevelXP = (int)DestructionRanker.CalculateLevelXP(rank);
                                levelBar.maxValue = nextLevelXP;
                            }
                            else
                            {
                                // finish the while loop by interp'ing the remaining overflow XP:
                                yield return StartCoroutine(InterpolateXPBar(xpRunningTotal, xpRunningTotal + xpToAdd, steps, stepPeriod));
                                xpRunningTotal += xpToAdd;
                                currentXP = xpRunningTotal;
                                xpToAdd = 0;
                            }
                        }
                    }
                    else
                    {
                        levelBar.maxValue = 1;
                        levelBar.value = 1;
                        // TODO: any extra handling for max rank.
                    }
                }
                else
                {
                    yield return StartCoroutine(InterpolateXPBar(xpRunningTotal, xpRunningTotal + xpToAdd, steps, stepPeriod));
                    currentXP += xpToAdd;
                }
            }

            // Interpolate time counter:
            yield return StartCoroutine(InterpolateTimeValue(0, levelTimeInSeconds, (int)Mathf.Clamp(steps * 2, 1, Mathf.Infinity)));

            // Interpolate game score:
            levelScore = CalculateScore(levelTimeInSeconds, Convert.ToInt32(aircraftVal + shipsVal + cruiserVal + buildingsVal));
            yield return StartCoroutine(InterpolateScore(0, levelScore, steps));

            skipButton.gameObject.SetActive(false);

            // Award any rewards:
            if (ApplicationModel.Mode != GameMode.Skirmish)
            {
                if (coinsToAward > 0 || creditsToAward > 0 || nukesToAward > 0)
                {
                    rewardsCounter.SetActive(true);
                }
            }

            // Interpolate Lifetime Damage (same deal as regular damage)
            yield return StartCoroutine(InterpolateLifetimeDamageValue(prevAllTimeVal, allTimeVal, (int)Mathf.Clamp(steps / 3, 1, Mathf.Infinity)));
        }

        private void SkipAnim()
        {
            skipButton.gameObject.SetActive(false);

            timeStep = 0.0f;
            stepPeriod = 0.0f;
            steps = 1;

            for (int i = 0; i < destructionCards.Length; i++)
            {
                GameObject card = destructionCards[i].gameObject;
                Animator anim = card.GetComponent<Animator>();
                card.SetActive(true);
                anim.Play("DestructionCard", 0, 1);
            }
        }

        IEnumerator InterpolateDamageValue(long startVal, long endVal, int steps)
        {
            long interpStep = (endVal - startVal) / steps;
            float stepPeriod = timeStep / steps;

            for (int i = 1; i <= steps; i++)
            {
                startVal += interpStep;
                damageCausedValueText.text = FormatNumber(startVal);
                yield return new WaitForSeconds(stepPeriod);
            }
        }

        IEnumerator InterpolateLifetimeDamageValue(long startVal, long endVal, int steps)
        {
            long interpStep = (endVal - startVal) / steps;
            float stepPeriod = timeStep / steps;

            for (int i = 1; i <= steps; i++)
            {
                startVal += interpStep;
                allTimeDamageValueText.text = FormatNumber(startVal);
                yield return new WaitForSeconds(stepPeriod);
            }
        }

        IEnumerator InterpolateTimeValue(float startVal, float endVal, int steps)
        {
            float interpStep = (endVal - startVal) / steps;
            float stepPeriod = (timeStep * 2.0f) / steps; // timestamps look a bit nicer if they interp a bit slower

            for (int i = 1; i <= steps; i++)
            {
                startVal += interpStep;
                timeValueText.text = FormatTime(startVal);
                yield return new WaitForSeconds(stepPeriod);
            }
        }

        IEnumerator InterpolateXPBar(float startVal, float endVal, int steps, float stepPeriod)
        {
            float interpStep = (endVal - startVal) / steps;

            for (int i = 1; i <= steps; i++)
            {
                startVal += interpStep;
                levelBar.value = startVal;
                yield return new WaitForSeconds(stepPeriod);
            }
        }

        IEnumerator InterpolateScore(long startVal, long endVal, int steps)
        {
            long interpStep = (endVal - startVal) / steps;
            float stepPeriod = timeStep / steps;

            for (int i = 1; i <= steps; i++)
            {
                startVal += interpStep;
                scoreText.text = startVal.ToString();
                yield return new WaitForSeconds(stepPeriod);
            }
        }

        IEnumerator DisplayRankUpModal(float stepPeriod)
        {
            // Display modal
            levelUpModal.SetActive(true);
            yield return new WaitForSeconds(stepPeriod);
            levelUpModal.SetActive(false);
        }

        private long CalculateScore(float time, long damage)
        {
            // feels weird to make this a method but I don't like doing it directly in the animation methods:
            long score = 0;
            if (damage > 0)
            {
                long divider = ((long)(time * 250) / scoreDivider);
                if (divider < 1)
                    divider = 1;
                score = (damage * 1000) / divider;
            }
            return score;
        }

        private void CalculateRewards()
        {
            // Calculate base rewards (always shown, regardless of ad)
            coinsToAward = CalculateCoins(CalculateScore(levelTimeInSeconds, (aircraftVal + shipsVal + cruiserVal + buildingsVal)));
            creditsToAward = CalculateCredits();

            // Update UI for base rewards
            if (coinsToAward > 0)
            {
                coinsCounter.SetActive(true);
                coinsText.text = "+" + coinsToAward.ToString();
            }
            else
            {
                coinsCounter.SetActive(false);
            }

            if (creditsToAward > 0)
            {
                creditsCounter.SetActive(true);
                creditsText.text = "+" + creditsToAward.ToString();
            }
            else
            {
                creditsCounter.SetActive(false);
            }

            nukesToAward = CalculateNukes();
            if (nukesToAward > 0)
            {
                nukesCounter.SetActive(true);
                nukesText.text = "+" + nukesToAward.ToString();
            }
            else
            {
                nukesCounter.SetActive(false);
            }

            // Show rewarded ad offer if rewards exist and ad is available
            Debug.Log($"[Rewards] CalculateRewards: watchedRewardedAd={watchedRewardedAd}, coinsToAward={coinsToAward}, creditsToAward={creditsToAward}");
            if (!watchedRewardedAd && (coinsToAward > 0 || creditsToAward > 0))
            {
                Debug.Log("[Rewards] Calling ShowRewardedAdOffer()");
                ShowRewardedAdOffer();
            }
            else
            {
                Debug.Log("[Rewards] NOT calling ShowRewardedAdOffer() - condition failed");
            }
        }

        private int CalculateCoins(long score)
        {
            // 5 coins
            if (score >= coin5Threshold)
            {
                return 5;
            }
            // 4 coins
            if (score >= coin4Threshold)
            {
                return 4;
            }
            // 3 coins
            if (score >= coin3Threshold)
            {
                return 3;
            }
            // 2 coins
            else if (score >= coin2Threshold)
            {
                return 2;
            }
            // 1 coin
            else if (score >= coin1Threshold)
            {
                return 1;
            }

            return 0;
        }

        private long CalculateCredits()
        {
            long creditsAward = (aircraftVal + shipsVal + cruiserVal + buildingsVal) / creditDivider;
            if (creditsAward > creditMax)
            {
                return (long)creditMax;
            }
            return creditsAward;
        }

        private int CalculateNukes()
        {
            // Nuke Calculation Goes Here?



            return 0;
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape)
                || Input.GetKeyUp(KeyCode.Space)
                || Input.GetKeyUp(KeyCode.Return))
            {
                Done();
            }
        }

        private async void UpdateGameModelVals()
        {
            // Update GameModel vars
            // lifetime damage (this value is all we need for rank image/titles elsewhere):
            if (ApplicationModel.Mode != GameMode.Skirmish)//update the gamemodel if the game mode is not skirmish
            {
                long destructionScore = aircraftVal + shipsVal + cruiserVal + buildingsVal;
                DataProvider.GameModel.LifetimeDestructionScore = allTimeVal;

                // we need XPToNextLevel to populate any XP progress bars:
                long newLifetimeScore = DataProvider.GameModel.LifetimeDestructionScore;
                Debug.Log(DataProvider.GameModel.LifetimeDestructionScore);

                // Give the player their rewards:
                DataProvider.GameModel.Coins += coinsToAward;
                DataProvider.GameModel.Credits += creditsToAward;
                DataProvider.SaveGame();
                //DataProvider.GameModel.Nukes += nukesToAward; <--- This does not exist right now.


                if (await LandingSceneGod.CheckForInternetConnection())
                {
                    try
                    {
                        await DataProvider.SyncCoinsToCloud();
                        await DataProvider.SyncCreditsToCloud();

                        // Save changes:
                        await DataProvider.CloudSave();
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex);
                    }
                }
                else
                {
                    // Can't sync, save for later:
                    DataProvider.GameModel.CoinsChange += coinsToAward;
                    DataProvider.GameModel.CreditsChange += (int)creditsToAward;
                }
            }

        }

        private void Done()
        {
            // and now we actually are done:
            SceneNavigator.GoToScene(SceneNames.SCREENS_SCENE, false);
        }

        //taken from https://stackoverflow.com/questions/30180672/string-format-numbers-to-millions-thousands-with-rounding
        private string FormatNumber(long num)
        {
            if (num > 0)
            {
                num = num * 1000;
                long i = (long)Math.Pow(10, (int)Math.Max(0, Math.Log10(num) - 2));
                num = num / i * i;
                if (num >= 1000000000000)
                    return "$" + (num / 1000000000000D).ToString("0.##") + " " + quadrillion.text;
                if (num >= 1000000000)
                    return "$" + (num / 1000000000D).ToString("0.##") + " " + trillion.text;
                if (num >= 1000000)
                    return "$" + (num / 1000000D).ToString("0.##") + " " + billion.text;
                if (num >= 1000)
                    return "$" + (num / 1000D).ToString("0.##") + " " + million.text;

                return "$" + num.ToString("#,0");
            }
            else
            {
                return "$0";
            }
        }

        private string FormatRankNumber(int rank)
        {
            string numString = rank.ToString();
            if (rank < 10)
            {
                numString = "0" + rank.ToString();
            }
            return numString;
        }

        private string FormatTime(float num)
        {
            TimeSpan time = TimeSpan.FromSeconds(num);

            // less than an hour (filtering these values is probably not necessary but WHO KNOWS):
            if (num <= 3659.0f)
            {
                return time.ToString("mm':'ss");
            }
            // less than 23:59:59, the maximum of the clock:
            else if (num > 3659.0f && num <= 86399.0f)
            {
                return time.ToString("hh':'mm':'ss");
            }
            else
            {
                return "Owwww";
            }
        }

        // ========== REWARDED AD METHODS ==========

        private void ShowRewardedAdOffer()
        {
            Debug.Log("[Rewards] ShowRewardedAdOffer() called");

            // Check if rewarded ads are enabled
            if (AdConfigManager.Instance != null && !AdConfigManager.Instance.RewardedAdsEnabled)
            {
                Debug.Log($"[Rewards] Rewarded ads disabled via Remote Config (RewardedAdsEnabled={AdConfigManager.Instance.RewardedAdsEnabled})");
                return;
            }
            Debug.Log("[Rewards] Rewarded ads enabled");

            if (rewardedAdButton == null)
            {
                Debug.Log("[Rewards] rewardedAdButton is null - not assigned in scene");
                return;
            }
            Debug.Log("[Rewards] rewardedAdButton is assigned");

            // Check if ad is ready (for free players)
            bool isPremium = DataProvider.GameModel.PremiumEdition;
            Debug.Log($"[Rewards] isPremium={isPremium}");

            if (!isPremium && (AppLovinManager.Instance == null || !AppLovinManager.Instance.IsRewardedAdReady()))
            {
                Debug.Log($"[Rewards] Rewarded ad not available: AppLovinManager.Instance={AppLovinManager.Instance}, IsRewardedAdReady={(AppLovinManager.Instance?.IsRewardedAdReady() ?? false)}");
                return;
            }
            Debug.Log("[Rewards] Ad is available (premium or ready)");

            // Get reward amounts based on first-time vs returning player
            var (offerCoins, offerCredits) = AdConfigManager.Instance?.GetRewardAmountsForPlayer() ?? (500, 4500);

            // Update UI text objects
            if (rewardedAdCoinsText != null)
            {
                rewardedAdCoinsText.text = $"+{offerCoins}";
            }
            if (rewardedAdCreditsText != null)
            {
                rewardedAdCreditsText.text = $"+{offerCredits}";
            }

            // Update button text
            if (rewardedAdButtonText != null)
            {
                rewardedAdButtonText.text = $"Watch Ad";
            }

            // Show button
            rewardedAdButton.SetActive(true);
            Debug.Log("[Rewards] Set rewardedAdButton active");

            // Log to Firebase
            if (FirebaseAnalyticsManager.Instance != null)
            {
                FirebaseAnalyticsManager.Instance.LogRewardedAdOffered("destruction_screen", offerCoins, offerCredits);
            }

            // Start timer to hide button
            StartCoroutine(HideRewardedAdOfferAfterDelay());

            Debug.Log($"[Rewards] Offering rewarded ad: +{offerCoins} coins, +{offerCredits} credits (Status: {(AdConfigManager.HasEverWatchedRewardedAd() ? "ADWATCHER" : "VIRGIN")})");
        }

        private IEnumerator HideRewardedAdOfferAfterDelay()
        {
            Debug.Log($"[Rewards] Starting hide timer: {rewardedAdOfferDuration}s");
            yield return new WaitForSeconds(rewardedAdOfferDuration);

            Debug.Log($"[Rewards] Hide timer expired. watchedRewardedAd={watchedRewardedAd}, button exists={rewardedAdButton != null}, activeSelf={rewardedAdButton?.activeSelf ?? false}");

            if (!watchedRewardedAd && rewardedAdButton != null && rewardedAdButton.activeSelf)
            {
                rewardedAdButton.SetActive(false);
                Debug.Log("[Rewards] Hid rewarded ad button due to timer expiry");

                // Log skip to Firebase
                if (FirebaseAnalyticsManager.Instance != null)
                {
                    FirebaseAnalyticsManager.Instance.LogRewardedAdSkipped("destruction_screen");
                }

                Debug.Log("[Rewards] Rewarded ad offer expired");
            }
            else
            {
                Debug.Log("[Rewards] Not hiding button - either already watched, button null, or already hidden");
            }
        }

        /// <summary>
        /// Called when user clicks the rewarded ad button
        /// </summary>
        public void OnWatchRewardedAdButtonClicked()
        {
            bool isPremium = DataProvider.GameModel.PremiumEdition;

            if (isPremium)
            {
                // Premium: Show joke ad, then grant reward
                ShowJokeAdAndGrantReward();
            }
            else
            {
                // Free: Show real AppLovin ad
                if (AppLovinManager.Instance == null)
                {
                    Debug.LogWarning("[Rewards] AppLovinManager not found, creating one...");
                    GameObject adsObj = new GameObject("AppLovinManager");
                    adsObj.AddComponent<AppLovinManager>();
                    
                    // Wait for next frame to let it initialize, then try again
                    StartCoroutine(RetryShowRewardedAdAfterInit());
                    return;
                }

                if (!AppLovinManager.Instance.IsRewardedAdReady())
                {
                    Debug.LogWarning("[Rewards] Rewarded ad not ready");
                    
                    // Hide button since ad isn't available
                    if (rewardedAdButton != null)
                    {
                        rewardedAdButton.SetActive(false);
                    }
                    return;
                }

                // Hide button immediately
                if (rewardedAdButton != null)
                {
                    rewardedAdButton.SetActive(false);
                }

                // Log ad start to Firebase
                if (FirebaseAnalyticsManager.Instance != null)
                {
                    FirebaseAnalyticsManager.Instance.LogRewardedAdStarted("destruction_screen");
                }

                // Show the real ad
                AppLovinManager.Instance.ShowRewardedAd();
                Debug.Log("[Rewards] Showing rewarded ad");
            }
        }

        /// <summary>
        /// Show joke/fallback ad for premium players, then grant reward
        /// </summary>
        private void ShowJokeAdAndGrantReward()
        {
            Debug.Log("[Rewards] Premium player - showing joke ad");
            
            // Hide button
            if (rewardedAdButton != null)
            {
                rewardedAdButton.SetActive(false);
            }

            // Find FullScreenAdverts to show joke ad
            FullScreenAdverts fullScreenAdverts = FindObjectOfType<FullScreenAdverts>();
            if (fullScreenAdverts != null && fullScreenAdverts.defaultAd != null)
            {
                // Show joke ad panel
                fullScreenAdverts.defaultAd.UpdateImage();
                fullScreenAdverts.gameObject.SetActive(true);
                
                // Set up callback to grant reward when joke ad is closed
                // Note: FullScreenAdverts.CloseAdvert() will be called when user closes joke ad
                // We'll grant reward immediately since premium players don't need to watch
                GrantRewardedAdReward();
            }
            else
            {
                // Fallback: grant reward immediately if joke ad system not available
                Debug.LogWarning("[Rewards] FullScreenAdverts not found, granting reward directly");
                GrantRewardedAdReward();
            }
        }

        /// <summary>
        /// Grant the rewarded ad reward to player (additional on top of base rewards)
        /// </summary>
        private async void GrantRewardedAdReward()
        {
            var (coins, credits) = AdConfigManager.Instance?.GetRewardAmountsForPlayer() ?? (500, 4500);

            // Mark as watched (only on first time)
            if (!AdConfigManager.HasEverWatchedRewardedAd())
            {
                AdConfigManager.MarkRewardedAdWatched();
                Debug.Log("[Rewards] Player marked as ADWATCHER (was VIRGIN)");
            }

            // Grant additional rewards (base rewards already granted in UpdateGameModelVals)
            DataProvider.GameModel.Coins += coins;
            DataProvider.GameModel.Credits += credits;

            // Save locally first
            DataProvider.SaveGame();

            // CRITICAL: Sync rewarded currency changes to cloud immediately to prevent CloudLoad from overwriting
            try
            {
                await DataProvider.SyncCoinsToCloud();
                await DataProvider.SyncCreditsToCloud();
                Debug.Log("[Rewards] Currency synced to cloud");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Rewards] Failed to sync to cloud: {e.Message}");
            }
            
            // Update UI to show new totals (base + ad reward)
            if (coinsCounter != null)
            {
                coinsCounter.SetActive(true);
                coinsText.text = "+" + (coinsToAward + coins).ToString();
            }
            if (creditsCounter != null)
            {
                creditsCounter.SetActive(true);
                creditsText.text = "+" + (creditsToAward + credits).ToString();
            }
            
            Debug.Log($"[Rewards] Granted {coins} coins, {credits} credits from ad (Base: {coinsToAward} coins, {creditsToAward} credits | Total: {coinsToAward + coins} coins, {creditsToAward + credits} credits)");
            
            // Mark as watched for this session
            watchedRewardedAd = true;
        }

        private IEnumerator RetryShowRewardedAdAfterInit()
        {
            yield return new WaitForSeconds(0.5f);
            
            if (AppLovinManager.Instance != null)
            {
                // Register callbacks
                AppLovinManager.Instance.OnRewardedAdRewarded += OnRewardedAdCompleted;
                AppLovinManager.Instance.OnRewardedAdClosed += OnRewardedAdClosed;
                AppLovinManager.Instance.OnRewardedAdShowFailed += OnRewardedAdFailed;
                
                // Try showing ad again
                OnWatchRewardedAdButtonClicked();
            }
            else
            {
                Debug.LogError("[Rewards] Failed to create AppLovinManager");
                if (rewardedAdButton != null)
                {
                    rewardedAdButton.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Called when user completes watching the rewarded ad
        /// </summary>
        private void OnRewardedAdCompleted()
        {
            Debug.Log("[Rewards] User completed watching rewarded ad");
            
            // Grant the reward
            GrantRewardedAdReward();

            // Log completion to Firebase
            if (FirebaseAnalyticsManager.Instance != null)
            {
                var (coins, credits) = AdConfigManager.Instance?.GetRewardAmountsForPlayer() ?? (500, 4500);
                
                FirebaseAnalyticsManager.Instance.LogRewardedAdCompleted(
                    "destruction_screen", 
                    coins, 
                    credits
                );

                // Track as virtual currency earned
                FirebaseAnalyticsManager.Instance.LogEarnVirtualCurrency("coins", coins, "rewarded_ad");
                FirebaseAnalyticsManager.Instance.LogEarnVirtualCurrency("credits", credits, "rewarded_ad");
            }
        }

        /// <summary>
        /// Called when rewarded ad is closed (whether completed or not)
        /// </summary>
        private void OnRewardedAdClosed()
        {
            Debug.Log("[Rewards] Rewarded ad closed");
            // Ad closed, rewards already applied if completed
        }

        /// <summary>
        /// Called when rewarded ad fails to show
        /// </summary>
        private void OnRewardedAdFailed()
        {
            Debug.LogWarning("[Rewards] Rewarded ad failed to show");
            
            // Show button again if it was hidden
            if (!watchedRewardedAd && rewardedAdButton != null)
            {
                rewardedAdButton.SetActive(true);
            }
        }

        void OnApplicationQuit()
        {
            DataProvider.SaveGame();
            Debug.Log(DataProvider.GameModel.LifetimeDestructionScore);
            try
            {
                DataProvider.SaveGame();
                DataProvider.SyncCoinsToCloud();
                DataProvider.SyncCreditsToCloud();

                // Save changes:
                DataProvider.CloudSave();
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
            }
        }

        private void OnDestroy()
        {
            // Unregister rewarded ad callbacks
            if (AppLovinManager.Instance != null)
            {
                AppLovinManager.Instance.OnRewardedAdRewarded -= OnRewardedAdCompleted;
                AppLovinManager.Instance.OnRewardedAdClosed -= OnRewardedAdClosed;
                AppLovinManager.Instance.OnRewardedAdShowFailed -= OnRewardedAdFailed;
            }
        }
    }
}
