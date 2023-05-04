using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units.Aircraft
{
    public class PvPKamikazeController : MonoBehaviour
    {
        private IPvPUnit _parentAircraft;
        private IPvPTargetFilter _targetFilter;
        private IPvPDamageApplier _damageApplier;
        private IPvPExplosionPoolProvider _explosionPoolProvider;
        private IPvPTarget _initialTarget;

        // Have this to defer damaging the target until the next FixedUpdate(), because
        // there is a bug in Unity that if the target is destroyed from OnTriggerEnter2D()
        // the target collider does not trigger OnTriggerExit2D().  I filed a bug with
        // Unity so *hopefully* this is fixed one day and I can remove this deferral :)
        private IPvPTarget _targetToDamage;

        private const float KAMIKAZE_DAMAGE_MULTIPLIER = 2;

        public void Initialise(IPvPUnit parentAircraft, IPvPFactoryProvider factoryProvider, IPvPTarget target)
        {
            PvPHelper.AssertIsNotNull(parentAircraft, factoryProvider);

            _parentAircraft = parentAircraft;
            _initialTarget = target;
            _targetToDamage = null;

            List<PvPTargetType> targetTypes = new List<PvPTargetType>() { PvPTargetType.Buildings, PvPTargetType.Cruiser, PvPTargetType.Ships };
            _targetFilter = factoryProvider.Targets.FilterFactory.CreateTargetFilter(_initialTarget.Faction, targetTypes);

            IPvPDamageStats kamikazeDamageStats
                = factoryProvider.DamageApplierFactory.CreateDamageStats(
                    damage: parentAircraft.MaxHealth * KAMIKAZE_DAMAGE_MULTIPLIER,
                    damageRadiusInM: parentAircraft.Size.x);
            _damageApplier = factoryProvider.DamageApplierFactory.CreateFactionSpecificAreaOfDamageApplier(kamikazeDamageStats, _initialTarget.Faction);

            _explosionPoolProvider = factoryProvider.PoolProviders.ExplosionPoolProvider;

            _initialTarget.Destroyed += Target_Destroyed;
        }

        private void Target_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            if (!_parentAircraft.IsDestroyed)
            {
                _explosionPoolProvider.FlakExplosionsPool.GetItem(transform.position);
                CleanUp();
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            IPvPTarget target = collider.gameObject.GetComponent<IPvPTargetProxy>()?.Target;

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
                _damageApplier.ApplyDamage(_targetToDamage, _parentAircraft.Position, damageSource: _parentAircraft);
                _explosionPoolProvider.FlakExplosionsPool.GetItem(transform.position);
                CleanUp();
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
