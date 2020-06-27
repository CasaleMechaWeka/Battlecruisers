using BattleCruisers.UI.BattleScene.InGameHints;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.InGameHints
{
    public class NonRepeatingHintDisplayerTests
    {
        private IHintDisplayer _nonRepeatingDisplayer, _coreDisplayer;
        private string _hint1, _hint2;

        [SetUp]
        public void TestSetup()
        {
            _coreDisplayer = Substitute.For<IHintDisplayer>();
            _nonRepeatingDisplayer = new NonRepeatingHintDisplayer(_coreDisplayer);

            _hint1 = "halbs";
            _hint2 = "and siege";
        }

        [Test]
        public void FirstTimeHint()
        {
            _nonRepeatingDisplayer.ShowHint(_hint1);
            _coreDisplayer.Received().ShowHint(_hint1);
        }

        [Test]
        public void SecondTimeHint()
        {
            _nonRepeatingDisplayer.ShowHint(_hint1);
            _coreDisplayer.ClearReceivedCalls();

            _nonRepeatingDisplayer.ShowHint(_hint1);
            _coreDisplayer.DidNotReceive().ShowHint(_hint1);
        }

        [Test]
        public void FirstTimeHint_AfterSecondTimeHint()
        {
            _nonRepeatingDisplayer.ShowHint(_hint1);
            _nonRepeatingDisplayer.ShowHint(_hint1);
            _coreDisplayer.ClearReceivedCalls();

            _nonRepeatingDisplayer.ShowHint(_hint2);
            _coreDisplayer.Received().ShowHint(_hint2);
        }
    }
}