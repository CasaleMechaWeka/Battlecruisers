using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI
{
    public class LoadingCircle : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private const float ROTATE_SPEED_IN_DEGRESS_PER_S = 100;

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            Assert.IsNotNull(_rectTransform);
        }

        private void Update()
        {
            _rectTransform.Rotate(0, 0, ROTATE_SPEED_IN_DEGRESS_PER_S * Time.deltaTime);
        }
    }
}
