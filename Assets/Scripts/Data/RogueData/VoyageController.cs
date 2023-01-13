using BattleCruisers.Data;
using BattleCruisers.Data.Helpers;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.PostBattleScreen;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.ScreensScene;
using BattleCruisers.UI.ScreensScene.ChooseDifficultyScreen;
using BattleCruisers.UI.ScreensScene.HomeScreen;
using BattleCruisers.UI.ScreensScene.LevelsScreen;
using BattleCruisers.UI.ScreensScene.LoadoutScreen;
using BattleCruisers.UI.ScreensScene.PostBattleScreen;
using BattleCruisers.UI.ScreensScene.SettingsScreen;
using BattleCruisers.UI.ScreensScene.SkirmishScreen;
using BattleCruisers.UI.ScreensScene.TrashScreen;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Cache;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes
{

    public class VoyageController : MonoBehaviour
    {
        // Reference to the "New Voyage" button
        public Button newVoyageButton;

        // Reference to the label GameObject that displays the VoyageNumber
        public GameObject voyageNumberLabel;

        // Reference to the text GameObject that displays the VoyageNumber
        public GameObject voyageNumberText;

        private void Start()
        {
            // Set up the "New Voyage" button to call the NewVoyage method when clicked
            newVoyageButton.onClick.AddListener(NewVoyage);
        }

        private void NewVoyage()
        {
            // Create a new instance of the Voyage singleton object
            Voyage voyage = new Voyage();

            // Set the singleton field to the new instance of the Voyage object
            Voyage.Instance = voyage;

            // Update the text component of the voyageNumberText GameObject to display the new VoyageNumber
            voyageNumberText.GetComponent<Text>().text = voyage.VoyageNumber.ToString();
        }
    }

}
