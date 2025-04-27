using System.Collections.Generic;
using BattleCruisers.Data.Static;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;

namespace BattleCruisers.Buildables.Units.Aircraft
{
    public class KamikazeController : MonoBehaviour
    {
        private IUnit _parentAircraft;
        private ITargetFilter _targetFilter;
        private IDamageApplier _damageApplier;
        private ITarget _initialTarget;

        // Have this to defer damaging the target until the next FixedUpdate(), because
        // there is a bug in Unity that if the target is destroyed from OnTriggerEnter2D()
        // the target collider does not trigger OnTriggerExit2D().  I filed a bug with
        // Unity so *hopefully* this is fixed one day and I can remove this deferral :)
        private ITarget _targetToDamage;

        private const float KAMIKAZE_DAMAGE_MULTIPLIER = 1;
        private IDamageStats kamikazeDamageStats;
        private float remainingPotentialDamage;

        public void Initialise(IUnit parentAircraft, ITarget target)
        {
            Helper.AssertIsNotNull(parentAircraft);

            _parentAircraft = parentAircraft;
            _initialTarget = target;
            _targetToDamage = null;

            remainingPotentialDamage = parentAircraft.MaxHealth * KAMIKAZE_DAMAGE_MULTIPLIER;

            List<TargetType> targetTypes = new List<TargetType>() { TargetType.Buildings, TargetType.Cruiser, TargetType.Ships };
            _targetFilter = new FactionAndTargetTypeFilter(_initialTarget.Faction, targetTypes);

            kamikazeDamageStats = new DamageStats(remainingPotentialDamage, parentAircraft.Size.x);

            _damageApplier = new AreaOfEffectDamageApplier(kamikazeDamageStats, new FactionTargetFilter(_initialTarget.Faction));

            _initialTarget.Destroyed += Target_Destroyed;
        }

        private void Target_Destroyed(object sender, DestroyedEventArgs e)
        {
            if (!_parentAircraft.IsDestroyed)
            {
                PrefabFactory.ShowExplosion(ExplosionType.FlakExplosion, transform.position);
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
                && _targetToDamage == null
                && target.Health > 0f)
            {
                _targetToDamage = target;
            }
        }

        private void FixedUpdate()
        {
            if (_targetToDamage != null
                && !_parentAircraft.IsDestroyed)
            {
                if (remainingPotentialDamage <= 0f)
                    CleanUp();

                float prevTargetHP = _targetToDamage.Health;
                Debug.Log(_targetToDamage.Health);

                kamikazeDamageStats = new DamageStats(
                        Mathf.Min(remainingPotentialDamage, _targetToDamage.Health),
                        _parentAircraft.Size.x);

                _damageApplier = new AreaOfEffectDamageApplier(kamikazeDamageStats, new FactionTargetFilter(_initialTarget.Faction));

                _damageApplier.ApplyDamage(_targetToDamage, _parentAircraft.Position, damageSource: _parentAircraft);
                PrefabFactory.ShowExplosion(ExplosionType.FlakExplosion, transform.position);

                float damageDealt = prevTargetHP - _targetToDamage.Health;
                float f = damageDealt / KAMIKAZE_DAMAGE_MULTIPLIER;
                _parentAircraft.TakeDamage(damageDealt / KAMIKAZE_DAMAGE_MULTIPLIER, _parentAircraft);


                if (_targetToDamage.Health == 0f)
                    _targetToDamage = null;

                if (damageDealt < remainingPotentialDamage)
                {
                    remainingPotentialDamage -= damageDealt;
                    kamikazeDamageStats = new DamageStats(remainingPotentialDamage, _parentAircraft.Size.x);
                    _damageApplier = new AreaOfEffectDamageApplier(kamikazeDamageStats, new FactionTargetFilter(_initialTarget.Faction));
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
