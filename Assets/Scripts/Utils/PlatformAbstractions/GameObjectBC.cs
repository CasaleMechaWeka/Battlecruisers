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

        public Vector2 Position => _platformObject.transform.position;

        public GameObjectBC(GameObject platformObject)
        {
            Assert.IsNotNull(platformObject);
            _platformObject = platformObject;
        }
    }
}