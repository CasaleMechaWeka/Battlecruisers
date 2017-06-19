using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails
{
	public class ComparingState : IBuildableDetailsState
	{
		private BuildableDetailsManager _buildableDetailsManager;

		public ComparingState(BuildableDetailsManager buildableDetailsManager)
		{
			_buildableDetailsManager = buildableDetailsManager;
		}

		public IBuildableDetailsState SelectBuildable(Buildable selectedBuildable)
		{
			throw new InvalidProgramException();
		}

		public IBuildableDetailsState CompareSelectedBuildable()
		{
			throw new InvalidProgramException();
		}

		public IBuildableDetailsState Dismiss()
		{
			_buildableDetailsManager.HideBuildableDetails();
			return new DismissedState(_buildableDetailsManager);
		}
	}
}

