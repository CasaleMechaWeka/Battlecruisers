using BattleCruisers.UI.BattleScene.GameSpeed;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.GameSpeed
{
    public class SpeedbuttonManagerTests
    {
        private ISpeedButtonManager _manager;
        private IGameSpeedButton _button1, _button2;

        [SetUp]
        public void SetuUp()
        {
            _manager = new SpeedButtonManager();

            _button1 = Substitute.For<IGameSpeedButton>();
            _button2 = Substitute.For<IGameSpeedButton>();
        }

        [Test]
        public void SelectButton_NoCurrentlySelectedButton()
        {
            _manager.SelectButton(_button1);
            _button1.Received().IsSelected = true;
        }

        [Test]
        public void SelectButton_HaveCurrentlySelectedButton()
        {
            // Select first button, no currently selected button
            _manager.SelectButton(_button1);

            // Select second button, first buttonis currently selected
            _manager.SelectButton(_button2);
            _button1.Received().IsSelected = false;
            _button2.Received().IsSelected = true;
        }
    }
}
