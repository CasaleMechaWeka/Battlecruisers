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

		public override IBuildableDetailsState SelectBuildable(Buildable selectedBuildable)
		{
			_buildableDetailsManager.ShowBuildableDetails(selectedBuildable);
			return new SelectedState(_buildableDetailsManager, selectedBuildable);
		}

		public override IBuildableDetailsState CompareSelectedBuildable()
		{
			return new ReadyToCompareState(_buildableDetailsManager, _selectedBuildable);
		}
	}
}

