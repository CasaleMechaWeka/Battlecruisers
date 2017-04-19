using BattleCruisers.Buildables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI
{
	/// <summary>
	/// Shows an IDamagable's health when:
	/// 
	/// 1. They health is not at 100%
	/// AND
	/// 2. Their health is not 0
	/// 
	/// In either of these cases the health bar is hidden.
	/// </summary>
	public class HealthBarController : MonoBehaviour 
	{
		public Image healthBarOutline;
		public Image remainingHealth;

		private float _outlineWidth;
		private float _maxHealth;

		private bool AreImagesEnabled
		{
			get
			{
				return healthBarOutline.enabled && remainingHealth.enabled;
			}
		}

		private const float MIN_HEALTH = 0;

		void Awake()
		{
			_outlineWidth = ((RectTransform)healthBarOutline.transform).rect.width;
		}

		public void Initialise(IDamagable damagable)
		{
			_maxHealth = damagable.Health;

			damagable.FullyRepaired += Damagable_FullyRepaired;
			damagable.Destroyed += Damagable_Destroyed;
			damagable.HealthChanged += Damagable_HealthChanged;

			HideHealthBar();
		}

		private void Damagable_FullyRepaired(object sender, EventArgs e)
		{
			HideHealthBar();
		}

		private void Damagable_Destroyed(object sender, EventArgs e)
		{
			HideHealthBar();
		}

		private void Damagable_HealthChanged(object sender, HealthChangedEventArgs e)
		{
			if (!AreImagesEnabled && e.NewHealth != 0)
			{
				ShowHealthBar();
			}

			RectTransform newHealth = (RectTransform)remainingHealth.transform;
			newHealth.sizeDelta = new Vector2(e.NewHealth / _maxHealth * _outlineWidth, newHealth.sizeDelta.y);
		}

		private void ShowHealthBar()
		{
			EnableImages(true);
		}

		private void HideHealthBar()
		{
			EnableImages(false);
		}

		private void EnableImages(bool enabled)
		{
			healthBarOutline.enabled = enabled;
			remainingHealth.enabled = enabled;
		}
	}
}
