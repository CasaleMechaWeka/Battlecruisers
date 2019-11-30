using UnityEngine;

namespace BattleCruisers.Effects.BarrelRecoil
{
    public class DummyAnimationInitialiser : MonoBehaviour, IAnimationInitialiser
    {
        public IAnimation CreateAnimation()
        {
            return new DummyAnimation();
        }
    }
}