using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Explosions
{
    /// <summary>
    /// Explosion scaled by default to have a radius of 1 meter.  Adjust transform
    /// scale linearly to achieve desired radius.
    /// </summary>
    public class Explosion : MonoBehaviour
    {
        private float _radiusToScaleRatio;

        public void StaticInitailise()
        {
            RectTransform rectTransfrom = transform.Parse<RectTransform>();

            Assert.AreEqual(rectTransfrom.sizeDelta.x, rectTransfrom.sizeDelta.y);
            Assert.AreEqual(transform.localScale.x, transform.localScale.y);

            _radiusToScaleRatio = rectTransfrom.sizeDelta.x / transform.localScale.x;

            gameObject.SetActive(false);
        }

        public void Show(float radiusInM, float durationInS)
        {
            float newScale = radiusInM / _radiusToScaleRatio;
            transform.localScale = new Vector3(newScale, newScale, 1);

            gameObject.SetActive(true);

            Destroy(gameObject, durationInS);
        }
    }
}
