using UnityEngine;
using UnityEngine.Assertions;

namespace UnityCommon.PlatformAbstractions
{
    public class GameObjectBC : IGameObject
    {
        public GameObject PlatformObject { get; }
        public ITransform Transform { get; }

        public GameObjectBC(GameObject platformObject)
        {
            Assert.IsNotNull(platformObject);

            PlatformObject = platformObject;
            Transform = new TransformBC(platformObject.transform);
        }

        public void Destroy()
        {
            Object.Destroy(PlatformObject);
        }
    }
}