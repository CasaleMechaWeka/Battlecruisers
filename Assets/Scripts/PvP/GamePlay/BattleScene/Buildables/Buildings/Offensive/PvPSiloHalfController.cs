using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Offensive
{
    public class PvPSiloHalfController : PvPRotatingController
    {
        public SpriteRenderer Renderer { get; private set; }

        public void StaticInitialise()
        {
            Renderer = gameObject.GetComponentInChildren<SpriteRenderer>();
            Assert.IsNotNull(Renderer);
        }
    }
}