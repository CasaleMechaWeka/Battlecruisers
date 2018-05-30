using BattleCruisers.UI.BattleScene.Navigation;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.Navigation
{
	public class NavigationSettingsTests
    {
		private INavigationSettings _settings;

        [SetUp]
        public void SetuUp()
        {
			_settings = new NavigationSettings();
        }

        [Test]
        public void Permission_Default()
        {
			AssertSettings(areTransitionsEnabled: true, isUserInputEnabled: true);
        }

        [Test]
        public void Permission_None()
        {
			_settings.Permission = NavigationPermission.None;
			AssertSettings(areTransitionsEnabled: false, isUserInputEnabled: false);
		}

        [Test]
        public void Permission_TransitionOnly()
        {
			_settings.Permission = NavigationPermission.TransitionsOnly;
            AssertSettings(areTransitionsEnabled: true, isUserInputEnabled: false);
        }

        [Test]
        public void Permission_UserInputOnly()
        {
			_settings.Permission = NavigationPermission.UserInputOnly;
            AssertSettings(areTransitionsEnabled: false, isUserInputEnabled: true);
        }

        [Test]
        public void Permission_Both()
        {
			_settings.Permission = NavigationPermission.Both;
			AssertSettings(areTransitionsEnabled: true, isUserInputEnabled: true);
        }

        private void AssertSettings(bool areTransitionsEnabled, bool isUserInputEnabled)
		{
			Assert.AreEqual(areTransitionsEnabled, _settings.AreTransitionsEnabled);
			Assert.AreEqual(areTransitionsEnabled, _settings.AreTransitionsEnabledFilter.IsMatch);
			Assert.AreEqual(isUserInputEnabled, _settings.IsUserInputEnabled);
			Assert.AreEqual(isUserInputEnabled, _settings.IsUserInputEnabledFilter.IsMatch);
		}
    }
}
