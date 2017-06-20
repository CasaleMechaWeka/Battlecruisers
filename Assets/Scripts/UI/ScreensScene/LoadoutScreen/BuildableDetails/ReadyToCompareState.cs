using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails
{
	public class ReadyToCompareState : BaseState
	{
		private BuildableLoadoutItem _buildableToCompare;

		public ReadyToCompareState(BuildableDetailsManager buildableDetailsManager, BuildableLoadoutItem buildableToCompare)
			: base(buildableDetailsManager)
		{
			_buildableToCompare = buildableToCompare;
		}

		public override IBuildableDetailsState SelectBuildable(BuildableLoadoutItem selectedBuildable)
		{
			_buildableToCompare.ShowSelectedFeedback = false;
			_buildableDetailsManager.CompareBuildableDetails(_buildableToCompare.Buildable, selectedBuildable.Buildable);
			return new ComparingState(_buildableDetailsManager);
		}
	}
}

