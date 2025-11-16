using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Buildables.Units.Aircraft;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Movement.Velocity;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public class PvPDeathstarController : PvPSatelliteController
    {
        private IPvPBarrelWrapper _barrelWrapper;

        public RotatingController leftWing, rightWing;

        private const float LEFT_WING_TARGET_ANGLE_IN_DEGREES = 270;
        private const float RIGHT_WING_TARGET_ANGLE_IN_DEGREES = 90;
        private const float WING_ROTATE_SPEED_IN_M_DEGREES_S = 45;

        public override TargetType TargetType => TargetType.Satellite;

        public override void StaticInitialise(GameObject parent, PvPHealthBarController healthBar)
        {
            base.StaticInitialise(parent, healthBar);

            PvPHelper.AssertIsNotNull(leftWing, rightWing);

            _barrelWrapper = gameObject.GetComponentInChildren<IPvPBarrelWrapper>();
            Assert.IsNotNull(_barrelWrapper);
            _barrelWrapper.StaticInitialise();
            AddDamageStats(_barrelWrapper.DamageCapability);
        }

        public override void Initialise()
        {
            base.Initialise();
            leftWing.Initialise(WING_ROTATE_SPEED_IN_M_DEGREES_S, LEFT_WING_TARGET_ANGLE_IN_DEGREES);
            rightWing.Initialise(WING_ROTATE_SPEED_IN_M_DEGREES_S, RIGHT_WING_TARGET_ANGLE_IN_DEGREES);
        }

        public override void Initialise(PvPUIManager uiManager)
        {
            base.Initialise(uiManager);
            // sava added
            leftWing.Initialise(WING_ROTATE_SPEED_IN_M_DEGREES_S, LEFT_WING_TARGET_ANGLE_IN_DEGREES);
            rightWing.Initialise(WING_ROTATE_SPEED_IN_M_DEGREES_S, RIGHT_WING_TARGET_ANGLE_IN_DEGREES);
        }

        protected override void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();
                Assert.IsTrue(cruisingAltitudeInM > transform.position.y);
                _barrelWrapper.Initialise(this, _cruiserSpecificFactories);
                OnBuildableCompletedClientRpc();
            }
            else
            {
                OnBuildableCompleted_PvPClient();
                _barrelWrapper.Initialise(this);
            }
        }

        protected override IList<IPatrolPoint> GetPatrolPoints()
        {
            IList<Vector2> patrolPositions = _aircraftProvider.DeathstarPatrolPoints(transform.position, cruisingAltitudeInM);

            IList<IPatrolPoint> patrolPoints = new List<IPatrolPoint>(patrolPositions.Count)
            {
                new PatrolPoint(patrolPositions[0], removeOnceReached: true, actionOnReached: OnClearingLaunchStation),
                new PatrolPoint(patrolPositions[1], removeOnceReached: true)
            };

            for (int i = 2; i < patrolPositions.Count; ++i)
            {
                patrolPoints.Add(new PatrolPoint(patrolPositions[i]));
            }

            return patrolPoints;
        }

        private void OnClearingLaunchStation()
        {
            // Stop moving
            ActiveMovementController = DummyMovementController;
            UnfoldWings();
            UnfoldWingsClientRpc();
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

        protected override void OnBuildableProgressEvent()
        {
            if (IsServer)
                OnBuildableProgressEventClientRpc();
            else
                base.OnBuildableProgressEvent();
        }
        protected override void OnCompletedBuildableEvent()
        {
            if (IsServer)
                OnCompletedBuildableEventClientRpc();
            else
                base.OnCompletedBuildableEvent();
        }


        public override void Activate(PvPBuildableActivationArgs activationArgs)
        {
            OnActivatePvPClientRpc(activationArgs.ParentCruiser.Position, activationArgs.EnemyCruiser.Position, activationArgs.ParentCruiser.Direction, isAtCruiserHeight: false);
            base.Activate(activationArgs);
        }

        //-------------------------------------- RPCs -------------------------------------------------//
        [ClientRpc]
        private void OnActivatePvPClientRpc(Vector3 ParentCruiserPosition, Vector3 EnemyCruiserPosition, Direction facingDirection, bool isAtCruiserHeight)
        {
            if (!IsHost)
            {
                _aircraftProvider = new AircraftProvider(ParentCruiserPosition, EnemyCruiserPosition);
                FacingDirection = facingDirection;
                //    _isAtCruisingHeight = isAtCruiserHeight;
                Activate_PvPClient();
            }
        }

        [ClientRpc]
        private void OnBuildableProgressEventClientRpc()
        {
            if (!IsHost)
                OnBuildableProgressEvent();
        }


        [ClientRpc]
        private void OnCompletedBuildableEventClientRpc()
        {
            if (!IsHost)
                OnCompletedBuildableEvent();
        }

        [ClientRpc]
        private void UnfoldWingsClientRpc()
        {
            if (!IsHost)
            {
                leftWing.StartRotating();
                rightWing.StartRotating();
            }
        }
    }
}
