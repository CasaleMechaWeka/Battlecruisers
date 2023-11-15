using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables
{
    public class PvPTargetProxy : MonoBehaviour, IPvPTargetProxy
    {
        public IPvPTarget Target { get; private set; }

        public void Initialise(IPvPTarget target)
        {
            Assert.IsNotNull(target);
            Target = target;
        }
    }
}