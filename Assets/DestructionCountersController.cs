using BattleCruisers.Buildables;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.PostBattleScreen;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.UI;
using BattleCruisers.UI.Loading;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace BattleCruisers.Scenes
{
    public class DestructionCountersController : MonoBehaviour
    {
        [SerializeField]
        private DestructionCard[] destructionCards;
        private long[] destructionValues;

        // Duplicated in DestructionSceneGod :(
        public Text million, billion, trillion, quadrillion;

        [SerializeField]
        private AnimationClip cardRevealAnim;
        [SerializeField]
        private TextMeshProUGUI damageCausedValueText; //<-- Damage totals are TextMeshPro for better formatting. Everything else could be too but that takes effort.
        [SerializeField]
        private Text timeValueText;
        [SerializeField]
        private TextMeshProUGUI allTimeDamageValueText;
        [SerializeField]
        private Text scoreText;

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

        [SerializeField]
        private GameObject levelUpModal;
        [SerializeField]
        private AnimationClip levelUpModalAnim;
        private float modalPeriod; // length of levelUpModalAnim

        private float timeStep; // used as the basis for all WaitForSeconds() returns 

        // Start is called before the first frame update
        void Start()
        {
            // TEMP - prevents NREs when testing DestructionScene without battle data
            ISceneNavigator sceneNavigator = LandingSceneGod.SceneNavigator;
            if (sceneNavigator == null)
            {
                // Fake data for testing:
                aircraftVal = Convert.ToInt64(UnityEngine.Random.Range(1.0f, 10000.0f));
                shipsVal = Convert.ToInt64(UnityEngine.Random.Range(1.0f, 10000.0f));
                cruiserVal = Convert.ToInt64(UnityEngine.Random.Range(1.0f, 10000.0f));
                buildingsVal = Convert.ToInt64(UnityEngine.Random.Range(1.0f, 10000.0f));
                allTimeVal = Convert.ToInt64(UnityEngine.Random.Range(10000.0f, 1000000.0f));
            }
            else
            {
                aircraftVal = BattleSceneGod.deadBuildables[TargetType.Aircraft].GetTotalDamageInCredits();
                shipsVal = BattleSceneGod.deadBuildables[TargetType.Ships].GetTotalDamageInCredits();
                cruiserVal = BattleSceneGod.deadBuildables[TargetType.Cruiser].GetTotalDamageInCredits();
                buildingsVal = BattleSceneGod.deadBuildables[TargetType.Buildings].GetTotalDamageInCredits();
                allTimeVal = ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.LifetimeDestructionScore;
            }
            // this seemed like the easiest way to store the values, so their indices match the destructionCards array:
            destructionValues = new long[] { aircraftVal, shipsVal, cruiserVal, buildingsVal };

            // TODO: Check if level time is already tracked? Doesn't seem to be a gettable field yet.
            // Here's a random number to test with for now:
            Debug.LogWarning("TIME value is fake! This should not be shipped!");
            levelTimeInSeconds = UnityEngine.Random.Range(200.0f, 800.0f);

            // TODO: Same issue as time, but for player XP:
            Debug.LogWarning("PLAYER XP value is fake! This should not be shipped!");
            currentXP = Mathf.FloorToInt(UnityEngine.Random.Range(100.0f, 10000.0f));
            nextLevelXP = Mathf.FloorToInt(UnityEngine.Random.Range(currentXP * 1.5f, currentXP * 5.0f));

            //### Screen Setup ###

            // Turn cards off by default:
            for (int i = 0; i < destructionCards.Length; i++)
            {
                destructionCards[i].gameObject.SetActive(false);
            }

            timeStep = cardRevealAnim.length;
            modalPeriod = levelUpModalAnim.length;

            // Set XP bar current/max values:
            levelBar.maxValue = nextLevelXP;
            levelBar.value = currentXP;

            // Set value texts:
            damageCausedValueText.text = "0";
            timeValueText.text = "00:00";
            prevAllTimeVal = allTimeVal - (aircraftVal + shipsVal + cruiserVal + buildingsVal);
            allTimeDamageValueText.text = FormatNumber(prevAllTimeVal);
            scoreText.text = "";

            // Start animating:
            StartCoroutine(AnimateScreen());
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
                yield return new WaitForSeconds(timeStep); // wait for destruction card reveal anim to finish before proceeding

                // Increase XP counter:
                int xpToAdd = Convert.ToInt32(destructionValues[i]);
                int xpRunningTotal = currentXP;

                // If the bar would fill up, it needs some special handling.
                if (xpToAdd + currentXP >= nextLevelXP)
                {
                    while (xpToAdd > 0)
                    {
                        if (xpToAdd + currentXP > nextLevelXP)
                        {
                            yield return StartCoroutine(InterpolateXPBar(xpRunningTotal, nextLevelXP, steps, stepPeriod));
                            yield return StartCoroutine(DisplayRankUpModal(modalPeriod));

                            xpToAdd -= (nextLevelXP - xpRunningTotal);
                            xpRunningTotal = 0;
                            currentXP = 0;

                            // TODO: get the NEXT level's XP and overwrite the nextLevelXP var
                        }
                        else
                        {
                            yield return StartCoroutine(InterpolateXPBar(xpRunningTotal, xpRunningTotal + xpToAdd, steps, stepPeriod));
                            xpRunningTotal += xpToAdd;
                            currentXP = xpRunningTotal;
                            xpToAdd = 0;
                        }
                    }
                }
                else
                {
                    yield return StartCoroutine(InterpolateXPBar(xpRunningTotal, xpRunningTotal + xpToAdd, steps, stepPeriod));
                    currentXP += xpToAdd;
                }
            }

            // Pause for a moment to catch our breath:
            yield return new WaitForSeconds(timeStep * 1.5f);

            // Interpolate time counter:
            yield return StartCoroutine(InterpolateTimeValue(0, levelTimeInSeconds, 60));
            yield return new WaitForSeconds(timeStep * 2.0f);

            // Interpolate game score:
            levelScore = CalculateScore(levelTimeInSeconds, Convert.ToInt32(aircraftVal + shipsVal + cruiserVal + buildingsVal));
            yield return StartCoroutine(InterpolateScore(0, levelScore, 25));

            // TODO: level rating (maybe?)

            // Interpolate Lifetime Damage (same deal as regular damage)
            yield return StartCoroutine(InterpolateLifetimeDamageValue(prevAllTimeVal, allTimeVal, 10));
        }

        IEnumerator InterpolateDamageValue(long startVal, long endVal, int steps)
        {
            long interpStep = (endVal - startVal)/steps;
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
            // Get old level (graphic + name)
            // Get new level (graphic + name)

            // Display modal
            levelUpModal.SetActive(true);
            yield return new WaitForSeconds(stepPeriod);
            levelUpModal.SetActive(false);
        }

        private long CalculateScore(float time, long damage)
        {
            // feels weird to make this a method but I don't like doing it in the animation methods:
            long score = damage / (long)Mathf.Pow(time, 2.0f) / (long)10;
            return score;
        }

        // TODO: Allow animation to be skipped, set all fields to their final state.
        //       For the impatient fans.
        void SkipAnimation()
        {

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

        // DUPLICATE CODE DETECTED!
        // Consider rolling DestructionCounterController into DestructionSceneGod.
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
    }
}