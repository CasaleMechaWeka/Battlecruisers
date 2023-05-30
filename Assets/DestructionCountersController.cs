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

        // Damage values to interpolate damaged caused with:
        private long aircraftVal;
        private long shipsVal;
        private long cruiserVal;
        private long buildingsVal;
        private long allTimeVal;

        // Time value to interpolate with:
        private float levelTimeInSeconds;

        // Level and XP tracking:
        [SerializeField]
        private Slider levelBar;
        private int nextLevelXP;
        private int currentXP;

        // Start is called before the first frame update
        void Start()
        {
            // TEMP - prevents NREs when testing DestructionScene without battle data
            ISceneNavigator sceneNavigator = LandingSceneGod.SceneNavigator;
            if (sceneNavigator == null)
            {
                // Fake data for testing:
                aircraftVal = Convert.ToInt64(UnityEngine.Random.Range(1.0f, 1000.0f));
                shipsVal = Convert.ToInt64(UnityEngine.Random.Range(1.0f, 1000.0f));
                cruiserVal = Convert.ToInt64(UnityEngine.Random.Range(1.0f, 1000.0f));
                buildingsVal = Convert.ToInt64(UnityEngine.Random.Range(1.0f, 1000.0f));
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
            levelTimeInSeconds = UnityEngine.Random.Range(0.0f, 3000.0f);

            // TODO: Same issue as time, but for player XP:
            Debug.LogWarning("PLAYER XP value is fake! This should not be shipped!");
            currentXP = Mathf.FloorToInt(UnityEngine.Random.Range(0.0f, 1000.0f));
            nextLevelXP = Mathf.FloorToInt(UnityEngine.Random.Range((currentXP * 5.0f), (currentXP * 15.0f)));

            //### Screen Setup ###

            // Turn cards off by default:
            for (int i = 0; i < destructionCards.Length; i++)
            {
                destructionCards[i].gameObject.SetActive(false);
            }

            // Set XP bar current/max values:
            levelBar.maxValue = nextLevelXP;
            levelBar.value = currentXP;

            // Set value texts:
            damageCausedValueText.text = "0";
            timeValueText.text = "00:00";
            allTimeDamageValueText.text = FormatNumber(allTimeVal);

            // Start animating:
            StartCoroutine(AnimateScreen());
        }

        IEnumerator AnimateScreen()
        {
            yield return new WaitForSeconds(cardRevealAnim.length * 4.0f);

            // Enable a card, interpolate its total into the Damage Total, add XP to bar, repeat
            long damageRunningTotal = 0;
            for (int i = 0; i < destructionCards.Length; i++)
            {
                // Enable a card (playing its animation in the process):
                destructionCards[i].gameObject.SetActive(true);

                // Interpolate the Damage Caused value, from the current running total to that + the card's damage value
                // by the specified number of steps. Steps are divided over time so the length of the animation remains constant:
                StartCoroutine(InterpolateDamageValue(damageRunningTotal, damageRunningTotal + destructionValues[i], 10, returnValue =>
                {
                    damageRunningTotal += returnValue;
                }));

                // Increase XP counter:
                int xpToAdd = Convert.ToInt32(destructionValues[i]);
                // If the bar would fill up here, it needs some special handling:
                int xpRunningTotal = currentXP;
                StartCoroutine(InterpolateXPBar(xpRunningTotal, xpRunningTotal + xpToAdd, 60, returnValue =>
                {
                    if (xpRunningTotal + returnValue < nextLevelXP)
                    {
                        xpRunningTotal += returnValue;
                    }
                    else
                    {
                        // do level pop
                        // TODO: StartCoroutine(DoLevelPop);

                        xpRunningTotal = 0;
                        currentXP = 0;
                    }
                }));
                currentXP += xpToAdd;


                // Pause for a moment to catch our breath:
                yield return new WaitForSeconds(cardRevealAnim.length * 1.5f);
            }

            // Interpolate time counter:
            StartCoroutine(InterpolateTimeValue(0, levelTimeInSeconds, 60));

            // TODO: game score (damage * time, or something along those lines)

            // TODO: level rating (maybe?)

            // Interpolate Lifetime Damage (same deal as regular damage)
            StartCoroutine(InterpolateLifetimeDamageValue(damageRunningTotal, allTimeVal + damageRunningTotal, 10));

            //yield return null;
        }

        IEnumerator InterpolateDamageValue(long startVal, long endVal, int steps, System.Action<long> callback = null)
        {
            long interpStep = (endVal - startVal)/steps;
            long curVal = startVal;
            float stepPeriod = (cardRevealAnim.length) / steps;

            for (int i = 1; i <= steps; i++)
            {
                curVal += interpStep;
                damageCausedValueText.text = FormatNumber(curVal);
                yield return new WaitForSeconds(stepPeriod);
            }
            callback.Invoke(curVal);
        }

        IEnumerator InterpolateLifetimeDamageValue(long startVal, long endVal, int steps)
        {
            long interpStep = (endVal - startVal) / steps;
            long curVal = startVal;
            float stepPeriod = (cardRevealAnim.length) / steps;

            for (int i = 1; i <= steps; i++)
            {
                curVal += interpStep;
                allTimeDamageValueText.text = FormatNumber(curVal);
                yield return new WaitForSeconds(stepPeriod);
            }
        }

        IEnumerator InterpolateTimeValue(float startVal, float endVal, int steps)
        {
            float interpStep = (endVal - startVal) / steps;
            float curVal = startVal;
            float stepPeriod = (cardRevealAnim.length * 2.0f) / steps; // timestamps look a bit nicer if they interp a bit slower

            for (int i = 1; i <= steps; i++)
            {
                curVal += interpStep;
                timeValueText.text = FormatTime(curVal);
                yield return new WaitForSeconds(stepPeriod);
            }
        }

        IEnumerator InterpolateXPBar(int startVal, int endVal, int steps, System.Action<int> callback = null)
        {
            int interpStep = (endVal - startVal) / steps;
            int curVal = startVal;
            float stepPeriod = (cardRevealAnim.length) / steps;

            for (int i = 1; i <= steps; i++)
            {
                curVal += interpStep;
                levelBar.value = curVal;
                yield return new WaitForSeconds(stepPeriod);
            }
            callback.Invoke(curVal);
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