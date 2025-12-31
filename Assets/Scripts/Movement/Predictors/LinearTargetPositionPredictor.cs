using BattleCruisers.Buildables;
using UnityEngine;

namespace BattleCruisers.Movement.Predictors
{
    public class LinearTargetPositionPredictor : TargetPositionPredictor
    {
        protected override float EstimateTimeToTarget(Vector2 sourcePosition, Vector2 targetPositionToAttack,
        float projectileVelocityInMPerS, float currentAngleInRadians, ITarget target)
        {
            Vector2? predictedTargetVector = GetIntersection(targetPositionToAttack, target.Velocity, sourcePosition, projectileVelocityInMPerS);

            if (predictedTargetVector == null)
                return Vector2.Distance(sourcePosition, targetPositionToAttack) / projectileVelocityInMPerS;
            else
                return Vector2.Distance(sourcePosition, (Vector2)predictedTargetVector) / projectileVelocityInMPerS;
        }

        Vector2? GetIntersection(Vector2 targetPos, Vector2 targetVelocity, Vector2 sourcePos, float projectileSpeed)
        {
            Vector2 r = targetPos - sourcePos;
            float denominator = Vector2.Dot(targetVelocity, targetVelocity) - (projectileSpeed * projectileSpeed);

            if (denominator == 0)
                return null;

            float a = denominator;
            float b = 2 * Vector2.Dot(targetVelocity, r);
            float c = Vector2.Dot(r, r);


            float t1 = (-b + Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);
            float t2 = (-b - Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);

            float t = 0f;

            if (t1 > 0 && t2 > 0)
                t = Mathf.Min(t1, t2);
            else if (t1 > 0)
                t = t1;
            else if (t2 > 0)
                t = t2;
            else
                return null;

            return targetPos + t * targetVelocity;
        }
    }
}

