using BattleCruisers.Buildables;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Buildables
{
    public class HealthTrackerTests
    {
        private IHealthTracker _healthTracker;
        private int _healthChangedCounter, _healthGoneCounter;
        private const float MAX_HEALTH = 100;

        [SetUp]
        public void TestSetup()
        {
            _healthTracker = new HealthTracker(MAX_HEALTH);

            _healthChangedCounter = 0;
            _healthTracker.HealthChanged += (sender, e) => _healthChangedCounter++;

            _healthGoneCounter = 0;
            _healthTracker.HealthGone += (sender, e) => _healthGoneCounter++;

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void InitialState()
        {
            Assert.AreEqual(MAX_HEALTH, _healthTracker.MaxHealth);
            Assert.AreEqual(MAX_HEALTH, _healthTracker.Health);
        }

        #region RemoveHealth()
        [Test]
        public void RemoveHealth_NonPositive_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _healthTracker.RemoveHealth(0));
        }

        [Test]
        public void RemoveHealth_AlreadyNoHealthLeft()
        {
            // Reduce health to 0
            _healthTracker.RemoveHealth(_healthTracker.MaxHealth);
            _healthChangedCounter = 0;
            _healthGoneCounter = 0;

            Assert.IsFalse(_healthTracker.RemoveHealth(1));
            Assert.AreEqual(0, _healthTracker.Health);
            Assert.AreEqual(0, _healthChangedCounter);
            Assert.AreEqual(0, _healthGoneCounter);
        }

        [Test]
        public void RemoveHealth_ImmutableState()
        {
            _healthTracker.State = HealthTrackerState.Immutable;

            Assert.IsFalse(_healthTracker.RemoveHealth(MAX_HEALTH));
            Assert.AreEqual(MAX_HEALTH, _healthTracker.Health);
            Assert.AreEqual(0, _healthChangedCounter);
            Assert.AreEqual(0, _healthGoneCounter);
        }

        [Test]
        public void RemoveHealth_SomeHealthLeft()
        {
            Assert.IsTrue(_healthTracker.RemoveHealth(1));
            Assert.AreEqual(MAX_HEALTH - 1, _healthTracker.Health);
            Assert.AreEqual(1, _healthChangedCounter);
            Assert.AreEqual(0, _healthGoneCounter);
        }

        [Test]
        public void RemoveHealth_NoHealthLeft()
        {
            Assert.IsTrue(_healthTracker.RemoveHealth(MAX_HEALTH));
            Assert.AreEqual(0, _healthTracker.Health);
            Assert.AreEqual(1, _healthChangedCounter);
            Assert.AreEqual(1, _healthGoneCounter);
        }
        #endregion RemoveHealth()

        #region AddHealth()
        [Test]
        public void AddHealth_NonPositveAmount_Throws()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _healthTracker.AddHealth(0));
        }

        [Test]
        public void AddHealth_AlreadyMaxHealth()
        {
            Assert.IsFalse(_healthTracker.AddHealth(1));
            Assert.AreEqual(MAX_HEALTH, _healthTracker.Health);
            Assert.AreEqual(0, _healthChangedCounter);
        }

        [Test]
        public void AddHealth_ImmutableState()
        {
            _healthTracker.RemoveHealth(1);
            Assert.AreEqual(MAX_HEALTH - 1, _healthTracker.Health);
            _healthChangedCounter = 0;
            _healthTracker.State = HealthTrackerState.Immutable;

            Assert.IsFalse(_healthTracker.AddHealth(1));
            Assert.AreEqual(MAX_HEALTH - 1, _healthTracker.Health);
            Assert.AreEqual(0, _healthChangedCounter);
        }

        [Test]
        public void AddHealth()
        {
            _healthTracker.RemoveHealth(1);
            Assert.AreEqual(MAX_HEALTH - 1, _healthTracker.Health);
            _healthChangedCounter = 0;

            Assert.IsTrue(_healthTracker.AddHealth(1));
            Assert.AreEqual(MAX_HEALTH, _healthTracker.Health);
            Assert.AreEqual(1, _healthChangedCounter);

        }
        #endregion AddHealth()

        [Test]
        public void SetMinHealth()
        {
            _healthTracker.SetMinHealth();

            Assert.AreEqual(HealthTracker.MIN_HEALTH, _healthTracker.Health);
            Assert.AreEqual(1, _healthChangedCounter);
        }
    }
}