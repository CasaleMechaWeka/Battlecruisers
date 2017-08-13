using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators
{
    public interface IAngleCalculator
	{
		// Cannot use both namespaces and optional parameters in MonoBehaviour scripts :D
		// Otherwise I would use optional parameters for the last two parameters.
		// https://forum.unity3d.com/threads/script-can-use-namespace-or-optional-parameters-but-not-both.164563/
		float FindDesiredAngle(Vector2 source, ITarget target, bool isSourceMirrored, float projectileVelocityInMPerS, float currentAngleInRadians);
		float FindDirectionMultiplier(float currentAngleInRadians, float desiredAngleInDegrees);
	}
}
