using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails
{
	public class DismissedState : IBuildableDetailsState
	{
		private BuildableDetailsManager _buildableDetailsManager;

		public DismissedState(BuildableDetailsManager buildableDetailsManager)
		{
			_buildableDetailsManager = buildableDetailsManager;
		}

		public IBuildableDetailsState SelectBuildable(Buildable selectedBuildable)
		{
			_buildableDetailsManager.ShowBuildableDetails(selectedBuildable);
			return new SelectedState(_buildableDetailsManager, selectedBuildable);
		}

		public IBuildableDetailsState CompareSelectedBuildable()
		{
			throw new InvalidProgramException();
		}

		public IBuildableDetailsState Dismiss()
		{
			throw new InvalidProgramException();
		}
	}
}

