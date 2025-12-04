using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using BattleCruisers.Ads;

namespace BattleCruisers.Tests.Analytics
{
    /// <summary>
    /// Lightweight EditMode tests for FirebaseAnalyticsManager.
    /// These do NOT talk to Firebase; they just verify our wrapper logic and gating.
    /// </summary>
    public class FirebaseAnalyticsManagerTests
    {
        private GameObject _go;
        private FirebaseAnalyticsManager _manager;
        private FieldInfo _isInitializedField;
        private FieldInfo _sessionEventCountField;

        [SetUp]
        public void SetUp()
        {
            _go = new GameObject("FirebaseAnalyticsManager_Test");
            _manager = _go.AddComponent<FirebaseAnalyticsManager>();

            var type = typeof(FirebaseAnalyticsManager);
            _isInitializedField = type.GetField("isInitialized", BindingFlags.NonPublic | BindingFlags.Instance);
            _sessionEventCountField = type.GetField("sessionEventCount", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.IsNotNull(_isInitializedField, "Failed to reflect isInitialized field");
            Assert.IsNotNull(_sessionEventCountField, "Failed to reflect sessionEventCount field");
        }

        [TearDown]
        public void TearDown()
        {
            if (_go != null)
            {
                Object.DestroyImmediate(_go);
            }
        }

        private void SetInitialized(bool initialized)
        {
            _isInitializedField.SetValue(_manager, initialized);
        }

        private int GetSessionEventCount()
        {
            return (int)_sessionEventCountField.GetValue(_manager);
        }

        private void SetSessionEventCount(int value)
        {
            _sessionEventCountField.SetValue(_manager, value);
        }

        [Test]
        public void LogEvent_NotInitialized_DoesNotIncrementSessionCount()
        {
            SetInitialized(false);
            SetSessionEventCount(0);

            _manager.LogEvent("test_event_not_initialized");

            Assert.AreEqual(0, GetSessionEventCount(), "sessionEventCount should not change when not initialized");
        }

        [Test]
        public void LogEvent_Initialized_IncrementsSessionCount()
        {
            SetInitialized(true);
            SetSessionEventCount(0);

            _manager.LogEvent("event_1");
            _manager.LogEvent("event_2");

            Assert.AreEqual(2, GetSessionEventCount(), "sessionEventCount should increment once per logged event");
        }

        [Test]
        public void LevelEvents_IncrementSessionEventCount()
        {
            SetInitialized(true);
            SetSessionEventCount(0);

            _manager.LogLevelStart("level_1", 1, "campaign", 1);
            _manager.LogLevelComplete("level_1", 1, "campaign", 123.4f, 999);
            _manager.LogLevelFail("level_1", 1, "campaign", 45.6f, "timeout");

            Assert.AreEqual(
                3,
                GetSessionEventCount(),
                "Each level wrapper (start/complete/fail) should log exactly one event");
        }

        [Test]
        public void AdEvents_IncrementSessionEventCount()
        {
            SetInitialized(true);
            SetSessionEventCount(0);

            _manager.LogAdImpression("applovin", "interstitial", "hub_banner");
            _manager.LogAdClosed("applovin", "interstitial", true);
            _manager.LogAdClicked("applovin", "interstitial");
            _manager.LogRewardedAdOffered("shop_reward", 50, 10);
            _manager.LogRewardedAdStarted("shop_reward");
            _manager.LogRewardedAdCompleted("shop_reward", 50, 10);
            _manager.LogRewardedAdSkipped("shop_reward");

            Assert.AreEqual(
                7,
                GetSessionEventCount(),
                "Each ad wrapper should log exactly one event");
        }

        [Test]
        public void MonetizationEvents_IncrementSessionEventCount()
        {
            SetInitialized(true);
            SetSessionEventCount(0);

            _manager.LogIAPAttempt("coins_pack_1", "consumable", 4.99, "USD");
            _manager.LogIAPSuccess("coins_pack_1", "consumable", 4.99, "USD");
            _manager.LogIAPFailed("coins_pack_1", "network_error");

            Assert.AreEqual(
                3,
                GetSessionEventCount(),
                "Each IAP wrapper should log exactly one event");
        }
    }
}


