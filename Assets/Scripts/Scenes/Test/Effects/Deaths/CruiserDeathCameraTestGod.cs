using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Scenes.Test.Utilities;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.Factories;
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
        private IFactoryProvider _factoryProvider;

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
            ICameraComponents cameraComponents = cameraInitialiser
                .Initialise(
                    ApplicationModelProvider.ApplicationModel.DataProvider.SettingsManager,
                    playerCruiser,
                    aiCruiser,
                    navigationPermitters,
                    _updaterProvider.SwitchableUpdater,
                    Substitute.For<ISingleSoundPlayer>());
            _cameraFocuser = cameraComponents.CameraFocuser;

            BuildableInitialisationArgs args = new BuildableInitialisationArgs(helper);
            _factoryProvider = args.FactoryProvider;
        }

        public void PlayerNuke()
        {
            _factoryProvider.PoolProviders.ExplosionPoolProvider.HugeExplosionsPool.GetItem(playerCruiser.Position);

        }

        public void PlayerCruiser()
        {
            _cameraFocuser.FocusOnPlayerCruiserDeath();
        }

        public void PlayerCruiserNuke()
        {
            _cameraFocuser.FocusOnPlayerCruiserNuke();
        }

        public void AINuke()
        {
            _factoryProvider.PoolProviders.ExplosionPoolProvider.HugeExplosionsPool.GetItem(aiCruiser.Position);
        }

        public void AICruiser()
        {
            _cameraFocuser.FocusOnAICruiserDeath();
        }

        public void AICruiserNuke()
        {
            _cameraFocuser.FocusOnAICruiserNuke();
        }
    }
}