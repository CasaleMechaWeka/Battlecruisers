using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils.BattleScene;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene
{
    public class DummyModalManager : ModalManager
    {
        public DummyModalManager(INavigationPermitterManager navigationPermitterManager, IPauseGameManager pauseGameManager) 
            : base(navigationPermitterManager, pauseGameManager)
        {
        }

        public void TriggerShowModal()
        {
            ShowModal();
        }

        public void TriggerHideModal()
        {
            HideModal();
        }
    }

    public class ModalManagerTests : ModalManagerTestsBase
    {
        private DummyModalManager _modalManager;
        private NavigationPermittersState _preMenuState;

        [SetUp]
        public override void TestSetup()
        {
            base.TestSetup();

            _modalManager = new DummyModalManager(_navigationPermitterManager, _pauseGameManager);

            _preMenuState = new NavigationPermittersState(default, default, default, default);
            _navigationPermitterManager.PauseNavigation().Returns(_preMenuState);
        }

        [Test]
        public void ShowMenu()
        {
            _modalManager.TriggerShowModal();

            _navigationPermitterManager.Received().PauseNavigation();
            _pauseGameManager.Received().PauseGame();
        }

        [Test]
        public void ShowMenu_SecondTme()
        {
            _modalManager.TriggerShowModal();
            _pauseGameManager.ClearReceivedCalls();
            _navigationPermitterManager.ClearReceivedCalls();

            _modalManager.TriggerShowModal();

            _navigationPermitterManager.DidNotReceive().PauseNavigation();
            _pauseGameManager.Received().PauseGame();
        }

        [Test]
        public void DismissMenu_WithouPreviousShow()
        {
            _modalManager.TriggerHideModal();

            _navigationPermitterManager.DidNotReceiveWithAnyArgs().RestoreNavigation(default);
            _pauseGameManager.Received().ResumeGame();
        }

        [Test]
        public void DismissMenu_WithPreviousShow()
        {
            _modalManager.TriggerShowModal();

            _modalManager.TriggerHideModal();

            _navigationPermitterManager.RestoreNavigation(_preMenuState);
            _pauseGameManager.Received().ResumeGame();
        }
    }
}