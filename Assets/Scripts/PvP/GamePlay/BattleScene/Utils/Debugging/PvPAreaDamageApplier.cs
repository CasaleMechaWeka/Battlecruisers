using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.DamageAppliers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Targets.TargetFinders.Filters;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Debugging
{
    /// <summary>
    /// Applies area of effect damage whenever the "D" key is pressed, 
    /// at the location of the mouse.  For testing :P
    /// </summary>
    public class PvPAreaDamageApplier : PvPCheaterBase
    {
        private IDamageApplier _areaDamageApplier;

        public float damage;
        public float damageRadiusInM;
        public KeyCode hotkey;

        void Start()
        {
            IDamageStats damageStats = new PvPDamageStats(damage, damageRadiusInM);
            ITargetFilter targetFilter = new DummyTargetFilter(isMatchResult: true);
            _areaDamageApplier = new PvPAreaOfEffectDamageApplier(damageStats, targetFilter);
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
