using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.Utils.BattleScene;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene
{
    public abstract class ModalManagerTestsBase
    {
        protected IPauseGameManager _pauseGameManager;
        protected INavigationPermitterManager _navigationPermitterManager;

        [SetUp]
        public virtual void TestSetup()
        {
            _pauseGameManager = Substitute.For<IPauseGameManager>();
            _navigationPermitterManager = Substitute.For<INavigationPermitterManager>();
        }
    }
}