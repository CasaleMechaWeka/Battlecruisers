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
		private float _maxHealth;

		public void Initialise(IDamagable damagable)
		{
			Logging.Log(Tags.PROGRESS_BARS, $"Initialise()  {damagable}");

			_maxHealth = damagable.Health;
			damagable.HealthChanged += Damagable_HealthChanged;
		}

		private void Damagable_HealthChanged(object sender, HealthChangedEventArgs e)
		{
			OnProgressChanged(e.NewHealth / _maxHealth);
		}
	}
}
