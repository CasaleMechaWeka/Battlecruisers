using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.PlatformAbstractions
{
    public class GameObjectBC : IGameObject
    {
        private readonly GameObject _platformObject;

        public bool IsVisible
        {
            get { return _platformObject.activeSelf; }
            set { _platformObject.SetActive(value); }
        }

        public Vector3 Position
        {
            get { return _platformObject.transform.position; }
            set { _platformObject.transform.position = value; }
        }

        public GameObjectBC(GameObject platformObject)
        {
            Assert.IsNotNull(platformObject);
            _platformObject = platformObject;
        }
    }
}