using BattleCruisers.Buildables;

namespace BattleCruisers.UI.BattleScene.ProgressBars
{
    public class BuildableProgressBarController : BaseProgressBarController
	{
		private IBuildable _buildable;
        public IBuildable Buildable
        {
            private get { return _buildable; }
            set
            {
                if (_buildable != null)
                {
                    _buildable.BuildableProgress -= Buildable_BuildableProgress;
                }

                _buildable = value;

                if (_buildable != null)
                {
                    _buildable.BuildableProgress += Buildable_BuildableProgress;
					UpdateVisibility();
                }
            }
        }

		private void Buildable_BuildableProgress(object sender, BuildProgressEventArgs e)
		{
			OnProgressChanged(e.Buildable.BuildProgress);
		}

        private void UpdateVisibility()
        {
            OnProgressChanged(_buildable.BuildProgress);
        }
	}
}
