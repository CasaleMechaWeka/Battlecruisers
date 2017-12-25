using System.Collections.Generic;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Explosions;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class KamikazeController : MonoBehaviour
    {
        private IUnit _parentAircraft;
        private ITargetFilter _targetFilter;
        private IDamageApplier _damageApplier;
        private IExplosion _explosion;

        private const float KAMIKAZE_DAMAGE_MULTIPLIER = 1.5f;

        public void Initialise(IUnit parentAircraft, IFactoryProvider factoryProvider, ITarget target)
        {
            Helper.AssertIsNotNull(parentAircraft, factoryProvider);

            _parentAircraft = parentAircraft;

            List<TargetType> targetTypes = new List<TargetType>() { TargetType.Buildings, TargetType.Cruiser, TargetType.Ships };
            _targetFilter = factoryProvider.TargetsFactory.CreateTargetFilter(target.Faction, targetTypes);

            IDamageStats kamikazeDamageStats
                = factoryProvider.DamageApplierFactory.CreateDamageStats(
                    damage: parentAircraft.MaxHealth * KAMIKAZE_DAMAGE_MULTIPLIER,
                    damageRadiusInM: parentAircraft.Size.x);
            _damageApplier = factoryProvider.DamageApplierFactory.CreateFactionSpecificAreaOfDamageApplier(kamikazeDamageStats, target.Faction);

            _explosion = factoryProvider.ExplosionFactory.CreateExplosion(kamikazeDamageStats.DamageRadiusInM);
        }

		void OnTriggerEnter2D(Collider2D collider)
		{
			ITarget target = collider.gameObject.GetComponent<ITarget>();

			if (target != null 
                && _targetFilter.IsMatch(target)
                && !target.IsDestroyed
                && !_parentAircraft.IsDestroyed)
			{
                _damageApplier.ApplyDamage(target, _parentAircraft.Position, damageSource: _parentAircraft);
                _explosion.Show(_parentAircraft.Position);
                _parentAircraft.Destroy();
			}
		}
    }
}
