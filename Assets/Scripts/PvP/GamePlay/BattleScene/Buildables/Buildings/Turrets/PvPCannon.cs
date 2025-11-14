using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets
{
    public class PvPCannon : PvPOffenseTurret
    {
        protected override SoundKey FiringSound => SoundKeys.Firing.Broadsides;
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.Artillery;
    }
}
