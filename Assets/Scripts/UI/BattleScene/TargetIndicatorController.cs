using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene
{
    public class TargetIndicatorController : MonoBehaviour, ITargetIndicator
    {
        private Animator _animation;

        public void Initialise()
        {
            _animation = GetComponent<Animator>();
            Assert.IsNotNull(_animation);
        }

        public void Show(Vector2 position)
        {
            Logging.LogMethod(Tags.USER_CHOSEN_TARGET);

            transform.position = position;
            gameObject.SetActive(true);
            _animation.Play("TargetIndicator", layer: -1, normalizedTime: 0);
        }

        public void Hide()
        {
            Logging.LogMethod(Tags.USER_CHOSEN_TARGET);
            gameObject.SetActive(false);
        }
    }
}