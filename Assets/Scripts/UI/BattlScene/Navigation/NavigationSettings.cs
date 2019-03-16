using BattleCruisers.UI.Filters;
using System;

namespace BattleCruisers.UI.BattleScene.Navigation
{
    public class NavigationSettings : INavigationSettings
	{
		public bool AreTransitionsEnabled => AreTransitionsEnabledFilter.IsMatch;
		public bool IsUserInputEnabled => IsUserInputEnabledFilter.IsMatch;

		private readonly BroadcastingFilter _areTransitionsEnabledFilter;
		public IBroadcastingFilter AreTransitionsEnabledFilter => _areTransitionsEnabledFilter;

		private readonly BroadcastingFilter _isUserInputEnabledFilter;
		public IBroadcastingFilter IsUserInputEnabledFilter => _isUserInputEnabledFilter;
        
		public NavigationPermission Permission
		{
			set
			{
				switch (value)
				{
					case NavigationPermission.None:
						_areTransitionsEnabledFilter.IsMatch = false;
						_isUserInputEnabledFilter.IsMatch = false;
						break;

					case NavigationPermission.TransitionsOnly:
						_areTransitionsEnabledFilter.IsMatch = true;
                        _isUserInputEnabledFilter.IsMatch = false;
                        break;

					case NavigationPermission.UserInputOnly:
                        _areTransitionsEnabledFilter.IsMatch = false;
                        _isUserInputEnabledFilter.IsMatch = true;
                        break;

					case NavigationPermission.Both:
                        _areTransitionsEnabledFilter.IsMatch = true;
                        _isUserInputEnabledFilter.IsMatch = true;
                        break;

					default:
						throw new ArgumentException();
				}
			}
		}

		public NavigationSettings()
		{
			_areTransitionsEnabledFilter = new BroadcastingFilter(isMatch: true);
			_isUserInputEnabledFilter = new BroadcastingFilter(isMatch: true);
		}
	}
}
