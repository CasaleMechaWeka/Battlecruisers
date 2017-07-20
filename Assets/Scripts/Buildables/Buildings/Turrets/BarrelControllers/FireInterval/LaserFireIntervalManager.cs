using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    // FELIX  Avoid duplicate code with FireIntervalmManager
    public class LaserFireIntervalManager : MonoBehaviour, IFireIntervalManager
	{
		private LaserTurretStats _laserTurretStats;
		private float _currentFireIntervalInS;
		private float _timeSinceLastFireInS;
		private bool _inLaserBurst;
		private float _timeSinceLaserBurstStartInS;

		public void Initialise(LaserTurretStats laserTurretStats)
		{
			_laserTurretStats = laserTurretStats;
			_currentFireIntervalInS = _laserTurretStats.NextFireIntervalInS;
			_timeSinceLastFireInS = float.MaxValue;
			_inLaserBurst = false;
			_timeSinceLaserBurstStartInS = 0;
		}

		/// <returns>
		/// Always returns true while in the laser burst length.  Ie, if the laser burst length
		/// is 2 seconds, this method will return true during those two seconds.
		/// </returns>
		public bool ShouldFire()
		{
			bool isIntervalUp = false;

			if (_inLaserBurst)
			{
				if (_timeSinceLaserBurstStartInS <= _laserTurretStats.laserDurationInS)
				{
					isIntervalUp = true;
				}
				else
				{
					_timeSinceLaserBurstStartInS = 0;

					// Stop laser burst
					_inLaserBurst = false;
				}
			}
			else
			{
				if (_timeSinceLastFireInS >= _currentFireIntervalInS)
				{
					_timeSinceLastFireInS = 0;
					_currentFireIntervalInS = _laserTurretStats.NextFireIntervalInS;

					// Start laser burst
					_inLaserBurst = true;

					isIntervalUp = true;
				}
			}
			return isIntervalUp;
		}

		// FELIX  Create state classes to avoid if/else?
		void Update()
		{
			if (_inLaserBurst)
			{
				_timeSinceLaserBurstStartInS += Time.deltaTime;
			}
			else
			{
				_timeSinceLastFireInS += Time.deltaTime;
			}
		}
	}
}
