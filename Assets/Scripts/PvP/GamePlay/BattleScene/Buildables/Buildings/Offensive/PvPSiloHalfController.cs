using BattleCruisers.Movement.Rotation;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Offensive
{
    public class PvPSiloHalfController : RotatingController
    {
        public SpriteRenderer Renderer { get; private set; }

        public void StaticInitialise()
        {
            Renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
            Assert.IsNotNull(Renderer);
        }
    }
}