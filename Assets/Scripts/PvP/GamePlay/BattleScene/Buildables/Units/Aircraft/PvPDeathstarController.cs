using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.Localisation;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public class PvPDeathstarController : PvPSatelliteController
    {
        private IPvPBarrelWrapper _barrelWrapper;

        public PvPRotatingController leftWing, rightWing;

        private const float LEFT_WING_TARGET_ANGLE_IN_DEGREES = 270;
        private const float RIGHT_WING_TARGET_ANGLE_IN_DEGREES = 90;
        private const float WING_ROTATE_SPEED_IN_M_DEGREES_S = 45;

        public override PvPTargetType TargetType => PvPTargetType.Satellite;

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar, ILocTable commonStrings)
        {
            base.StaticInitialise(parent, healthBar, commonStrings);

            PvPHelper.AssertIsNotNull(leftWing, rightWing);

            _barrelWrapper = gameObject.GetComponentInChildren<IPvPBarrelWrapper>();
            Assert.IsNotNull(_barrelWrapper);
            _barrelWrapper.StaticInitialise();
            AddDamageStats(_barrelWrapper.DamageCapability);
        }

        public override void Initialise(IPvPUIManager uiManager, IPvPFactoryProvider factoryProvider)
        {
            base.Initialise(uiManager, factoryProvider);

            leftWing.Initialise(_movementControllerFactory, WING_ROTATE_SPEED_IN_M_DEGREES_S, LEFT_WING_TARGET_ANGLE_IN_DEGREES);
            rightWing.Initialise(_movementControllerFactory, WING_ROTATE_SPEED_IN_M_DEGREES_S, RIGHT_WING_TARGET_ANGLE_IN_DEGREES);
        }

        protected override void OnBuildableCompleted()
        {
            base.OnBuildableCompleted();

            Assert.IsTrue(cruisingAltitudeInM > transform.position.y);

            _barrelWrapper.Initialise(this, _factoryProvider, _cruiserSpecificFactories);
        }

        protected override IList<IPvPPatrolPoint> GetPatrolPoints()
        {
            IList<Vector2> patrolPositions = _aircraftProvider.FindDeathstarPatrolPoints(transform.position, cruisingAltitudeInM);

            IList<IPvPPatrolPoint> patrolPoints = new List<IPvPPatrolPoint>(patrolPositions.Count)
            {
                new PvPPatrolPoint(patrolPositions[0], removeOnceReached: true, actionOnReached: OnClearingLaunchStation),
                new PvPPatrolPoint(patrolPositions[1], removeOnceReached: true)
            };

            for (int i = 2; i < patrolPositions.Count; ++i)
            {
                patrolPoints.Add(new PvPPatrolPoint(patrolPositions[i]));
            }

            return patrolPoints;
        }

        private void OnClearingLaunchStation()
        {
            // Stop moving
            ActiveMovementController = DummyMovementController;

            UnfoldWings();
        }

        private void UnfoldWings()
        {
            leftWing.ReachedDesiredAngle += Wing_ReachedDesiredAngle;

            leftWing.StartRotating();
            rightWing.StartRotating();
        }

        private void Wing_ReachedDesiredAngle(object sender, EventArgs e)
        {
            leftWing.ReachedDesiredAngle -= Wing_ReachedDesiredAngle;

            ActiveMovementController = PatrollingMovementController;
        }

        protected override void OnDirectionChange()
        {
            // Do not switch direction, as this flips the invisible turret barrel 
            // angle and means the laser doesn't work as well as it should :P
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
            _barrelWrapper.DisposeManagedState();
        }
    }
}
