using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Offensive;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.BattleScene.Navigation;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.Navigation
{
    public class CruiserDeathCameraFocuserTests
    {
        private ICruiserDeathCameraFocuser _cruiserDeathCameraFocuser;
        private ICameraFocuser _cameraFocuser;
        private ICruiser _losingCruiser;
        private NukeLauncherController _nukeLauncherController;

        [SetUp]
        public void TestSetup()
        {
            _cameraFocuser = Substitute.For<ICameraFocuser>();
            _cruiserDeathCameraFocuser = new CruiserDeathCameraFocuser(_cameraFocuser);

            _losingCruiser = Substitute.For<ICruiser>();
            _nukeLauncherController = new NukeLauncherController();
        }

        [Test]
        public void FocusOnLosingCruiser_PlayerCruiser_Nuked()
        {
            _losingCruiser.IsPlayerCruiser.Returns(true);
            _losingCruiser.LastDamagedSource.Returns(_nukeLauncherController);

            _cruiserDeathCameraFocuser.FocusOnLosingCruiser(_losingCruiser);

            _cameraFocuser.Received().FocusOnPlayerCruiserNuke();
        }

        [Test]
        public void FocusOnLosingCruiser_PlayerCruiser_NotNuked()
        {
            _losingCruiser.IsPlayerCruiser.Returns(true);
            _losingCruiser.LastDamagedSource.Returns((ITarget)null);

            _cruiserDeathCameraFocuser.FocusOnLosingCruiser(_losingCruiser);

            _cameraFocuser.Received().FocusOnPlayerCruiserDeath();
        }

        [Test]
        public void FocusOnLosingCruiser_NotPlayerCruiser_Nuked()
        {
            _losingCruiser.IsPlayerCruiser.Returns(false);
            _losingCruiser.LastDamagedSource.Returns(_nukeLauncherController);

            _cruiserDeathCameraFocuser.FocusOnLosingCruiser(_losingCruiser);

            _cameraFocuser.Received().FocusOnAICruiserNuke();
        }

        [Test]
        public void FocusOnLosingCruiser_NotPlayerCruiser_NotNuked()
        {
            _losingCruiser.IsPlayerCruiser.Returns(false);
            _losingCruiser.LastDamagedSource.Returns((ITarget)null);

            _cruiserDeathCameraFocuser.FocusOnLosingCruiser(_losingCruiser);

            _cameraFocuser.Received().FocusOnAICruiserDeath();
        }
    }
}