using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Utils;

namespace BattleCruisers.Effects.Laser
{
    public interface ILaserCooldownEffectInitialiser
    {
        IManagedDisposable CreateLaserCooldownEffect(IFireIntervalManager fireIntervalManager);
    }
}