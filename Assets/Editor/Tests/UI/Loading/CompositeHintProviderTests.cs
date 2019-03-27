using BattleCruisers.Data.Models;
using BattleCruisers.UI.Loading;
using BattleCruisers.Utils;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Loading
{
    public class CompositeHintProviderTests
    {
        private IHintProvider _compositHintProvider, _basicHints, _advancedHints;
        private IGameModel _gameModel;
        private IRandomGenerator _random;
        private string _basicHint, _advancedHint;

        [SetUp]
        public void TestSetup()
        {
            _basicHints = Substitute.For<IHintProvider>();
            _advancedHints = Substitute.For<IHintProvider>();
            _gameModel = Substitute.For<IGameModel>();
            _random = Substitute.For<IRandomGenerator>();

            _compositHintProvider = new CompositeHintProvider(_basicHints, _advancedHints, _gameModel, _random);

            _basicHint = "basic hint";
            _basicHints.GetHint().Returns(_basicHint);

            _advancedHint = "advanced hint";
            _advancedHints.GetHint().Returns(_advancedHint);
        }

        [Test]
        public void GetHint_EarlyInGame()
        {
            _gameModel.NumOfLevelsCompleted.Returns(CompositeHintProvider.ADVANCED_HINT_LEVEL_REQUIREMENT - 1);
            Assert.AreEqual(_basicHint, _compositHintProvider.GetHint());
        }

        [Test]
        public void GetHint_LateInGame_StillBasicHint()
        {
            _gameModel.NumOfLevelsCompleted.Returns(CompositeHintProvider.ADVANCED_HINT_LEVEL_REQUIREMENT + 1);
            _random.NextBool().Returns(false);
            Assert.AreEqual(_basicHint, _compositHintProvider.GetHint());
        }

        [Test]
        public void GetHint_LateInGame_AdvancedHint()
        {
            _gameModel.NumOfLevelsCompleted.Returns(CompositeHintProvider.ADVANCED_HINT_LEVEL_REQUIREMENT + 1);
            _random.NextBool().Returns(true);
            Assert.AreEqual(_advancedHint, _compositHintProvider.GetHint());
        }
    }
}