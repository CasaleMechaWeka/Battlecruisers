using System;
using BattleCruisers.Buildables;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen.BuildableDetails
{
	public interface IBuildableDetailsState
	{
		IBuildableDetailsState SelectBuildable(BuildableLoadoutItem selectedBuildable);
		IBuildableDetailsState CompareSelectedBuildable();
		IBuildableDetailsState Dismiss();
	}

	public abstract class BaseState : IBuildableDetailsState
	{
		protected BuildableDetailsManager _buildableDetailsManager;

		public BaseState(BuildableDetailsManager buildableDetailsManager)
		{
			_buildableDetailsManager = buildableDetailsManager;
		}

		public virtual IBuildableDetailsState SelectBuildable(BuildableLoadoutItem selectedBuildable)
		{
			throw new InvalidProgramException();
		}

		public virtual IBuildableDetailsState CompareSelectedBuildable()
		{
			throw new InvalidProgramException();
		}

		public virtual IBuildableDetailsState Dismiss()
		{
			_buildableDetailsManager.HideBuildableDetails();
			return new DismissedState(_buildableDetailsManager);
		}
	}
}

