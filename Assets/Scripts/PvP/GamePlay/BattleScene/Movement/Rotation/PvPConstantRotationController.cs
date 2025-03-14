using BattleCruisers.Movement.Rotation;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation
{
    public class PvPConstantRotationController : IConstantRotationController
    {
        private float _rotateSpeedInDegreesPerS;
        private Transform _transform;
        private ITime _time;

        public PvPConstantRotationController(float rotateSpeedInDegreesPerS, Transform transform)
        {
            _rotateSpeedInDegreesPerS = rotateSpeedInDegreesPerS;
            _transform = transform;
            _time = TimeBC.Instance;
        }

        public void Rotate()
        {
            float rotationIncrement = _time.DeltaTime * _rotateSpeedInDegreesPerS;
            Vector3 rotationIncrementVector = Vector3.forward * rotationIncrement;
            _transform.Rotate(rotationIncrementVector);
        }
    }
}