using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators
{
    public class PvPStaticAngleCalculator : AngleCalculator
    {
        private readonly float _desiredAngleInDegrees;

        public PvPStaticAngleCalculator(IAngleHelper angleHelper, float desiredAngleInDegrees)
            : base(angleHelper)
        {
            _desiredAngleInDegrees = desiredAngleInDegrees;
        }

        public override float FindDesiredAngle(Vector2 sourcePosition, Vector2 targetPosition, bool isSourceMirrored)
        {
            return _desiredAngleInDegrees;
        }
    }
}
