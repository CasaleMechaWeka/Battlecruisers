using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.ProgressBars
{
    public class HealthBarController : BaseProgressBarController, IHealthBar
	{
		private IDamagable _damagable;
		private float _maxHealth;
        private bool _followDamagable;

        private Vector2 _offset;
		public Vector2 Offset
        {
            get => _offset;
            set
            {
                _offset = value;
                UpdatePosition();
            }
        }

		public void Initialise(IDamagable damagable, bool followDamagable = false)
		{
			Logging.Verbose(Tags.PROGRESS_BARS, damagable.ToString());

			Assert.IsNotNull(damagable);
			Assert.IsTrue(damagable.Health > 0);

			_damagable = damagable;
			_maxHealth = _damagable.Health;
			Offset = transform.position;
            _followDamagable = followDamagable;

			damagable.HealthChanged += Damagable_HealthChanged;
		}

		private void Damagable_HealthChanged(object sender, EventArgs e)
		{
			OnProgressChanged(_damagable.Health / _maxHealth);
		}

		void LateUpdate()
		{
			if (_followDamagable)
			{
                UpdatePosition();
			}
		}

        private void UpdatePosition()
        {
            Vector3 parentPosition = _damagable.GameObject.transform.position;
            transform.position 
                = new Vector3(
                    parentPosition.x + Offset.x,
                    parentPosition.y + Offset.y,
                    transform.position.z);
		}
	}
}
