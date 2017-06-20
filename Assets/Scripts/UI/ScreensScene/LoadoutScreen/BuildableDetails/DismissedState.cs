using BattleCruisers.Buildables;
using System;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails
{
	public class DismissedState : BaseState
	{
		public DismissedState(BuildableDetailsManager buildbleDetailsManager)
			: base(buildbleDetailsManager) { }

		public override IBuildableDetailsState SelectBuildable(BuildableLoadoutItem selectedBuildable)
		{
			_buildableDetailsManager.ShowBuildableDetails(selectedBuildable.Buildable);
			return new SelectedState(_buildableDetailsManager, selectedBuildable);
		}

		public override IBuildableDetailsState Dismiss()
		{
			throw new InvalidProgramException();
		}
	}
}

