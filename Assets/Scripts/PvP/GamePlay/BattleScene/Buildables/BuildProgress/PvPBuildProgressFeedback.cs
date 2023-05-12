using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.UI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress
{
    public class PvPBuildProgressFeedback : IPvPBuildProgressFeedback
    {
        private readonly IPvPFillableImage _buildProgressImage;
        private readonly IPvPGameObject _pausedFeedback;
        private Image _unitImage;

        private IPvPBuildable _currentBuildable;
        private IPvPBuildable CurrentBuildable
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

        private IPvPFactory _currentFactory;
        private IPvPFactory CurrentFactory
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
                    else
                    {
                        _unitImage.color = Color.black;
                    }
                }
            }
        }

        public PvPBuildProgressFeedback(IPvPFillableImage buildProgressImage, IPvPGameObject pausedFeedback, Image unitImage)
        {
            PvPHelper.AssertIsNotNull(buildProgressImage, pausedFeedback);

            _buildProgressImage = buildProgressImage;
            _pausedFeedback = pausedFeedback;
            _unitImage = unitImage;
            HideBuildProgress();
        }

        private void _currentBuildable_BuildableProgress(object sender, PvPBuildProgressEventArgs e)
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

        private void _currentBuildable_Destroyed(object sender, PvPDestroyedEventArgs e)
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
            else
            {
                _unitImage.color = Color.black;
            }

        }

        public void ShowBuildProgress(IPvPBuildable buildable, IPvPFactory buildableFactory)
        {
            PvPHelper.AssertIsNotNull(buildable, buildableFactory);
            Assert.AreNotEqual(PvPBuildableState.Completed, buildable.BuildableState);

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