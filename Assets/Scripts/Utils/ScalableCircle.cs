using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils
{
    /// <summary>
    /// Game object scaled by default to have a radius of 1 meter.  Adjust transform
    /// scale linearly to achieve desired radius.
    /// </summary>
    public abstract class ScalableCircle : MonoBehaviour
    {
        public virtual void Initialise(float radiusInM)
        {
            Assert.IsTrue(radiusInM > 0);
            ScaleCircleSize(radiusInM);
        }

        private void ScaleCircleSize(float radiusInM)
        {
            float radiusToScaleRatio = FindRadiusToScaleRatio();
            float newScale = radiusInM / radiusToScaleRatio;
            transform.localScale = new Vector3(newScale, newScale, 1);
        }

        private float FindRadiusToScaleRatio()
        {
            RectTransform rectTransfrom = transform.Parse<RectTransform>();

            Assert.AreEqual(rectTransfrom.sizeDelta.x, rectTransfrom.sizeDelta.y);
            Assert.AreEqual(transform.localScale.x, transform.localScale.y);

            return rectTransfrom.sizeDelta.x / transform.localScale.x;
        }
    }
}
