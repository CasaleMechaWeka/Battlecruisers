using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails
{
	public class DismissedBuildableDetailsState : IBuildableDetailsState
	{
		private BuildableDetailsManager _buildableDetailsManager;

		public DismissedBuildableDetailsState(BuildableDetailsManager buildableDetailsManager)
		{
			_buildableDetailsManager = buildableDetailsManager;
		}

		public IBuildableDetailsState SelectBuildable(Buildable buildable)
		{
			_buildableDetailsManager.ShowBuildableDetails(buildable);
			// FELIX  Return SelectedState :)

			return null;
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

