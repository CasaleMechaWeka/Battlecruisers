using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Fetchers;
using NSubstitute;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Effects.Deaths
{
    public class CruiserDeathCameraTestGod : TestGodBase
    {
        private ICameraFocuser _cameraFocuser;

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
            CameraComponents cameraComponents = cameraInitialiser
                .Initialise(
                    DataProvider.SettingsManager,
                    playerCruiser,
                    aiCruiser,
                    navigationPermitters,
                    _updaterProvider.SwitchableUpdater,
                    Substitute.For<SingleSoundPlayer>());
            _cameraFocuser = cameraComponents.CameraFocuser;

            BuildableInitialisationArgs args = new BuildableInitialisationArgs(helper);
        }

        public void PlayerNuke()
        {
            PrefabFactory.ShowExplosion(ExplosionType.Explosion500, playerCruiser.Position);
        }

        public void PlayerCruiser()
        {
            _cameraFocuser.FocusOnLeftCruiserDeath();
        }

        public void PlayerCruiserNuke()
        {
            _cameraFocuser.FocusOnLeftCruiserNuke();
        }

        public void AINuke()
        {
            PrefabFactory.ShowExplosion(ExplosionType.Explosion500, aiCruiser.Position);
        }

        public void AICruiser()
        {
            _cameraFocuser.FocusOnRightCruiserDeath();
        }

        public void AICruiserNuke()
        {
            _cameraFocuser.FocusOnRightCruiserNuke();
        }
    }
}