using BattleCruisers.Effects.Explosions;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Factories;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class KamikazeController : MonoBehaviour, IRemovable
    {
        private IUnit _parentAircraft;
        private IRemovable _parentAsRemovable;
        private ITargetFilter _targetFilter;
        private IDamageApplier _damageApplier;
        private IExplosionManager _explosionManager;

        // Have this to defer damaging the target until the next FixedUpdate(), because
        // there is a bug in Unity that if the target is destroyed from OnTriggerEnter2D()
        // the target collider does not trigger OnTriggerExit2D().  I filed a bug with
        // Unity so *hopefully* this is fixed one day and I can remove this deferral :)
        private ITarget _targetToDamage;

        private const float KAMIKAZE_DAMAGE_MULTIPLIER = 1.5f;

        public void Initialise(IUnit parentAircraft, IFactoryProvider factoryProvider, ITarget target)
        {
            Helper.AssertIsNotNull(parentAircraft, factoryProvider);

            _parentAircraft = parentAircraft;
            _parentAsRemovable = parentAircraft;
            _targetToDamage = null;

            List<TargetType> targetTypes = new List<TargetType>() { TargetType.Buildings, TargetType.Cruiser, TargetType.Ships };
            _targetFilter = factoryProvider.TargetFactories.FilterFactory.CreateTargetFilter(target.Faction, targetTypes);

            IDamageStats kamikazeDamageStats
                = factoryProvider.DamageApplierFactory.CreateDamageStats(
                    damage: parentAircraft.MaxHealth * KAMIKAZE_DAMAGE_MULTIPLIER,
                    damageRadiusInM: parentAircraft.Size.x);
            _damageApplier = factoryProvider.DamageApplierFactory.CreateFactionSpecificAreaOfDamageApplier(kamikazeDamageStats, target.Faction);

            _explosionManager = factoryProvider.ExplosionManager;
        }

		private void OnTriggerEnter2D(Collider2D collider)
		{
			ITarget target = collider.gameObject.GetComponent<ITarget>();

			if (target != null 
                && !target.IsDestroyed
                && _targetFilter.IsMatch(target)
                && !_parentAircraft.IsDestroyed
                && _targetToDamage == null)
			{
                _targetToDamage = target;
			}
		}

        private void FixedUpdate()
        {
            if (_targetToDamage != null
                && !_parentAircraft.IsDestroyed)
            {
                RemoveFromScene();
                _damageApplier.ApplyDamage(_targetToDamage, _parentAircraft.Position, damageSource: _parentAircraft);
                IExplosionStats explosionStats = new ExplosionStats(ExplosionSize.Small, showTrails: true);
                _explosionManager.ShowExplosion(explosionStats, transform.position);
            }
        }

        public void RemoveFromScene()
        {
            _parentAsRemovable.RemoveFromScene();
        }
    }
}
