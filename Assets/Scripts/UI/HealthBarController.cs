using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI
{
	public class HealthBarController : MonoBehaviour 
	{
		public Image healthBarOutline;
		public Image remainingHealth;

		private float _outlineWidth;
		private float _maxHealth;

		// FELIX  Remove _health backing field
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
				}
				UpdateRemainingHealth(_health);
			}
		}

		public float Progress
		{
			set
			{
				if (value < 0)
				{
					value = 0;
				}
				else if (value > 1)
				{
					value = 1;
				}
				UpdateProgress(value);
			}
		}

		private const float MIN_HEALTH = 0;

		void Awake()
		{
			_outlineWidth = ((RectTransform)healthBarOutline.transform).rect.width;
		}

		public void Initialise(float maxHealth)
		{
			Assert.IsTrue(maxHealth > MIN_HEALTH);
			_maxHealth = maxHealth;
		}

		private void UpdateRemainingHealth(float health)
		{
			UpdateProgress(health / _maxHealth);
		}
		
		private void UpdateProgress(float progress)
		{
			RectTransform newHealth = (RectTransform)remainingHealth.transform;
			newHealth.sizeDelta = new Vector2(progress * _outlineWidth, newHealth.sizeDelta.y);
		}
	}
}
