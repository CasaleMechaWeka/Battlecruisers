using BattleCruisers.Buildables;
using BattleCruisers.UI.BattleScene.ProgressBars;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.ProgressBars
{
    public class HealthDialTests
    {
        private IHealthDial<IDamagable> _healthDial;
        private IFillableImage _fillableImage;
        private IFilter<IDamagable> _visibilityFilter;
        private IDamagable _damagable1, _damagable2;

        [SetUp]
        public void TestSetup()
        {
            _fillableImage = Substitute.For<IFillableImage>();
            _visibilityFilter = Substitute.For<IFilter<IDamagable>>();

            _healthDial = new HealthDial<IDamagable>(_fillableImage, _visibilityFilter);

            _fillableImage.Received().IsVisible = false;

            _damagable1 = Substitute.For<IDamagable>();
            _damagable1.MaxHealth.Returns(10);
            _damagable1.Health.Returns(7);

            _damagable2 = Substitute.For<IDamagable>();
            _damagable2.MaxHealth.Returns(20);
            _damagable2.Health.Returns(10);
        }

        [Test]
        public void SetDamagable_DoesNotMatchVisibilityFilter_DoesNothing()
        {
            _visibilityFilter.IsMatch(_damagable1).Returns(false);
            _healthDial.Damagable = _damagable1;
            Assert.IsFalse(_fillableImage.IsVisible);
        }

        [Test]
        public void HealthChanged_WasNotMatch_DoesNothing()
        {
            SetDamagable_DoesNotMatchVisibilityFilter_DoesNothing();

            _damagable1.HealthChanged += Raise.Event();
            _fillableImage.DidNotReceiveWithAnyArgs().FillAmount = default;
        }

        [Test]
        public void SetDamagable_MatchesVisibilityFilter_ShowsHealth()
        {
            _visibilityFilter.IsMatch(_damagable1).Returns(true);

            _healthDial.Damagable = _damagable1;

            Assert.IsTrue(_fillableImage.IsVisible);
            CheckFillAmount(_damagable1);
        }

        [Test]
        public void HealthChanged_WasMatch_UpdatesDial()
        {
            // Set valid damagable that will show health dial
            SetDamagable_MatchesVisibilityFilter_ShowsHealth();

            _damagable1.Health.Returns(5);
            _damagable1.HealthChanged += Raise.Event();

            CheckFillAmount(_damagable1);
        }

        [Test]
        public void SetDamagable_ReplacesExistingDamagable()
        {
            // Set valid damagable that will show health dial
            SetDamagable_MatchesVisibilityFilter_ShowsHealth();

            _visibilityFilter.IsMatch(_damagable2).Returns(true);
            _healthDial.Damagable = _damagable2;
            CheckFillAmount(_damagable2);
            _fillableImage.ClearReceivedCalls();

            // Old damagable health changes are ignored
            _damagable1.HealthChanged += Raise.Event();
            _fillableImage.DidNotReceiveWithAnyArgs().FillAmount = default;

            // New damagable health changes are processed
            _damagable2.Health.Returns(12);
            _damagable2.HealthChanged += Raise.Event();
            CheckFillAmount(_damagable2);
        }

        [Test]
        public void SetDamagable_Null_HidesDial()
        {
            // Set valid damagable that will show health dial
            SetDamagable_MatchesVisibilityFilter_ShowsHealth();

            _healthDial.Damagable = null;
            Assert.IsFalse(_fillableImage.IsVisible);
        }

        private void CheckFillAmount(IDamagable damagable)
        {
            float expectedProportion = damagable.Health / damagable.MaxHealth;
            Assert.AreEqual(expectedProportion, _fillableImage.FillAmount);
        }
    }
}