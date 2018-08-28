using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Buildables.Buildings.Factories
{
    public class Ultralisk : DroneStation
    {
        protected override ISoundKey ConstructionCompletedSoundKey { get { return SoundKeys.Completed.Ultra; } }
    }
}
