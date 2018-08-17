using BattleCruisers.UI.Filters;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public enum NavigationPermission
	{
		None, TransitionsOnly, UserInputOnly, Both
	}

	public interface INavigationSettings
	{
		bool AreTransitionsEnabled { get; }
		bool IsUserInputEnabled { get; }

		IBroadcastingFilter AreTransitionsEnabledFilter { get; }
		IBroadcastingFilter IsUserInputEnabledFilter { get; }

		NavigationPermission Permission { set; }
	}
}
