using BattleCruisers.Buildables;

namespace BattleCruisers.UI.BattleScene.ProgressBars
{
    public class BuildableProgressBarController : BaseProgressBarController
	{
		private IBuildable _buildable;

		public void Initialise(IBuildable buildable)
		{
			_buildable = buildable;
			_buildable.BuildableProgress += Buildable_BuildableProgress;
		}

		private void Buildable_BuildableProgress(object sender, BuildProgressEventArgs e)
		{
			OnProgressChanged(e.Buildable.BuildProgress);
		}

		public void Cleanup()
		{
			_buildable.BuildableProgress -= Buildable_BuildableProgress;
		}
	}
}
