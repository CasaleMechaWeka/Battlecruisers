using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails
{
	public class SelectedState : BaseState
	{
		private Buildable _selectedBuildable;

		public SelectedState(BuildableDetailsManager buildableDetailsManager, Buildable selectedBuildable)
			: base(buildableDetailsManager)
		{
			_selectedBuildable = selectedBuildable;
		}

		public override IBuildableDetailsState CompareSelectedBuildable()
		{
			_buildableDetailsManager.HideBuildableDetails();
			return new ReadyToCompareState(_buildableDetailsManager, _selectedBuildable);
		}
	}
}

