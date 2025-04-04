using BattleCruisers.Buildables;
using BattleCruisers.Effects.Explosions.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public class PvPKamikazeController : MonoBehaviour
    {
        private IPvPUnit _parentAircraft;
        private ITargetFilter _targetFilter;
        private IDamageApplier _damageApplier;
        private IExplosionPoolProvider _explosionPoolProvider;
        private ITarget _initialTarget;

        // Have this to defer damaging the target until the next FixedUpdate(), because
        // there is a bug in Unity that if the target is destroyed from OnTriggerEnter2D()
        // the target collider does not trigger OnTriggerExit2D().  I filed a bug with
        // Unity so *hopefully* this is fixed one day and I can remove this deferral :)
        private ITarget _targetToDamage;
        private IDamageStats kamikazeDamageStats;

        private float remainingPotentialDamage;

        private const float KAMIKAZE_DAMAGE_MULTIPLIER = 1;

        public void Initialise(IPvPUnit parentAircraft, ITarget target)
        {
            PvPHelper.AssertIsNotNull(parentAircraft);

            _parentAircraft = parentAircraft;
            _initialTarget = target;
            _targetToDamage = null;

            remainingPotentialDamage = parentAircraft.MaxHealth * KAMIKAZE_DAMAGE_MULTIPLIER;

            List<TargetType> targetTypes = new List<TargetType>() { TargetType.Buildings, TargetType.Cruiser, TargetType.Ships };
            _targetFilter = PvPTargetFactoriesProvider.FilterFactory.CreateTargetFilter(_initialTarget.Faction, targetTypes);

            kamikazeDamageStats = new DamageStats(remainingPotentialDamage, parentAircraft.Size.x);
            _damageApplier = new PvPAreaOfEffectDamageApplier(kamikazeDamageStats, new FactionTargetFilter(_initialTarget.Faction));

            _explosionPoolProvider = PvPFactoryProvider.PoolProviders.ExplosionPoolProvider;

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
                float prevTargetHP = _targetToDamage.Health;
                kamikazeDamageStats = new DamageStats(
                    Mathf.Min(remainingPotentialDamage, _targetToDamage.Health),
                    _parentAircraft.Size.x);

                _damageApplier = new PvPAreaOfEffectDamageApplier(kamikazeDamageStats, new FactionTargetFilter(_initialTarget.Faction));

                _damageApplier.ApplyDamage(_targetToDamage, _parentAircraft.Position, damageSource: _parentAircraft);
                _explosionPoolProvider.FlakExplosionsPool.GetItem(transform.position);

                float damageDealt = prevTargetHP - _targetToDamage.Health;
                _parentAircraft.TakeDamage(damageDealt / KAMIKAZE_DAMAGE_MULTIPLIER, _parentAircraft);

                if (_targetToDamage.Health == 0f)
                    _targetToDamage = null;

                if (damageDealt < remainingPotentialDamage)
                {
                    remainingPotentialDamage -= damageDealt;
                    kamikazeDamageStats = new DamageStats(
                            damage: remainingPotentialDamage,
                            damageRadiusInM: _parentAircraft.Size.x);
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
