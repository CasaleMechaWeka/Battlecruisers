using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using UnityEngine;
using System.Collections.Generic;

namespace BattleCruisers.Utils.BattleScene
{
    public class Remover : MonoBehaviour 
	{
		void OnTriggerEnter2D(Collider2D collider)
		{
            IRemovable removable = collider.GetComponent<IRemovable>();

            if (removable != null)
            {
                // Check if this is a projectile hitting water
                if (IsProjectileHittingWater(collider))
                {
                    HandleProjectileWaterImpact(collider, removable);
                }
                else
                {
                    removable.RemoveFromScene();
                }
                return;
            }

            IRemovable targetRemovable = collider.GetComponent<ITargetProxy>()?.Target as IRemovable;

            if (targetRemovable != null)
            {
                targetRemovable.RemoveFromScene();
            }
		}

        private bool IsProjectileHittingWater(Collider2D collider)
        {
            var removable = collider.GetComponent<IRemovable>();
            if (removable == null) return false;

            var projectileController = collider.GetComponent<BattleCruisers.Projectiles.ProjectileControllerBase>();
            var pvpProjectileController = collider.GetComponent<BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.PvPProjectileControllerBase>();
            
            return projectileController != null || pvpProjectileController != null;
        }

        private static Dictionary<Vector2Int, float> _lastWaterImpactTimeByZone = new Dictionary<Vector2Int, float>();
        private const float ZONE_SIZE = 2.0f; // 2m x 2m zones
        private const float IMPACT_COOLDOWN = 0.05f; // 50ms between effects per zone

        private void HandleProjectileWaterImpact(Collider2D collider, IRemovable removable)
        {
            Vector2Int zone = GetZoneKey(collider.transform.position);
            float currentTime = Time.time;
            
            if (_lastWaterImpactTimeByZone.TryGetValue(zone, out float lastTime) && 
                currentTime - lastTime < IMPACT_COOLDOWN)
            {
                // Skip effect, just remove projectile
                removable.RemoveFromScene();
                return;
            }
            
            // Play effect and update time for this zone
            _lastWaterImpactTimeByZone[zone] = currentTime;
            removable.RemoveFromScene();
        }

        private Vector2Int GetZoneKey(Vector2 position)
        {
            return new Vector2Int(
                Mathf.FloorToInt(position.x / ZONE_SIZE),
                Mathf.FloorToInt(position.y / ZONE_SIZE)
            );
        }
	}
}
