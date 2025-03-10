using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Buildables.BuildProgress
{
    public class BuildProgressFeedback : IBuildProgressFeedback
    {
        private readonly IFillableImage _buildProgressImage;
        private readonly IGameObject _pausedFeedback;
        private Image _unitImage;

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

                    ShowBuildProgress(_currentBuildable.BuildProgress);
                }
            }
        }

        private IFactory _currentFactory;
        private IFactory CurrentFactory
        {
            set
            {
                if (_currentFactory != null)
                {
                    _currentFactory.IsUnitPaused.ValueChanged -= _currentFactory_IsUnitPausedChanged;
                }

                _currentFactory = value;

                if (_currentFactory != null)
                {
                    _currentFactory.IsUnitPaused.ValueChanged += _currentFactory_IsUnitPausedChanged;
                    _pausedFeedback.IsVisible = _currentFactory.IsUnitPaused.Value;
                    if (_currentFactory.IsUnitPaused.Value)
                    {
                        _unitImage.color = Color.clear;
                    }
                    else{
                        _unitImage.color = Color.black;
                    }
                }
            }
        }

        public BuildProgressFeedback(IFillableImage buildProgressImage, IGameObject pausedFeedback, Image unitImage)
        {
            Helper.AssertIsNotNull(buildProgressImage, pausedFeedback);

            _buildProgressImage = buildProgressImage;
            _pausedFeedback = pausedFeedback;
            _unitImage = unitImage;
            HideBuildProgress();
        }

        private void _currentBuildable_BuildableProgress(object sender, BuildProgressEventArgs e)
        {
            ShowBuildProgress(_currentBuildable.BuildProgress);
        }

        private void ShowBuildProgress(float buildProgress)
        {
            _buildProgressImage.FillAmount = buildProgress;
        }

        private void _currentBuildable_CompletedBuildable(object sender, System.EventArgs e)
        {
            _buildProgressImage.FillAmount = 0;
        }

        private void _currentBuildable_Destroyed(object sender, DestroyedEventArgs e)
        {
            _buildProgressImage.FillAmount = 0;
        }

        private void _currentFactory_IsUnitPausedChanged(object sender, System.EventArgs e)
        {
            _pausedFeedback.IsVisible = _currentFactory.IsUnitPaused.Value;
            if (_currentFactory.IsUnitPaused.Value)
            {
                _unitImage.color = Color.clear;
            }
            else{
                _unitImage.color = Color.black;
            }
             
        }

        public void ShowBuildProgress(IBuildable buildable, IFactory buildableFactory)
        {
            Helper.AssertIsNotNull(buildable, buildableFactory);
            Assert.AreNotEqual(BuildableState.Completed, buildable.BuildableState);

            CurrentBuildable = buildable;
            CurrentFactory = buildableFactory;

            _buildProgressImage.IsVisible = true;
        }

        public void HideBuildProgress()
        {
            CurrentBuildable = null;
            CurrentFactory = null;

            _buildProgressImage.IsVisible = false;
            _pausedFeedback.IsVisible = false;
            _unitImage.color = Color.black;
        }
    }
}