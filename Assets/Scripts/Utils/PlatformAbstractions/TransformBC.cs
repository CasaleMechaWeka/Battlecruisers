using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    public class TransformBC : ITransform
    {
        public Transform PlatformObject { get; private set; }

        public Vector3 Position
        {
            get { return PlatformObject.position; }
            set { PlatformObject.position = value; }
        }

        public Vector3 EulerAngles { get { return PlatformObject.rotation.eulerAngles; } }
        public Vector3 Right { get { return PlatformObject.right; } }
        public Vector3 Up { get { return PlatformObject.up; } }
        public Quaternion Rotation { get { return PlatformObject.rotation; } }

        public TransformBC(Transform platformTransform)
        {
            Assert.IsNotNull(platformTransform);
            PlatformObject = platformTransform;
        }

        public void Rotate(Vector3 rotationChangeVector)
        {
            PlatformObject.Rotate(rotationChangeVector);
        }
    }
}