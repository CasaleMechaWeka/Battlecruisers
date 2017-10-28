using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Explosions
{
    /// <summary>
    /// Explosion scaled by default to have a radius of 1 meter.  Adjust transform
    /// scale linearly to achieve desired radius.
    /// </summary>
    public class Explosion : MonoBehaviour, IExplosion
    {
        private float _durationInS;

        public void Initialise(float radiusInM, float durationInS)
        {
            Assert.IsTrue(radiusInM > 0);
            Assert.IsTrue(durationInS > 0);

            _durationInS = durationInS;

            float radiusToScaleRatio = FindRadiusToScaleRatio();
            float newScale = radiusInM / radiusToScaleRatio;
            transform.localScale = new Vector3(newScale, newScale, 1);

            gameObject.SetActive(false);
        }

        private float FindRadiusToScaleRatio()
        {
            RectTransform rectTransfrom = transform.Parse<RectTransform>();

            Assert.AreEqual(rectTransfrom.sizeDelta.x, rectTransfrom.sizeDelta.y);
            Assert.AreEqual(transform.localScale.x, transform.localScale.y);

            return rectTransfrom.sizeDelta.x / transform.localScale.x;
        }

        public void Show(Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);

            Destroy(gameObject, _durationInS);
        }
    }
}
