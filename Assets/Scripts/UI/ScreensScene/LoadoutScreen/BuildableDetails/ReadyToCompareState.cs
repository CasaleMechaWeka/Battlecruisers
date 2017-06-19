using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails
{
	public class ReadyToCompareState : BaseState
	{
		private Buildable _buildableToCompare;

		public ReadyToCompareState(BuildableDetailsManager buildableDetailsManager, Buildable buildableToCompare)
			: base(buildableDetailsManager)
		{
			_buildableToCompare = buildableToCompare;
		}

		public IBuildableDetailsState SelectBuildable(Buildable selectedBuildable)
		{
			_buildableDetailsManager.CompareBuildableDetails(_buildableToCompare, selectedBuildable);
			return new ComparingState(_buildableDetailsManager);
		}
	}
}

