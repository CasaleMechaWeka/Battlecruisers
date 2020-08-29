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

            // FELIX  Fix :)
            _toggleGroup 
                = new ToggleButtonGroup(
                    new List<IToggleButton>() { _button1, _button2 },
                    null);
        }

        [Test]
        public void UnselectedButtonClicked_SelectsButton()
        {
            _button1.Clicked += Raise.Event();
            _button1.Received().IsSelected = true;
        }

        [Test]
        public void SelectedButtonClicked_DeselectsButton()
        {
            // Select button
            _button1.Clicked += Raise.Event();
            _button1.Received().IsSelected = true;

            // Deselect button
            _button1.Clicked += Raise.Event();
            _button1.Received().IsSelected = false;
        }

        [Test]
        public void UnselectedButtonClicked_DeselectsCurrentlySelectedButton_AndSelectsClickedButton()
        {
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