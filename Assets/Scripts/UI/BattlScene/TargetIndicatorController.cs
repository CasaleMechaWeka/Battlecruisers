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
            transform.position = position;
            gameObject.SetActive(true);
            _animation.Play("TargetIndicator", layer: -1, normalizedTime: 0);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}