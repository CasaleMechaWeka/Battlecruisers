using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.BarrelRecoil
{
    public class PvPDummyAnimationInitialiser : MonoBehaviour, IPvPAnimationInitialiser
    {
        public IPvPAnimation CreateAnimation()
        {
            return new PvPDummyAnimation();
        }
    }
}