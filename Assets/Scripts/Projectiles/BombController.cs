using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Projectiles
{
    public class BombController : ProjectileController
    {
        protected override ISoundKey ImpactSoundKey => SoundKeys.Explosions.Bomb;
    }
}
