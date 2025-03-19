using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public class AngleCalculator : IAngleCalculator
    {
        private readonly AngleHelper _angleHelper;

        public AngleCalculator(AngleHelper angleHelper)
        {
            Assert.IsNotNull(angleHelper);
            _angleHelper = angleHelper;
        }

        public virtual float FindDesiredAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored)
        {
            return _angleHelper.FindAngle(sourcePosition, targetPosition, isSourceMirrored);
        }
    }
}
