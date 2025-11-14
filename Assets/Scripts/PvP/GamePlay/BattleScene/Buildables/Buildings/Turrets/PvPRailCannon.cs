using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets
{
    public class PvPRailCannon : PvPOffenseTurret
    {
        protected override SoundKey FiringSound => SoundKeys.Firing.RailCannon;
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.Railgun;
    }
}