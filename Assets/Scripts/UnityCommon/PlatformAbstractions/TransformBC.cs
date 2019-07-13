using UnityEngine;
using UnityEngine.Assertions;

namespace UnityCommon.PlatformAbstractions
{
    public class TransformBC : ITransform
    {
        public Transform PlatformObject { get; }

        public Vector3 Position
        {
            get { return PlatformObject.position; }
            set { PlatformObject.position = value; }
        }

        public Vector3 EulerAngles => PlatformObject.rotation.eulerAngles;
        public Vector3 Right => PlatformObject.right;
        public Vector3 Up => PlatformObject.up;
        public Quaternion Rotation => PlatformObject.rotation;

        public TransformBC(Transform platformTransform)
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