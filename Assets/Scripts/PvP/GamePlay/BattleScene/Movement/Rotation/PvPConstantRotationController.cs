using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation
{
    public class PvPConstantRotationController : IPvPConstantRotationController
    {
        private float _rotateSpeedInDegreesPerS;
        private Transform _transform;
        private IPvPTime _time;

        public PvPConstantRotationController(float rotateSpeedInDegreesPerS, Transform transform)
        {
            _rotateSpeedInDegreesPerS = rotateSpeedInDegreesPerS;
            _transform = transform;
            _time = PvPTimeBC.Instance;
        }

        public void Rotate()
        {
            float rotationIncrement = _time.DeltaTime * _rotateSpeedInDegreesPerS;
            Vector3 rotationIncrementVector = Vector3.forward * rotationIncrement;
            _transform.Rotate(rotationIncrementVector);
        }
    }
}