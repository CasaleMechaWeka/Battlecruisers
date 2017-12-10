using BattleCruisers.Projectiles.DamageAppliers;
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

        public void Initialise(IUnit parentAircraft, ITargetFilter targetFilter, IDamageApplier damageApplier)
        {
            Helper.AssertIsNotNull(parentAircraft, targetFilter, damageApplier);

            _parentAircraft = parentAircraft;
            _targetFilter = targetFilter;
			_damageApplier = damageApplier;
        }

		void OnTriggerEnter2D(Collider2D collider)
		{
			ITarget target = collider.gameObject.GetComponent<ITarget>();

			if (target != null 
                && _targetFilter.IsMatch(target)
                && !target.IsDestroyed
                && !_parentAircraft.IsDestroyed)
			{
                _damageApplier.ApplyDamage(target, transform.position);
                _parentAircraft.Destroy();
			}
		}
    }
}
