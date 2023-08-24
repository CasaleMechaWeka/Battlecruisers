using BattleCruisers.Buildables;
using BattleCruisers.Data;
using BattleCruisers.PostBattleScreen;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.UI;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes
{
    public class PvPDestructionSceneGod : MonoBehaviour
    {
        [SerializeField]
        private Text screenTitle;

        private IApplicationModel applicationModel;
        private ISceneNavigator _sceneNavigator;
        public DestructionCard[] destructionCards;
        public CanvasGroupButton nextButton;
        [SerializeField]
        private AudioSource _uiAudioSource;
        private ISingleSoundPlayer _soundPlayer;
        public Text million, billion, trillion, quadrillion;

        private long[] destructionValues;

        private bool realScene = true;

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

        // value to divide the score by:
        [SerializeField]
        private long scoreDivider;

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


        public Sprite BlackRig;
        public Sprite Bullshark;
        public Sprite Eagle;
        public Sprite Hammerhead;
        public Sprite HuntressBoss;
        public Sprite Longbow;
        public Sprite ManOfWarBoss;
        public Sprite Megalodon;
        public Sprite Raptor;
        public Sprite Rickshaw;
        public Sprite Rockjaw;
        public Sprite TasDevil;
        public Sprite Trident;
        public Sprite Yeti;

        async void Start()
        {
            _sceneNavigator = LandingSceneGod.SceneNavigator;

            if (_sceneNavigator != null)
            {
                LandingSceneGod.MusicPlayer.PlayVictoryMusic();
                applicationModel = ApplicationModelProvider.ApplicationModel;

                _soundPlayer
                    = new SingleSoundPlayer(
                        new SoundFetcher(),
                        new EffectVolumeAudioSource(
                            new AudioSourceBC(_uiAudioSource),
                            applicationModel.DataProvider.SettingsManager, 1));

                nextButton.Initialise(_soundPlayer, Done);
                _sceneNavigator.SceneLoaded(SceneNames.PvP_DESTRUCTION_SCENE);

            }

            PopulateScreen();
/*            // Populate screen:
            if (PvPBattleSceneGodServer.deadBuildables != null)
            {
                // real values:
                PopulateScreen();
            }
            else
            {
                // fake values if the screen is being launched for testing purposes:
                PopulateScreenFake();
            }*/

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
            allTimeVal = applicationModel.DataProvider.GameModel.LifetimeDestructionScore;
            levelTimeInSeconds = PvPBattleSceneGodTunnel._levelTimeInSeconds/*PvPBattleSceneGodServer.deadBuildables[PvPTargetType.PlayedTime].GetPlayedTime()*/;

            aircraftVal = PvPBattleSceneGodTunnel._aircraftVal/*PvPBattleSceneGodServer.deadBuildables[PvPTargetType.Aircraft].GetTotalDamageInCredits()*/;
            shipsVal = PvPBattleSceneGodTunnel._shipsVal/*PvPBattleSceneGodServer.deadBuildables[PvPTargetType.Ships].GetTotalDamageInCredits()*/;
            cruiserVal = PvPBattleSceneGodTunnel._cruiserVal/*PvPBattleSceneGodServer.deadBuildables[PvPTargetType.Cruiser].GetTotalDamageInCredits()*/;
            buildingsVal = PvPBattleSceneGodTunnel._buildingsVal/*PvPBattleSceneGodServer.deadBuildables[PvPTargetType.Buildings].GetTotalDamageInCredits()*/;

            // this seemed like the easiest way to store the values, so their indices match the destructionCards array:
            destructionValues = new long[] { aircraftVal, shipsVal, cruiserVal, buildingsVal };

            for (int i = 0; i < destructionCards.Length; i++)
            {
                destructionCards[i].destructionValue.text = FormatNumber(destructionValues[i]);
                destructionCards[i].numberOfUnitsDestroyed.text = i == 2 ? "1" : "" + PvPBattleSceneGodTunnel._totalDestroyed[i];
            }

            /*destructionCards[2].image.sprite = PvPBattleSceneGodServer.enemyCruiserSprite;*/
            switch (PvPBattleSceneGodTunnel._enemyCruiserName)
            {
                case "BlackRig":
                    destructionCards[2].image.sprite = BlackRig;
                    break;
                case "Bullshark":
                    destructionCards[2].image.sprite = Bullshark;
                    break;
                case "Eagle":
                    destructionCards[2].image.sprite = Eagle;
                    break;
                case "Hammerhead":
                    destructionCards[2].image.sprite = Hammerhead;
                    break;
                case "HuntressBoss":
                    destructionCards[2].image.sprite = HuntressBoss;
                    break;
                case "Longbow":
                    destructionCards[2].image.sprite = Longbow;
                    break;
                case "ManOfWarBoss":
                    destructionCards[2].image.sprite = ManOfWarBoss;
                    break;
                case "Megalodon":
                    destructionCards[2].image.sprite = Megalodon;
                    break;
                case "Raptor":
                    destructionCards[2].image.sprite = Raptor;
                    break;
                case "Rickshaw":
                    destructionCards[2].image.sprite = Rickshaw;
                    break;
                case "Rockjaw":
                    destructionCards[2].image.sprite = Rockjaw;
                    break;
                case "TasDevil":
                    destructionCards[2].image.sprite = TasDevil;
                    break;
                case "Trident":
                    destructionCards[2].image.sprite = Trident;
                    break;
                case "Yeti":
                    destructionCards[2].image.sprite = Yeti;
                    break;

            }
            destructionCards[2].description.text = PvPBattleSceneGodTunnel._enemyCruiserName/*PvPBattleSceneGodServer.enemyCruiserName*/;

            //### Screen Setup ###

            // Turn cards off by default:
            for (int i = 0; i < destructionCards.Length; i++)
            {
                destructionCards[i].gameObject.SetActive(false);
            }

            // Turn off Coins Counter by default:
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
            rank = ranker.CalculateRank(prevAllTimeVal);

            currentXP = (int)ranker.CalculateXpToNextLevel(prevAllTimeVal);
            nextLevelXP = (int)ranker.CalculateLevelXP(rank);
            rankNumber.text = FormatRankNumber(rank);
            rankText.text = ranker.destructionRanks[rank].transform.Find("RankNameText").GetComponent<Text>().text; // UGLY looking Find + Get
            rankGraphic.sprite = ranker.destructionRanks[rank].transform.Find("RankImage").GetComponent<Image>().sprite; // UGLY looking Find + Get

            CalculateRewards();

            // Set XP bar current/max values:
            if (ranker.CalculateRank(allTimeVal) == ranker.destructionRanks.Length - 1)
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
        }

        // Duplicate of PopulateScreen(), but with fake numbers.
        // Turns them into local vars for frequently-used values,
        // Populates text fields on the screen with those values.
        // This should not be displayed to real users.
        // DOES NOT awards any rewards.
        private void PopulateScreenFake()
        {
            long randomVal = 1000000000;

            allTimeVal = randomVal;
            levelTimeInSeconds = UnityEngine.Random.Range(300, 600);
            aircraftVal = randomVal / UnityEngine.Random.Range(4, 6);
            shipsVal = randomVal / UnityEngine.Random.Range(4, 6);
            cruiserVal = randomVal / UnityEngine.Random.Range(4, 6);
            buildingsVal = randomVal / UnityEngine.Random.Range(4, 6);

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

            // Turn off Coins Counter by default:
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
            rank = ranker.CalculateRank(prevAllTimeVal);

            currentXP = (int)ranker.CalculateXpToNextLevel(prevAllTimeVal);
            nextLevelXP = (int)ranker.CalculateLevelXP(rank);
            rankNumber.text = FormatRankNumber(rank);
            rankText.text = ranker.destructionRanks[rank].transform.Find("RankNameText").GetComponent<Text>().text; // UGLY looking Find + Get
            rankGraphic.sprite = ranker.destructionRanks[rank].transform.Find("RankImage").GetComponent<Image>().sprite; // UGLY looking Find + Get
            coinsToAward = CalculateCoins(CalculateScore(levelTimeInSeconds, (aircraftVal + shipsVal + cruiserVal + buildingsVal), scoreDivider));
            coinsText.text = "+" + coinsToAward.ToString();

            // Set XP bar current/max values:
            if (ranker.CalculateRank(allTimeVal) == ranker.destructionRanks.Length - 1)
            {
                levelBar.maxValue = 1;
                levelBar.value = 1;
            }
            else
            {
                levelBar.maxValue = nextLevelXP;
                levelBar.value = currentXP;
            }

            CalculateRewards();

            screenTitle.text = "Debug Mode";
            realScene = false;
        }

        IEnumerator AnimateScreen()
        {
            yield return new WaitForSeconds(1.0f);

            // Enable a card, interpolate its total into the Damage Total, add XP to bar, repeat
            long damageRunningTotal = 0;
            int steps = 30;
            float stepPeriod = timeStep / steps;

            for (int i = 0; i < destructionCards.Length; i++)
            {
                // Enable a card (playing its animation in the process):
                destructionCards[i].gameObject.SetActive(true);

                // Interpolate the Damage Caused value, from the current running total to that + the card's damage value
                // by the specified number of steps. Steps are divided over time:
                yield return StartCoroutine(InterpolateDamageValue(damageRunningTotal, damageRunningTotal + destructionValues[i], 10));
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
                    if (ranker.CalculateRank(allTimeVal) < ranker.destructionRanks.Length - 1)
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
                                nextLevelXP = (int)ranker.CalculateLevelXP(rank);
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
            yield return StartCoroutine(InterpolateTimeValue(0, levelTimeInSeconds, 60));

            // Interpolate game score:
            levelScore = CalculateScore(levelTimeInSeconds, Convert.ToInt32(aircraftVal + shipsVal + cruiserVal + buildingsVal), scoreDivider);
            yield return StartCoroutine(InterpolateScore(0, levelScore, 25));

            // Award any rewards:
            if (coinsToAward > 0 || creditsToAward > 0 || nukesToAward > 0)
            {
                if (BattleSceneGod.deadBuildables == null || applicationModel.Mode != GameMode.Skirmish)
                {
                    rewardsCounter.SetActive(true);
                }
            }

            // TODO: level rating (maybe?)

            // Interpolate Lifetime Damage (same deal as regular damage)
            yield return StartCoroutine(InterpolateLifetimeDamageValue(prevAllTimeVal, allTimeVal, 10));
        }

        IEnumerator InterpolateDamageValue(long startVal, long endVal, int steps)
        {
            long interpStep = (endVal - startVal) / steps;
            float stepPeriod = (timeStep) / steps;

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
            float stepPeriod = (timeStep) / steps;

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
            float stepPeriod = (timeStep) / steps;

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

        private void CalculateRewards()
        {
            coinsToAward = CalculateCoins(CalculateScore(levelTimeInSeconds, (aircraftVal + shipsVal + cruiserVal + buildingsVal), scoreDivider));
            if (coinsToAward > 0)
            {
                coinsCounter.SetActive(true);
                coinsText.text = "+" + coinsToAward.ToString();
            }
            else
            {
                coinsCounter.SetActive(false);
            }

            creditsToAward = CalculateCredits();
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
        }

        private long CalculateScore(float time, long damage, long constant)
        {
            // feels weird to make this a method but I don't like doing it directly in the animation methods:
            long score = (damage * 1000) / ((long)Mathf.Pow(time, 2.0f) / constant);
            return score;
        }

        private int CalculateCoins(long score)
        {
            // 5 coins
            if (score >= 5000)
            {
                return 5;
            }
            // 4 coins
            if (score >= 4000)
            {
                return 4;
            }
            // 3 coins
            if (score >= 3000)
            {
                return 3;
            }
            // 2 coins
            else if (score >= 2000)
            {
                return 2;
            }
            // 1 coin
            else if (score >= 1000)
            {
                return 1;
            }

            return 0;
        }

        private long CalculateCredits()
        {
            long creditsAward = (cruiserVal + buildingsVal) / scoreDivider;
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
            if (applicationModel.Mode != GameMode.Skirmish)//update the gamemodel if the game mode is not skirmish
            {
                long destructionScore = aircraftVal + shipsVal + cruiserVal + buildingsVal;
                applicationModel.DataProvider.GameModel.LifetimeDestructionScore += destructionScore;

                // we need XPToNextLevel to populate any XP progress bars:
                long newLifetimeScore = applicationModel.DataProvider.GameModel.LifetimeDestructionScore;

                // Give the player their rewards:
                applicationModel.DataProvider.GameModel.Coins += coinsToAward;
                applicationModel.DataProvider.GameModel.Credits += creditsToAward;
                applicationModel.DataProvider.SaveGame();
                //applicationModel.DataProvider.GameModel.Nukes += nukesToAward; <--- This does not exist right now.
                await applicationModel.DataProvider.SyncCoinsToCloud();
                await applicationModel.DataProvider.SyncCreditsToCloud();
                // Save changes:
                await applicationModel.DataProvider.CloudSave();
            }
        }

        private void Done()
        {
            // and now we actually are done:
            _sceneNavigator.GoToScene(SceneNames.SCREENS_SCENE, false);
        }

        //taken from https://stackoverflow.com/questions/30180672/string-format-numbers-to-millions-thousands-with-rounding
        private string FormatNumber(long num)
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

        private string FormatRankNumber(int rank)
        {
            string numString = rank.ToString();
            if (rank < 10)
            {
                numString = "0" + rank.ToString();
            }
            return rank.ToString();
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
    }
}

