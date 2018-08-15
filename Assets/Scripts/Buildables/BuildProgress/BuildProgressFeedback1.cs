using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.BuildProgress
{
    // FELIX  Test & use :)
    public class BuildProgressFeedback : IBuildProgressFeedback
    {
        private readonly IFillabelImage _buildProgressImage;

        private IBuildable _currentBuildable;
        private IBuildable CurrentBuildable
        {
            set
            {
                if (_currentBuildable != null)
                {
                    _currentBuildable.BuildableProgress -= _currentBuildable_BuildableProgress;
                    _currentBuildable.CompletedBuildable -= _currentBuildable_CompletedBuildable;
                    _currentBuildable.Destroyed -= _currentBuildable_Destroyed;
                }

                _currentBuildable = value;

                if (_currentBuildable != null)
                {
                    _currentBuildable.BuildableProgress += _currentBuildable_BuildableProgress;
                    _currentBuildable.CompletedBuildable += _currentBuildable_CompletedBuildable;
                    _currentBuildable.Destroyed += _currentBuildable_Destroyed;

                    _buildProgressImage.FillAmount = _currentBuildable.BuildProgress;
                }
            }
        }

        public BuildProgressFeedback(IFillabelImage buildProgressImage)
        {
            Assert.IsNotNull(buildProgressImage);
            _buildProgressImage = buildProgressImage;

            HideBuildProgress();
        }

        private void _currentBuildable_BuildableProgress(object sender, BuildProgressEventArgs e)
        {
            _buildProgressImage.FillAmount = _currentBuildable.BuildProgress;
        }

        private void _currentBuildable_CompletedBuildable(object sender, System.EventArgs e)
        {
            HideBuildProgress();
        }

        private void _currentBuildable_Destroyed(object sender, DestroyedEventArgs e)
        {
            HideBuildProgress();
        }

        public void ShowBuildProgress(IBuildable buildable)
        {
            Assert.IsNotNull(buildable);
            Assert.AreNotEqual(BuildableState.Completed, buildable.BuildableState);

            CurrentBuildable = buildable;
            _buildProgressImage.IsVisible = true;
        }

        public void HideBuildProgress()
        {
            CurrentBuildable = null;
            _buildProgressImage.IsVisible = false;
        }
    }
}