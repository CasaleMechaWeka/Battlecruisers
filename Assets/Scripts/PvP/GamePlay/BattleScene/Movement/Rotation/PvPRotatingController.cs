using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation
{
    public class PvPRotatingController : MonoBehaviour
    {
        private IPvPRotationMovementController _activeRotationController, _realRotationController, _dummyRotationController;
        private float _targetAngleInDegrees;
        private bool _haveReachedDesiredAngle;

        public event EventHandler ReachedDesiredAngle;

        public void Initialise(IPvPMovementControllerFactory movementControllerFactory, float rotateSpeedInMPerS, float targetAngleInDegrees)
        {
            _realRotationController = movementControllerFactory.CreateRotationMovementController(rotateSpeedInMPerS, transform, PvPTimeBC.Instance);
            _dummyRotationController = movementControllerFactory.CreateDummyRotationMovementController(isOnTarget: false);
            _activeRotationController = _dummyRotationController;

            _targetAngleInDegrees = targetAngleInDegrees;
            _haveReachedDesiredAngle = false;
        }

        public void StartRotating()
        {
            _activeRotationController = _realRotationController;
        }

        void FixedUpdate()
        {
            if (!_activeRotationController.IsOnTarget(_targetAngleInDegrees))
            {
                _activeRotationController.AdjustRotation(_targetAngleInDegrees);
            }
            else
            {
                if (!_haveReachedDesiredAngle)
                {
                    _haveReachedDesiredAngle = true;

                    ReachedDesiredAngle?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}
