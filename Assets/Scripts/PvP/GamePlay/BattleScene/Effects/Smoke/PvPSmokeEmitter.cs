using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Damage;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Smoke
{
    public class PvPSmokeEmitter
    {
        private readonly IPvPHealthStateMonitor _healthStateMonitor;
        private readonly IPvPSmoke _smoke;
        private readonly bool _showSmokeWhenDestroyed;

        public PvPSmokeEmitter(IPvPHealthStateMonitor healthStateMonitor, IPvPSmoke smoke, bool showSmokeWhenDestroyed)
        {
            Helper.AssertIsNotNull(healthStateMonitor, smoke);

            _healthStateMonitor = healthStateMonitor;
            _smoke = smoke;
            _showSmokeWhenDestroyed = showSmokeWhenDestroyed;

            _healthStateMonitor.HealthStateChanged += _healthStateMonitor_HealthStateChanged;
        }

        private void _healthStateMonitor_HealthStateChanged(object sender, EventArgs e)
        {
            _smoke.SmokeStrength = FindSmokeStrength(_healthStateMonitor.HealthState);
        }

        private PvPSmokeStrength FindSmokeStrength(PvPHealthState healthState)
        {
            switch (healthState)
            {
                case PvPHealthState.FullHealth:
                    return PvPSmokeStrength.None;

                case PvPHealthState.SlightlyDamaged:
                    return PvPSmokeStrength.Weak;

                case PvPHealthState.Damaged:
                    return PvPSmokeStrength.Normal;

                case PvPHealthState.SeverelyDamaged:
                    return PvPSmokeStrength.Strong;

                case PvPHealthState.NoHealth:
                    return _showSmokeWhenDestroyed ? PvPSmokeStrength.Strong : PvPSmokeStrength.None;

                default:
                    throw new ArgumentException();
            }
        }
    }
}