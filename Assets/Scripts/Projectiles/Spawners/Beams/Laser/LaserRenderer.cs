using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Spawners.Beams.Laser
{
    public class LaserRenderer : ILaserRenderer
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

                    LaserVisibilityChanged?.Invoke(this, new LaserVisibilityChangedEventArgs(_isVisible));
                }
            }
        }

        public event EventHandler<LaserVisibilityChangedEventArgs> LaserVisibilityChanged;

        public LaserRenderer(LineRenderer lineRenderer)
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
