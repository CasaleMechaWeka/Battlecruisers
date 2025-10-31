using BattleCruisers.Data.Static;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class MissileLauncher : OffenseTurret
    {
        protected override bool HasSingleSprite => true;
        public ProjectileType projectileType = ProjectileType.Rocket;
    }
}
