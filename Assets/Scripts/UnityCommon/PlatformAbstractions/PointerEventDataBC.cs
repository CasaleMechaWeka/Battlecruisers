using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace UnityCommon.PlatformAbstractions
{
    public class PointerEventDataBC : IPointerEventData
    {
        private readonly PointerEventData _platformObject;

        public PointerEventDataBC(PointerEventData platformObject)
        {
            Assert.IsNotNull(platformObject);
            _platformObject = platformObject;
        }

        public Vector2 Delta => _platformObject.delta;
        public Vector2 Position => _platformObject.position;
    }
}