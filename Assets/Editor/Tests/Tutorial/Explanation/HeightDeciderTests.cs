using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.UI;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Tutorial.Explanation
{
    public class HeightDeciderTests
    {
        private IHeightDecider _decider;
        private ITogglable _doneButton, _okButton;
        private string _shortText, _longText;

        [SetUp]
        public void TestSetup()
        {
            _decider = new HeightDecider();

            _doneButton = Substitute.For<ITogglable>();
            _okButton = Substitute.For<ITogglable>();
            _shortText = "1234567890123456789012345678901234567890123456789012345678901234"; // 64 chars
            _longText = "12345678901234567890123456789012345678901234567890123456789012345"; // 65 chars
        }

        [Test]
        public void CanShrinkPanel_DoneButtonEnabled()
        {
            _doneButton.Enabled.Returns(true);
            Assert.IsFalse(_decider.CanShrinkPanel(_doneButton, _okButton, _shortText));
        }

        [Test]
        public void CanShrinkPanel_DoneButtonDisabled_OkButtonEnabled()
        {
            _doneButton.Enabled.Returns(false);
            _okButton.Enabled.Returns(true);
            Assert.IsFalse(_decider.CanShrinkPanel(_doneButton, _okButton, _shortText));
        }

        [Test]
        public void CanShrinkPanel_DoneButtonDisabled_OkButtonDisabled_TextTooLong()
        {
            _doneButton.Enabled.Returns(false);
            _okButton.Enabled.Returns(false);
            Assert.IsFalse(_decider.CanShrinkPanel(_doneButton, _okButton, _longText));
        }

        [Test]
        public void CanShrinkPanel_DoneButtonDisabled_OkButtonDisabled_TextShortEnough()
        {
            _doneButton.Enabled.Returns(false);
            _okButton.Enabled.Returns(false);
            Assert.IsTrue(_decider.CanShrinkPanel(_doneButton, _okButton, _shortText));
        }
    }
}