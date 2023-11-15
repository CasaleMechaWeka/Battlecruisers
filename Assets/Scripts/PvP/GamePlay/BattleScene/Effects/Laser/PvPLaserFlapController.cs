using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Laser
{
    public class PvPLaserFlapController : MonoBehaviour, IPvPLaserFlap
    {
        private Animator _flapAnimator;

        private const string CLOSED_STATE = "ClosedFlap";
        private const string OPENED_STATE = "OpenedFlap";

        public void Initialise()
        {
            _flapAnimator = GetComponent<Animator>();
            Assert.IsNotNull(_flapAnimator);
        }

        public void OpenFlap()
        {
            _flapAnimator.Play(OPENED_STATE, layer: -1, normalizedTime: 0);
        }

        public void CloseFlap()
        {
            _flapAnimator.Play(CLOSED_STATE, layer: -1, normalizedTime: 0);
        }
    }
}