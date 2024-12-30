using BattleCruisers.Effects.Smoke;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Damage;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Smoke
{
    public class PvPSmokeEmitter
    {
        private readonly IPvPHealthStateMonitor _healthStateMonitor;
        private readonly ISmoke _smoke;
        private readonly bool _showSmokeWhenDestroyed;
        private PvPBuildable<PvPBuildableActivationArgs> _parentBuildable_buildable;
        private PvPBuildable<PvPBuildingActivationArgs> _parentBuildable_building;

        public PvPSmokeEmitter(PvPBuildable<PvPBuildableActivationArgs> parentBuildable, IPvPHealthStateMonitor healthStateMonitor, ISmoke smoke, bool showSmokeWhenDestroyed)
        {
            Helper.AssertIsNotNull(healthStateMonitor, smoke);

            _healthStateMonitor = healthStateMonitor;
            _smoke = smoke;
            _showSmokeWhenDestroyed = showSmokeWhenDestroyed;

            _healthStateMonitor.HealthStateChanged += _healthStateMonitor_HealthStateChanged;

            _parentBuildable_buildable = parentBuildable;
        }

        public PvPSmokeEmitter(PvPBuildable<PvPBuildingActivationArgs> parentBuildable, IPvPHealthStateMonitor healthStateMonitor, ISmoke smoke, bool showSmokeWhenDestroyed)
        {
            Helper.AssertIsNotNull(healthStateMonitor, smoke);

            _healthStateMonitor = healthStateMonitor;
            _smoke = smoke;
            _showSmokeWhenDestroyed = showSmokeWhenDestroyed;

            _healthStateMonitor.HealthStateChanged += _healthStateMonitor_HealthStateChanged;

            _parentBuildable_building = parentBuildable;
        }

        private void _healthStateMonitor_HealthStateChanged(object sender, EventArgs e)
        {
            if (_parentBuildable_buildable != null && _parentBuildable_buildable.BuildableState == PvPBuildableState.Completed)
                _smoke.SmokeStrength = FindSmokeStrength(_healthStateMonitor.HealthState);

            if (_parentBuildable_building != null && _parentBuildable_building.BuildableState == PvPBuildableState.Completed)
                _smoke.SmokeStrength = FindSmokeStrength(_healthStateMonitor.HealthState);
        }

        private SmokeStrength FindSmokeStrength(PvPHealthState healthState)
        {
            switch (healthState)
            {
                case PvPHealthState.FullHealth:
                    return SmokeStrength.None;

                case PvPHealthState.SlightlyDamaged:
                    return SmokeStrength.Weak;

                case PvPHealthState.Damaged:
                    return SmokeStrength.Normal;

                case PvPHealthState.SeverelyDamaged:
                    return SmokeStrength.Strong;

                case PvPHealthState.NoHealth:
                    // sava added
                    return _showSmokeWhenDestroyed ? SmokeStrength.Strong : SmokeStrength.None;
                //   return PvPSmokeStrength.None;

                default:
                    throw new ArgumentException();
            }
        }
    }
}