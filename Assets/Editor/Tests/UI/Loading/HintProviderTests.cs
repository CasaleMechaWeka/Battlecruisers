using BattleCruisers.Data.Models;
using BattleCruisers.UI.Loading;
using BattleCruisers.Utils;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Loading
{
    public class HintProviderTests
    {
        private GameModel _gameModel;
        private string _basicHint, _advancedHint;

        [SetUp]
        public void TestSetup()
        {
            _gameModel = Substitute.For<GameModel>();

            _basicHint = "basic hint";

            _advancedHint = "advanced hint";
        }

        [Test]
        public void GetHint_EarlyInGame()
        {
            _gameModel.NumOfLevelsCompleted.Returns(HintProvider.ADVANCED_HINT_LEVEL_REQUIREMENT - 1);
            Assert.AreEqual(_basicHint, HintProvider.GetHint());
        }

        [Test]
        public void GetHint_LateInGame_StillBasicHint()
        {
            _gameModel.NumOfLevelsCompleted.Returns(HintProvider.ADVANCED_HINT_LEVEL_REQUIREMENT + 1);
            RandomGenerator.NextBool().Returns(false);
            Assert.AreEqual(_basicHint, HintProvider.GetHint());
        }

        [Test]
        public void GetHint_LateInGame_AdvancedHint()
        {
            _gameModel.NumOfLevelsCompleted.Returns(HintProvider.ADVANCED_HINT_LEVEL_REQUIREMENT + 1);
            RandomGenerator.NextBool().Returns(true);
            Assert.AreEqual(_advancedHint, HintProvider.GetHint());
        }
    }
}