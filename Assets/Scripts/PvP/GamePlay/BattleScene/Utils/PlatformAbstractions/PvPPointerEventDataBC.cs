using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions
{
    public class PvPPointerEventDataBC : IPvPPointerEventData
    {
        private readonly PointerEventData _platformObject;

        public PvPPointerEventDataBC(PointerEventData platformObject)
        {
            Assert.IsNotNull(platformObject);
            _platformObject = platformObject;
        }

        public Vector2 Delta => _platformObject.delta;
        public Vector2 Position => _platformObject.position;
    }
}