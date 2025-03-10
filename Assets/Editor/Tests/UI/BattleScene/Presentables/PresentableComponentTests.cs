using BattleCruisers.UI.BattleScene.Presentables;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.Presentables
{
    public class PresentableComponentTests
    {
        private IPresentableComponent _component;
        private IPresentable _presentable1, _presentable2;
        private object _activationArgs;
        private int _dismissedCount;

        [SetUp]
        public void TestSetup()
        {
            _component = new PresentableComponent();

            _presentable1 = Substitute.For<IPresentable>();
            _presentable2 = Substitute.For<IPresentable>();

            _component.AddChildPresentable(_presentable1);
            _component.AddChildPresentable(_presentable2);

            _activationArgs = "sweet args";

            _dismissedCount = 0;
            _component.Dismissed += (sender, e) => _dismissedCount++;
        }

        [Test]
        public void OnPresenting()
        {
            _component.OnPresenting(_activationArgs);

            _presentable1.Received().OnPresenting(_activationArgs);
            _presentable2.Received().OnPresenting(_activationArgs);
        }

        [Test]
        public void OnDismissing()
        {
            _component.OnDismissing();

            _presentable1.Received().OnDismissing();
            _presentable2.Received().OnDismissing();

            Assert.AreEqual(1, _dismissedCount);
        }
    }
}