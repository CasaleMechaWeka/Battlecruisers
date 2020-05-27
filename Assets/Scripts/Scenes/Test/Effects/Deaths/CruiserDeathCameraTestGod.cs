using System.Collections;
using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Effects.Deaths
{
    public class CruiserDeathCameraTestGod : TestGodBase
    {
        public Cruiser playerCruiser, aiCruiser;

        protected override List<GameObject> GetGameObjects()
        {
            BCUtils.Helper.AssertIsNotNull(playerCruiser, aiCruiser);

            return new List<GameObject>()
            {
                playerCruiser.GameObject,
                aiCruiser.GameObject
            };
        }

        protected override void Setup(Helper helper)
        {
            helper.SetupCruiser(playerCruiser);
            helper.SetupCruiser(aiCruiser);

            NavigationPermitters navigationPermitters = new NavigationPermitters();

            CameraInitialiser cameraInitialiser = FindObjectOfType<CameraInitialiser>();
            Assert.IsNotNull(cameraInitialiser);
            cameraInitialiser
                .Initialise(
                    ApplicationModelProvider.ApplicationModel.DataProvider.SettingsManager,
                    playerCruiser,
                    aiCruiser,
                    navigationPermitters,
                    _updaterProvider.SwitchableUpdater);
        }
    }
}