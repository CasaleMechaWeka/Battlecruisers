using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    public class TransformBC : ITransform
    {
        private readonly Transform _platformTransform;

        public Vector3 Position
        {
            get { return _platformTransform.position; }
            set { _platformTransform.position = value; }
        }

        public Vector3 EulerAngles { get { return _platformTransform.rotation.eulerAngles; } }
        public Vector3 Right { get { return _platformTransform.right; } }
        public Vector3 Up { get { return _platformTransform.up; } }

        public TransformBC(Transform platformTransform)
        {
            Assert.IsNotNull(platformTransform);
            _platformTransform = platformTransform;
        }

        public void Rotate(Vector3 rotationChangeVector)
        {
            _platformTransform.Rotate(rotationChangeVector);
        }
    }
}