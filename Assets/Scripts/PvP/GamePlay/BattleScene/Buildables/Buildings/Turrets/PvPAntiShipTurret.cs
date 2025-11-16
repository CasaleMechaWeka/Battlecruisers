using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets
{
    public class PvPAntiShipTurret : PvPDefenseTurret
    {
        protected override SoundKey FiringSound => SoundKeys.Firing.BigCannon;
        protected override PrioritisedSoundKey ConstructionCompletedSoundKey => PrioritisedSoundKeys.Completed.Buildings.AntiShipTurret;
    }
}
