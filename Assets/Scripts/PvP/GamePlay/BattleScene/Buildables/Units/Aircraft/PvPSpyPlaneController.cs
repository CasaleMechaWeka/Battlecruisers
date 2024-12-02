using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Velocity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using Unity.Netcode;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.Test.Utilities
{
    public class PvPSpyPlaneController : PvPAircraftController, IPvPSpyPlaneController
    {
        // IList is not picked up by the Unity inspector
        public List<Vector2> patrolPoints;
        public IList<Vector2> PatrolPoints
        {
            get { return patrolPoints; }
            set { patrolPoints = new List<Vector2>(value); }
        }

        private bool _useDummyMovementController = false;
        public bool UseDummyMovementController
        {
            private get { return _useDummyMovementController; }
            set
            {
                _useDummyMovementController = value;

                if (_useDummyMovementController)
                {
                    // Create bogus patrol points so PatrollingMovementController is 
                    // created correctly in AircraftController base class
                    PatrolPoints = new List<Vector2> { new Vector2(0, 0), new Vector2(1, 1) };
                }
            }
        }

        protected override async void OnBuildableCompleted()
        {
            if (IsServer)
            {
                base.OnBuildableCompleted();

                PvPFaction enemyFaction = PvPHelper.GetOppositeFaction(Faction);
                IPvPTarget parent = this;
                IPvPUpdater updater = _factoryProvider.UpdaterProvider.PerFrameUpdater;

                _spriteChooser = await _factoryProvider.SpriteChooserFactory.CreateSpyPlaneSpriteChooserAsync(this);
                OnBuildableCompletedClientRpc();

                if (UseDummyMovementController)
                {
                    ActiveMovementController = DummyMovementController;
                }
            }
            else
            {
                OnBuildableCompleted_PvPClient();
                _spriteChooser = await _factoryProvider.SpriteChooserFactory.CreateSpyPlaneSpriteChooserAsync(this);
            }
        }

        protected override IList<IPvPPatrolPoint> GetPatrolPoints()
        {
            return BCUtils.PvPHelper.ConvertVectorsToPatrolPoints(patrolPoints);
        }

        // Add RPCs for client synchronization
        [ClientRpc]
        private void OnBuildableCompletedClientRpc()
        {
            if (!IsHost)
                OnBuildableCompleted_PvPClient();
        }

        [ClientRpc]
        private void OnSetPositionClientRpc(Vector3 pos)
        {
            if (!IsHost)
                Position = pos;
        }

        [ClientRpc]
        private void OnSetRotationClientRpc(Quaternion rotation)
        {
            if (!IsHost)
                Rotation = rotation;
        }

        [ClientRpc]
        private void OnProgressControllerVisibleClientRpc(bool isEnabled)
        {
            _buildableProgress.gameObject.SetActive(isEnabled);
            if (!IsHost && !isEnabled)
            {
                Invoke("ActiveTrail", 0.5f);
            }
        }

        private void ActiveTrail()
        {
            _aircraftTrailObj.SetActive(true);
        }

    }
}
