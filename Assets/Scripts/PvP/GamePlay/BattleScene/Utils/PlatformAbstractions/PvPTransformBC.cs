using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions
{
    public class PvPTransformBC : IPvPTransform
    {
        public Transform PlatformObject { get; }

        public Vector3 Position
        {
            get { return PlatformObject.position; }
            set { PlatformObject.position = value; }
        }

        public Vector3 EulerAngles => PlatformObject.eulerAngles;
        public Vector3 Right => PlatformObject.right;
        public Vector3 Up => PlatformObject.up;
        public Quaternion Rotation => PlatformObject.rotation;
        public bool IsMirroredAcrossYAxis => PlatformObject.eulerAngles.y == 180;

        public PvPTransformBC(Transform platformTransform)
        {
            Assert.IsNotNull(platformTransform);
            PlatformObject = platformTransform;
        }

        public void Rotate(Vector3 rotationChangeVector)
        {
            PlatformObject.Rotate(rotationChangeVector);
        }

        public void SetParent(Transform parent, bool worldPositionStays)
        {
            PlatformObject.SetParent(parent, worldPositionStays);
        }
    }
}


