using System.Collections.Generic;
using BattleCruisers.Effects.Explosions.Pools;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class KamikazeController : MonoBehaviour
    {
        private IUnit _parentAircraft;
        private ITargetFilter _targetFilter;
        private IDamageApplier _damageApplier;
        private IExplosionPoolProvider _explosionPoolProvider;
        private ITarget _initialTarget;

        // Have this to defer damaging the target until the next FixedUpdate(), because
        // there is a bug in Unity that if the target is destroyed from OnTriggerEnter2D()
        // the target collider does not trigger OnTriggerExit2D().  I filed a bug with
        // Unity so *hopefully* this is fixed one day and I can remove this deferral :)
        private ITarget _targetToDamage;

        private const float KAMIKAZE_DAMAGE_MULTIPLIER = 4;
        private IDamageStats kamikazeDamageStats;
        private IFactoryProvider _factoryProvider;
        private float ramainingPotentialDamage;

        public void Initialise(IUnit parentAircraft, IFactoryProvider factoryProvider, ITarget target)
        {
            Helper.AssertIsNotNull(parentAircraft, factoryProvider);

            _parentAircraft = parentAircraft;
            _factoryProvider = factoryProvider;
            _initialTarget = target;
            _targetToDamage = null;

            ramainingPotentialDamage = parentAircraft.MaxHealth * KAMIKAZE_DAMAGE_MULTIPLIER;

            List<TargetType> targetTypes = new List<TargetType>() { TargetType.Buildings, TargetType.Cruiser, TargetType.Ships };
            _targetFilter = factoryProvider.Targets.FilterFactory.CreateTargetFilter(_initialTarget.Faction, targetTypes);

            kamikazeDamageStats
                = factoryProvider.DamageApplierFactory.CreateDamageStats(
                    damage: ramainingPotentialDamage,
                    damageRadiusInM: parentAircraft.Size.x);
            _damageApplier = _factoryProvider.DamageApplierFactory.CreateFactionSpecificAreaOfDamageApplier(kamikazeDamageStats, _initialTarget.Faction);

            _explosionPoolProvider = _factoryProvider.PoolProviders.ExplosionPoolProvider;

            _initialTarget.Destroyed += Target_Destroyed;
        }

        private void Target_Destroyed(object sender, DestroyedEventArgs e)
        {
            if (!_parentAircraft.IsDestroyed)
            {
                _explosionPoolProvider.FlakExplosionsPool.GetItem(transform.position);
                CleanUp();
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            ITarget target = collider.gameObject.GetComponent<ITargetProxy>()?.Target;

            if (target != null
                && !target.IsDestroyed
                && _targetFilter.IsMatch(target)
                && !_parentAircraft.IsDestroyed
                && _targetToDamage == null)
            {
                if (target.Health != 0f)
                    _targetToDamage = target;
            }
        }

        private void FixedUpdate()
        {
            if (_targetToDamage != null
                && !_parentAircraft.IsDestroyed)
            {
                float prevTargetHP = _targetToDamage.Health;

                kamikazeDamageStats
                     = _factoryProvider.DamageApplierFactory.CreateDamageStats(
                        damage: Mathf.Min(ramainingPotentialDamage, _targetToDamage.Health),
                        damageRadiusInM: _parentAircraft.Size.x);
                _damageApplier = _factoryProvider.DamageApplierFactory.CreateFactionSpecificAreaOfDamageApplier(kamikazeDamageStats, _initialTarget.Faction);

                _damageApplier.ApplyDamage(_targetToDamage, _parentAircraft.Position, damageSource: _parentAircraft);
                _explosionPoolProvider.FlakExplosionsPool.GetItem(transform.position);

                float damageDealt = prevTargetHP - _targetToDamage.Health;
                ramainingPotentialDamage -= damageDealt;
                if (_targetToDamage.Health == 0f)
                    _targetToDamage = null;

                if (damageDealt < kamikazeDamageStats.Damage)
                {
                    kamikazeDamageStats
                        = _factoryProvider.DamageApplierFactory.CreateDamageStats(
                            damage: ramainingPotentialDamage,
                            damageRadiusInM: _parentAircraft.Size.x);
                    _damageApplier = _factoryProvider.DamageApplierFactory.CreateFactionSpecificAreaOfDamageApplier(kamikazeDamageStats, _initialTarget.Faction);
                }
                else
                {
                    CleanUp();
                }
            }
        }

        public void CleanUp()
        {
            _initialTarget.Destroyed -= Target_Destroyed;
            _targetToDamage = null;
            _parentAircraft.Destroy();
        }
    }
}
