using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators
{
    public class PvPAngleCalculator : IPvPAngleCalculator
    {
        private readonly IAngleHelper _angleHelper;

        public PvPAngleCalculator(IAngleHelper angleHelper)
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
