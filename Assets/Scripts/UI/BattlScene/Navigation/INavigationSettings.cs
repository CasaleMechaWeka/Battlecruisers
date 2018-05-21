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

		IFilter AreTransitionsEnabledFilter { get; }
		IFilter IsUserInputEnabledFilter { get; }

		NavigationPermission Permission { set; }
	}
}
