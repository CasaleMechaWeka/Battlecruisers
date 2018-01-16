using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Buildables.Buildings.Turrets
{
    public class Mortar : TurretController
	{
        protected override ISoundKey FiringSound { get { return SoundKeys.Firing.Artillery; } }
    }
}
