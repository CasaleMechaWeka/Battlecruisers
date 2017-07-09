using System;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
	public interface IFireIntervalProvider
	{
		float NextFireIntervalInS { get; }
	}

	public interface IFireIntervalManager
	{
		/// <summary>
		/// Automatically starts the next interval if the current interval is up.
		/// So two consecutive calls to could return "true" and "false" respectively.
		/// </summary>
		/// <returns><c>true</c> if the fire interval is up; otherwise, <c>false</c>.</returns>
		bool IsIntervalUp();
	}

	public class FireIntervalManager : MonoBehaviour, IFireIntervalManager
	{
		private IFireIntervalProvider _fireIntervalProvider;
		private float _currentFireIntervalInS;
		private float _timeSinceLastFireInS;

		public void Initialise(IFireIntervalProvider fireIntervalProvider)
		{
			_fireIntervalProvider = fireIntervalProvider;
			_currentFireIntervalInS = _fireIntervalProvider.NextFireIntervalInS;
			_timeSinceLastFireInS = float.MaxValue;
		}

		public bool IsIntervalUp()
		{
			if (_timeSinceLastFireInS >= _currentFireIntervalInS)
			{
				_timeSinceLastFireInS = 0;
				_currentFireIntervalInS = _fireIntervalProvider.NextFireIntervalInS;
				return true;
			}
			return false;
		}

		void Update()
		{
			_timeSinceLastFireInS += Time.deltaTime;
		}
	}
}

