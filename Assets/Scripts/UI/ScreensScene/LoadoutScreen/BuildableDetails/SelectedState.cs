using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails
{
	public class SelectedState : IBuildableDetailsState
	{
		private BuildableDetailsManager _buildableDetailsManager;
		private Buildable _selectedBuildable;

		public SelectedState(BuildableDetailsManager buildableDetailsManager, Buildable selectedBuildable)
		{
			_buildableDetailsManager = buildableDetailsManager;
			_selectedBuildable = selectedBuildable;
		}

		public IBuildableDetailsState SelectBuildable(Buildable selectedBuildable)
		{
			_buildableDetailsManager.ShowBuildableDetails(selectedBuildable);
			return new SelectedState(_buildableDetailsManager, selectedBuildable);
		}

		public IBuildableDetailsState CompareSelectedBuildable()
		{
			// FELIX  new state
			throw new InvalidProgramException();
		}

		public IBuildableDetailsState Dismiss()
		{
			_buildableDetailsManager.HideBuildableDetails();
			return new DismissedState(_buildableDetailsManager);
		}
	}
}

