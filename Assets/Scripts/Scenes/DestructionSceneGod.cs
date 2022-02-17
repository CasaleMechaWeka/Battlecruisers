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
        async void Start()
        {
            _sceneNavigator = LandingSceneGod.SceneNavigator;

            

            long totalDestruction = 0;
            for (int i = 0; i < destructionCards.Length; i++)
            {
                destructionCards[i].destructionValue.text = MakeDenomination(BattleSceneGod.deadBuildables[(TargetType)i].GetTotalDamageInCredits());
                destructionCards[i].numberOfUnitsDestroyed.text = i== 2 ? "1" : "" + BattleSceneGod.deadBuildables[(TargetType)i].GetTotalDestroyed();
                totalDestruction += BattleSceneGod.deadBuildables[(TargetType)i].GetTotalDamageInCredits();
            }

            postBattleDestructionScoreText.text = MakeDenomination(totalDestruction);
            lifetimeDestructionScoreText.text = MakeDenomination(ApplicationModelProvider.ApplicationModel.DataProvider.GameModel.LifetimeDestructionScore);

            destructionCards[2].image.sprite = BattleSceneGod.enemyCruiserSprite;
            destructionCards[2].description.text = BattleSceneGod.enemyCruiserName;
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

        private string MakeDenomination(long value)
        {
            string s = value.ToString("#,#", CultureInfo.InvariantCulture);
            if (value == 0)
            {
                s = "" + 0;
            }
            /*if (value >= 1000)
            {
                s = value.ToString("#,##0,K", CultureInfo.InvariantCulture);
            }
            if (value >= 1000000)
            {
                s = value.ToString("#,##0,,M", CultureInfo.InvariantCulture);
            }
            if (value >= 1000000000)
            {
                s = value.ToString("#,##0,,,B", CultureInfo.InvariantCulture);
            }
            if (value >= 1000000000000)
            {
                s = value.ToString("#,##0,,,,T", CultureInfo.InvariantCulture);
            }*/

            return "$" + s;
        }
    }
        
}
