using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Spawners.Beams.Laser
{
    public class PvPLaserRenderer : IPvPLaserRenderer
    {
        private readonly LineRenderer _lineRenderer;

        private bool _isVisible;
        private bool IsVisible
        {
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    _lineRenderer.enabled = _isVisible;

                    LaserVisibilityChanged?.Invoke(this, new PvPLaserVisibilityChangedEventArgs(_isVisible));
                }
            }
        }

        public event EventHandler<PvPLaserVisibilityChangedEventArgs> LaserVisibilityChanged;

        public PvPLaserRenderer(LineRenderer lineRenderer)
        {
            Assert.IsNotNull(lineRenderer);
            _lineRenderer = lineRenderer;
            _lineRenderer.positionCount = 2;

            _isVisible = false;
        }

        public void ShowLaser(Vector2 source, Vector2 target)
        {
            IsVisible = true;
            _lineRenderer.SetPosition(0, source);
            _lineRenderer.SetPosition(1, target);
        }

        public void HideLaser()
        {
            IsVisible = false;
        }
    }
}
