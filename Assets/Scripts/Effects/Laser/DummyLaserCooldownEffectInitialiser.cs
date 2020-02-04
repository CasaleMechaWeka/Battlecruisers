using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Effects.Laser
{
    public class DummyLaserCooldownEffectInitialiser : MonoBehaviour, ILaserCooldownEffectInitialiser
    {
        public IManagedDisposable CreateLaserCooldownEffect(IFireIntervalManager fireIntervalManager)
        {
            return new DummyManagedDisposable();
        }
    }
}
