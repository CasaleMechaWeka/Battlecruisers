using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Effects.Smoke
{
    public class SmokeEmitter
    {
        private readonly IHealthStateMonitor _healthStateMonitor;
        private readonly ISmoke _smoke;

        public SmokeEmitter(IHealthStateMonitor healthStateMonitor, ISmoke smoke)
        {
            Helper.AssertIsNotNull(healthStateMonitor, smoke);

            _healthStateMonitor = healthStateMonitor;
            _smoke = smoke;

            _healthStateMonitor.HealthStateChanged += _healthStateMonitor_HealthStateChanged;
        }

        private void _healthStateMonitor_HealthStateChanged(object sender, EventArgs e)
        {
            _smoke.SmokeStrength = FindSmokeStrength(_healthStateMonitor.HealthState);
        }

        private SmokeStrength FindSmokeStrength(HealthState healthState)
        {
            switch (healthState)
            {
                case HealthState.FullHealth:
                    return SmokeStrength.None;

                case HealthState.SlightlyDamaged:
                    return SmokeStrength.Weak;

                case HealthState.Damaged:
                    return SmokeStrength.Normal;

                case HealthState.SeverelyDamaged:
                    return SmokeStrength.Strong;

                default:
                    throw new ArgumentException();
            }
        }
    }
}