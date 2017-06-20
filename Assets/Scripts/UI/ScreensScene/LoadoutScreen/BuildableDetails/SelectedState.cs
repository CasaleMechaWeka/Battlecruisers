using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails
{
	public class SelectedState : BaseState
	{
		private BuildableLoadoutItem _selectedBuildable;

		public SelectedState(BuildableDetailsManager buildableDetailsManager, BuildableLoadoutItem selectedBuildable)
			: base(buildableDetailsManager)
		{
			_selectedBuildable = selectedBuildable;
		}

		public override IBuildableDetailsState CompareSelectedBuildable()
		{
			_buildableDetailsManager.HideBuildableDetails();
			_selectedBuildable.ShowSelectedFeedback = true;	
			return new ReadyToCompareState(_buildableDetailsManager, _selectedBuildable);
		}
	}
}

