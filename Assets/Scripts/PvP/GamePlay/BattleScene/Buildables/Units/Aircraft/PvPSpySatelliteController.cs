using BattleCruisers.Movement.Velocity;
using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using Unity.Netcode;
using UnityEngine;
using BattleCruisers.Buildables.Units.Aircraft;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public class PvPSpySatelliteController : PvPSatelliteController
    {
        public override TargetType TargetType => TargetType.Satellite;

        protected override IList<IPatrolPoint> GetPatrolPoints()
        {
            IList<Vector2> patrolPositions = _aircraftProvider.SpySatellitePatrolPoints(transform.position, cruisingAltitudeInM);

            IList<IPatrolPoint> patrolPoints = new List<IPatrolPoint>(patrolPositions.Count)
            {
                new PatrolPoint(patrolPositions[0], removeOnceReached: true)
            };

            for (int i = 1; i < patrolPositions.Count; ++i)
            {
                patrolPoints.Add(new PatrolPoint(patrolPositions[i]));
            }

            return patrolPoints;
        }

        //------------------------------------ methods for sync, written by Sava ------------------------------//
        public override void Activate(PvPBuildableActivationArgs activationArgs)
        {
            OnActivatePvPClientRpc(activationArgs.ParentCruiser.Position, activationArgs.EnemyCruiser.Position, activationArgs.ParentCruiser.Direction);
            base.Activate(activationArgs);
            OnActivatePvPClientRpc();
        }

        



        public NetworkVariable<float> PvP_BuildProgress = new NetworkVariable<float>();
        private void LateUpdate()
        {
            if (IsServer)
            {
                if (PvP_BuildProgress.Value != BuildProgress)
                    PvP_BuildProgress.Value = BuildProgress;
            }
            else
            {
                BuildProgress = PvP_BuildProgress.Value;
            }
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
        protected override void OnDestroyedEvent()
        {
            if (IsServer)
                OnDestroyedEventClientRpc();
            else
                base.OnDestroyedEvent();
        }

        //-------------------------------------- RPCs -------------------------------------------------//

        [ClientRpc]
        private void OnActivatePvPClientRpc(Vector3 ParentCruiserPosition, Vector3 EnemyCruiserPosition, Direction facingDirection)
        {
            if (!IsHost)
            {
                _aircraftProvider = new AircraftProvider(ParentCruiserPosition, EnemyCruiserPosition);
                FacingDirection = facingDirection;
                Activate_PvPClient();
            }
        }

        [ClientRpc]
        private void OnProgressControllerVisibleClientRpc(bool isEnabled)
        {
            _buildableProgress.gameObject.SetActive(isEnabled);
            if (!isEnabled)
            {
                Invoke("ActiveTrail", 0.5f);
            }
        }

        [ClientRpc]
        private void OnActivatePvPClientRpc()
        {
            if (!IsHost)
                Activate_PvPClient();
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
        private void OnDestroyedEventClientRpc()
        {
            if (!IsHost)
            {
                OnDestroyedEvent();
            }
        }



    }
}