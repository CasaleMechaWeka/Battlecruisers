using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Utils.Debugging
{
    /// <summary>
    /// Applies area of effect damage whenever the "D" key is pressed, 
    /// at the location of the mouse.  For testing :P
    /// </summary>
    /// TEMP  Turn this class off for final game :P
    public class AreaDamageApplier : MonoBehaviour
    {
        private IDamageApplier _areaDamageApplier;

        public float damage;
        public float damageRadiusInM;
        public KeyCode hotkey;

        void Start()
        {
            if (!Debug.isDebugBuild)
            {
                Destroy(gameObject);
            }
            else
            {
                IDamageStats damageStats = new DamageStats(damage, damageRadiusInM);
                ITargetFilter targetFilter = new DummyTargetFilter(isMatchResult: true);
                _areaDamageApplier = new AreaOfEffectDamageApplier(damageStats, targetFilter);
            }
        }

        void Update()
        {
            if (Input.GetKeyUp(hotkey))
            {
                Vector2 collisionPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                _areaDamageApplier
                    .ApplyDamage(
                    target: null,
                    collisionPoint: collisionPoint,
                    damageSource: null);
            }
        }
    }
}
