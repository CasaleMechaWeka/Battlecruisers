using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.Buildings
{
	public class HealthBarController : MonoBehaviour 
	{
		public Image healthBarOutline;
		public Image remainingHealth;

		private float _outlineWidth;
		private float _maxHealth;

		private float _health;
		public float Health
		{
			set
			{
				if (value < MIN_HEALTH)
				{
					_health = MIN_HEALTH;
				}
				else if (value > _maxHealth)
				{
					_health = _maxHealth;
				}
				else if (_health != value)
				{
					_health = value;
					UpdateRemainingHealth(_health);
				}
			}
		}

		private const float MIN_HEALTH = 0;

		void Awake()
		{
			_outlineWidth = ((RectTransform)healthBarOutline.transform).rect.width;
		}

		public void Initialise(float maxHealth)
		{
			Debug.Log($"HealthBarController.Initialise()  maxHealth: {maxHealth}");

			Assert.IsTrue(maxHealth > MIN_HEALTH);
			_maxHealth = maxHealth;
		}

		private void UpdateRemainingHealth(float health)
		{
			Debug.Log($"HealthBarController.UpdateRemainingHealth()  health: {health}");

			RectTransform newHealth = (RectTransform)remainingHealth.transform;
			newHealth.sizeDelta = new Vector2((health / _maxHealth) * _outlineWidth, newHealth.sizeDelta.y);
		}
	}
}
