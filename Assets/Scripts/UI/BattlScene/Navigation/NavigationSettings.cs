using System;

namespace BattleCruisers.UI.BattleScene.Navigation
{
	public class NavigationSettings : INavigationSettings
	{
		public bool AreTransitionsEnabled { get { return AreTransitionsEnabledFilter.IsMatch; } }
		public bool IsUserInputEnabled { get { return IsUserInputEnabledFilter.IsMatch; } }

		private readonly BasicFilter _areTransitionsEnabledFilter;
		public IFilter AreTransitionsEnabledFilter { get { return _areTransitionsEnabledFilter; } }

		private readonly BasicFilter _isUserInputEnabledFilter;
		public IFilter IsUserInputEnabledFilter { get { return _isUserInputEnabledFilter; } }
        
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
			_areTransitionsEnabledFilter = new BasicFilter(isMatch: true);
			_isUserInputEnabledFilter = new BasicFilter(isMatch: true);
		}
	}
}
