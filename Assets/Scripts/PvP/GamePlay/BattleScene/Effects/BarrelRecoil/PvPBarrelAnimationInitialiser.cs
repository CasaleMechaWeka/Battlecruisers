using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.BarrelRecoil
{
    public class PvPBarrelAnimationInitialiser : MonoBehaviour, IPvPAnimationInitialiser
    {
        public IPvPAnimation CreateAnimation()
        {
            Animator animator = GetComponent<Animator>();
            Assert.IsNotNull(animator);
            animator.enabled = false;
            return new PvPBarrelAnimation(animator);
        }
    }
}