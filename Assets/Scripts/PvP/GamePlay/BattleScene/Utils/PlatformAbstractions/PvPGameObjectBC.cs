using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions
{
    public class PvPGameObjectBC : IPvPGameObject
    {
        public GameObject _platformObject;

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

        public PvPGameObjectBC(GameObject platformObject)
        {
            Assert.IsNotNull(platformObject);
            _platformObject = platformObject;
        }
    }
}