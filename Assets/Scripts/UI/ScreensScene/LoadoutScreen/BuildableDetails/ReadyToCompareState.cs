using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails
{
	public class ReadyToCompareState : IBuildableDetailsState
	{
		private BuildableDetailsManager _buildableDetailsManager;
		private Buildable _buildableToCompare;

		public ReadyToCompareState(BuildableDetailsManager buildableDetailsManager, Buildable buildableToCompare)
		{
			_buildableDetailsManager = buildableDetailsManager;
			_buildableToCompare = buildableToCompare;
		}

		public IBuildableDetailsState SelectBuildable(Buildable selectedBuildable)
		{
			_buildableDetailsManager.CompareBuildableDetails(_buildableToCompare, selectedBuildable);
			return new ComparingState(_buildableDetailsManager);
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

