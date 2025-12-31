using BattleCruisers.Buildables;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables
{
    public class PvPTargetProxy : MonoBehaviour, ITargetProxy
    {
        public ITarget Target { get; private set; }

        public void Initialise(ITarget target)
        {
            Assert.IsNotNull(target);
            Target = target;
        }
    }
}