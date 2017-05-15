using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.ProgressBars
{
	public class HealthBarController : BaseProgressBarController
	{
		private IDamagable _damagable;
		private float _maxHealth;
		private Vector3 _offset;

		public void Initialise(IDamagable damagable)
		{
			Logging.Log(Tags.PROGRESS_BARS, $"Initialise()  {damagable}");

			Assert.IsNotNull(damagable);
			Assert.IsTrue(damagable.Health > 0);

			_damagable = damagable;
			_maxHealth = _damagable.Health;
			_offset = transform.position;

			damagable.HealthChanged += Damagable_HealthChanged;
		}

		private void Damagable_HealthChanged(object sender, HealthChangedEventArgs e)
		{
			OnProgressChanged(e.NewHealth / _maxHealth);
		}

		void LateUpdate()
		{
			transform.position = _damagable.GameObject.transform.position + _offset;
		}
	}
}
