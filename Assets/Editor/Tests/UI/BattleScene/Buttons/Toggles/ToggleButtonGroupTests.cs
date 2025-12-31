using BattleCruisers.UI.BattleScene.Buttons.Toggles;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace BattleCruisers.Tests.UI.BattleScene.Buttons.Toggles
{
    public class ToggleButtonGroupTests
    {
        private ToggleButtonGroup _toggleGroup;
        private IToggleButton _button1, _button2;

        [SetUp]
        public void TestSetup()
        {
            _button1 = Substitute.For<IToggleButton>();
            _button2 = Substitute.For<IToggleButton>();

            _toggleGroup 
                = new ToggleButtonGroup(
                    new List<IToggleButton>() { _button1, _button2 },
                    _button2);
        }

        [Test]
        public void InitialState()
        {
            _button2.Received().IsSelected = true;
        }

        [Test]
        public void UnselectedButtonClicked_SelectsButton()
        {
            _button1.Clicked += Raise.Event();
            _button1.Received().IsSelected = true;
        }

        [Test]
        public void SelectedButtonClicked()
        {
            // Select button
            _button1.Clicked += Raise.Event();
            _button1.Received().IsSelected = true;
            _button1.ClearReceivedCalls();

            // Select button again
            _button1.Clicked += Raise.Event();
            _button1.Received().IsSelected = true;
        }

        [Test]
        public void UnselectedButtonClicked_DeselectsCurrentlySelectedButton_AndSelectsClickedButton()
        {
            _button2.ClearReceivedCalls();

            // Select button
            _button1.Clicked += Raise.Event();
            _button1.Received().IsSelected = true;

            // Select different button
            _button2.Clicked += Raise.Event();
            _button1.Received().IsSelected = false;
            _button2.Received().IsSelected = true;
        }
    }
}