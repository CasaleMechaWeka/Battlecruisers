using BattleCruisers.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation
{
    public class PvPRotationMovementController : IPvPRotationMovementController
    {
        private readonly IPvPRotationHelper _rotationHelper;
        private readonly IPvPTransform _transform;
        private readonly IPvPDeltaTimeProvider _time;
        private readonly float _rotateSpeedInDegreesPerS;

        public const float ROTATION_EQUALITY_MARGIN_IN_DEGREES = 1;

        public PvPRotationMovementController(
            IPvPRotationHelper rotationHelper,
            IPvPTransform transform,
            IPvPDeltaTimeProvider time,
            float rotateSpeedInDegreesPerS)
        {
            Helper.AssertIsNotNull(rotationHelper, transform, time);
            Assert.IsTrue(rotateSpeedInDegreesPerS > 0);

            _rotationHelper = rotationHelper;
            _transform = transform;
            _time = time;
            _rotateSpeedInDegreesPerS = rotateSpeedInDegreesPerS;
        }

        public bool IsOnTarget(float desiredAngleInDegrees)
        {
            float currentAngleInDegrees = _transform.EulerAngles.z;
            float differenceInDegrees = Mathf.Abs(currentAngleInDegrees - desiredAngleInDegrees);
            bool isOnTarget = differenceInDegrees < ROTATION_EQUALITY_MARGIN_IN_DEGREES;

            Logging.Verbose(Tags.ROTATION_MOVEMENT_CONTROLLER, $"isOnTarget: {isOnTarget}  currentAngle: {currentAngleInDegrees}  desiredAngle: {desiredAngleInDegrees}");
            return isOnTarget;
        }

        public void AdjustRotation(float desiredAngleInDegrees)
        {
            float currentAngleInDegrees = _transform.EulerAngles.z;
            float differenceInDegrees = Mathf.Abs(currentAngleInDegrees - desiredAngleInDegrees);
            float directionMultiplier = _rotationHelper.FindDirectionMultiplier(currentAngleInDegrees, desiredAngleInDegrees);

            float rotationIncrement = _time.DeltaTime * _rotateSpeedInDegreesPerS;
            if (rotationIncrement > differenceInDegrees)
            {
                rotationIncrement = differenceInDegrees;
            }
            Vector3 rotationIncrementVector = Vector3.forward * rotationIncrement * directionMultiplier;

            Logging.Verbose(Tags.ROTATION_MOVEMENT_CONTROLLER, $"Rotation pre rotate: {_transform.EulerAngles}");
            _transform.Rotate(rotationIncrementVector);
            Logging.Verbose(Tags.ROTATION_MOVEMENT_CONTROLLER, $"Rotated transform by: {rotationIncrement}  new rotation; {_transform.EulerAngles}");
        }
    }
}