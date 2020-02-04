using UnityCommon.Properties;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval
{
    public interface IFireIntervalManager
	{
        IBroadcastingProperty<bool> ShouldFire { get;  }

        void OnFired();
        void ProcessTimeInterval(float deltaTime);
	}
}
