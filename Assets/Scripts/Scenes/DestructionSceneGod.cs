using BattleCruisers.Buildables;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.PostBattleScreen;
using BattleCruisers.Scenes.BattleScene;
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

namespace BattleCruisers.Scenes
{
    public class DestructionSceneGod : MonoBehaviour
    {
        private ISceneNavigator _sceneNavigator;
        public DestructionCard [] destructionCards;
        public Text postBattleDestructionScoreText;
        public Text lifetimeDestructionScoreText;
        public DestructionRanker ranker;
        async void Start()
        {
            _sceneNavigator = LandingSceneGod.SceneNavigator;

            

            long totalDestruction = 0;
            for (int i = 0; i < destructionCards.Length; i++)
            {
                destructionCards[i].destructionValue.text = FormatNumber(BattleSceneGod.deadBuildables[(TargetType)i].GetTotalDamageInCredits());
                destructionCards[i].numberOfUnitsDestroyed.text = i== 2 ? "1" : "" + BattleSceneGod.deadBuildables[(TargetType)i].GetTotalDestroyed();
                totalDestruction += BattleSceneGod.deadBuildables[(TargetType)i].GetTotalDamageInCredits();
            }

            postBattleDestructionScoreText.text = FormatNumber(totalDestruction);
            lifetimeDestructionScoreText.text = FormatNumber(ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.LifetimeDestructionScore);

            destructionCards[2].image.sprite = BattleSceneGod.enemyCruiserSprite;
            destructionCards[2].description.text = BattleSceneGod.enemyCruiserName;

            ranker.DisplayRank(ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.LifetimeDestructionScore);



            
            _sceneNavigator.SceneLoaded(SceneNames.DESTRUCTION_SCENE);
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

        private void Done()
        {
            _sceneNavigator.GoToScene(SceneNames.SCREENS_SCENE);
        }

        //taken from https://stackoverflow.com/questions/30180672/string-format-numbers-to-millions-thousands-with-rounding
        private  string FormatNumber(long num)
        {
            num = num*1000;
            long i = (long)Math.Pow(10, (int)Math.Max(0, Math.Log10(num) - 2));
            num = num / i * i;
            if (num >= 1000000000000)
                return (num / 1000000000000D).ToString("0.##") + "Q";
            if (num >= 1000000000)
                return (num / 1000000000D).ToString("0.##") + "T";
            if (num >= 1000000)
                return (num / 1000000D).ToString("0.##") + "B";
            if (num >= 1000)
                return (num / 1000D).ToString("0.##") + "M";

            return num.ToString("#,0");
        }
    }
        
}
