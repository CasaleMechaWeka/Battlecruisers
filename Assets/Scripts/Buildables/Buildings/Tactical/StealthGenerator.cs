using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Buildables.Buildings.Tactical
{
    public class StealthGenerator : TacticalBuilding, IStealthGenerator
    {
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.StealthGenerator;
    }
}
