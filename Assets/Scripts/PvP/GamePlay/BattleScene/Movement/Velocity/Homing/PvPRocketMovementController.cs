using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.FlightPoints;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders;
using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity.Homing
{
    /// <summary>
    /// 1. Moves above source to cruising altitude
    /// 2. Moved horizontally towards target
    /// 3. Once above target drops down to hit target
    /// </summary>
    public class PvPRocketMovementController : PvPHomingMovementController
    {
        private readonly float _cruisingAltitudeInM;
        private readonly float _cruisingAltitidueMarginInM;

        private Queue<Vector2> _flightPoints;
        private Vector2 _currentTargetPoint;

        private const float CRUISING_ALTITUDE_MARGIN_PROPORTION = 0.25f;
        private const float MIN_SMOOTH_TIME_IN_S = 1;
        private const float MAX_SMOOTH_TIME_IN_S = 10;

        public PvPRocketMovementController(
            Rigidbody2D rigidBody,
            IPvPVelocityProvider maxVelocityProvider,
            IPvPTargetProvider targetProvider,
            float cruisingAltitudeInM,
            IPvPFlightPointsProvider flightPointsProvider)
            : base(rigidBody, maxVelocityProvider, targetProvider)
        {
            Assert.IsTrue(cruisingAltitudeInM > rigidBody.position.y);

            _cruisingAltitudeInM = cruisingAltitudeInM;
            _cruisingAltitidueMarginInM = _cruisingAltitudeInM * CRUISING_ALTITUDE_MARGIN_PROPORTION;

            IPvPTarget target = _targetProvider.Target;
            Assert.IsNotNull(target);

            _flightPoints = flightPointsProvider.FindFlightPoints(_rigidBody.position, target.Position, cruisingAltitudeInM);

            _currentTargetPoint = _flightPoints.Dequeue();
        }

        protected override Vector2 FindTargetPosition()
        {
            Assert.IsNotNull(_flightPoints, "FindTargetPosition() called before OnTargetSet() :(");

            float distanceFromCurrentTargetPoint = Vector2.Distance(_rigidBody.position, _currentTargetPoint);
            if (distanceFromCurrentTargetPoint <= _cruisingAltitidueMarginInM
                && _flightPoints.Count != 0)
            {
                _currentTargetPoint = _flightPoints.Dequeue();
            }

            return _currentTargetPoint;
        }

        protected override float FindVelocitySmoothTime()
        {
            if (Velocity.magnitude == 0)
            {
                return MAX_SMOOTH_TIME_IN_S;
            }

            float smoothTimeInS = _maxVelocityProvider.VelocityInMPerS / Velocity.magnitude;
            float clampedSmoothTimeInS = Mathf.Clamp(smoothTimeInS, MIN_SMOOTH_TIME_IN_S, MAX_SMOOTH_TIME_IN_S);

            Logging.Verbose(Tags.MOVEMENT, $"clampedSmoothTimeInS: {clampedSmoothTimeInS}  smoothTimeInS: {smoothTimeInS}");
            return clampedSmoothTimeInS;
        }
    }
}