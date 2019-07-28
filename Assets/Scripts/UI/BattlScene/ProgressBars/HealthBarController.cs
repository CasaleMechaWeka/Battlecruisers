using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.ProgressBars
{
    public class HealthBarController : BaseProgressBarController
	{
		private IDamagable _damagable;
		private float _maxHealth;
		private Vector3 _offset;
		private bool _followDamagable;

		public void Initialise(IDamagable damagable, bool followDamagable = false)
		{
			Logging.Verbose(Tags.PROGRESS_BARS, damagable.ToString());

			Assert.IsNotNull(damagable);
			Assert.IsTrue(damagable.Health > 0);

			_damagable = damagable;
			_maxHealth = _damagable.Health;
			_offset = transform.position;
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

		public void UpdateOffset(Vector2 offset)
		{
			_offset = offset;
            UpdatePosition();
		}

        private void UpdatePosition()
        {
			transform.position = _damagable.GameObject.transform.position + _offset;
		}
	}
}
