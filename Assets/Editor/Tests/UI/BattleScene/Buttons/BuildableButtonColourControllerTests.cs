using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.Buttons;
using UnityCommon.Properties;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace BattleCruisers.Tests.UI.BattleScene.Buttons
{
    public class BuildableButtonColourControllerTests
    {
        private BuildableButtonColourController _colourController;
        private IBroadcastingProperty<ITarget> _selectedItem;
        private IReadOnlyCollection<IBuildableButton> _buttons;
        private IBuildableButton _button1, _button2;
        private IBuildable _buildable1, _buildable2;
        private ITarget _targetWithoutButton;

        [SetUp]
        public void TestSetup()
        {
            _button1 = Substitute.For<IBuildableButton>();
            _buildable1 = Substitute.For<IBuildable>();
            _button1.Buildable.Returns(_buildable1);

            _button2 = Substitute.For<IBuildableButton>();
            _buildable2 = Substitute.For<IBuildable>();
            _button2.Buildable.Returns(_buildable2);

            _selectedItem = Substitute.For<IBroadcastingProperty<ITarget>>();

            List<IBuildableButton> buttons = new List<IBuildableButton>()
            {
                _button1,
                _button2
            };

            _colourController = new BuildableButtonColourController(_selectedItem, buttons.AsReadOnly());

            _targetWithoutButton = Substitute.For<ITarget>();
        }

        [Test]
        public void SelectedItemChanged_HaveMatchingButton_HighlightsButton()
        {
            SelectItem(_buildable1);
            _button1.Received().Color = ButtonColour.Selected;
        }

        [Test]
        public void SelectedItemChanged_HaveMatchingButton_UnhighlightsCurrentButton_HighlightsNewButton()
        {
            SelectItem(_buildable1);
            SelectItem(_buildable2);

            _button1.Received().Color = ButtonColour.Default;
            _button2.Received().Color = ButtonColour.Selected;
        }

        [Test]
        public void SelectedItemChanged_NoMatchingButton_UnhighlightsCurrentButton()
        {
            SelectItem(_buildable1);
            SelectItem(_targetWithoutButton);

            _button1.Received().Color = ButtonColour.Default;
        }

        [Test]
        public void SelectedItemChanged_Null_UnhighlightsCurrentButton()
        {
            SelectItem(_buildable1);
            SelectItem(null);

            _button1.Received().Color = ButtonColour.Default;
        }

        private void SelectItem(ITarget item)
        {
            _selectedItem.Value.Returns(item);
            _selectedItem.ValueChanged += Raise.Event();
        }
    }
}