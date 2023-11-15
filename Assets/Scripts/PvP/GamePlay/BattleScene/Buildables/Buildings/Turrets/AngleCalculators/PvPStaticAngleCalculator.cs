using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators
{
    public class PvPStaticAngleCalculator : PvPAngleCalculator
    {
        private readonly float _desiredAngleInDegrees;

        public PvPStaticAngleCalculator(IPvPAngleHelper angleHelper, float desiredAngleInDegrees)
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
