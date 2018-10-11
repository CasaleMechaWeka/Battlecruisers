using BattleCruisers.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Effects.Explosions
{
    /// <summary>
    /// Explosion scaled by default to have a radius of 1 meter.  Adjust transform
    /// scale linearly to achieve desired radius.
    /// </summary>
    public abstract class Explosion : ScalableCircle, IExplosion
    {
        protected float _durationInS;

        public virtual void Initialise(float radiusInM, float durationInS)
        {
            base.Initialise(radiusInM);

			Assert.IsTrue(durationInS > 0);
            _durationInS = durationInS;

            gameObject.SetActive(false);
        }

        public void Show(Vector3 position)
        {
            transform.position = position;
            gameObject.SetActive(true);

            OnShow();
        }

        protected virtual void OnShow() { }
    }
}
