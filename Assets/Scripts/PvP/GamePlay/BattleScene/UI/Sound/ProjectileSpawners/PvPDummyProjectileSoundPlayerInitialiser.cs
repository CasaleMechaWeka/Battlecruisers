using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners
{
    public class PvPDummyProjectileSoundPlayerInitialiser : MonoBehaviour, IPvPProjectileSoundPlayerInitialiser
    {
        public Task<IPvPProjectileSpawnerSoundPlayer> CreateSoundPlayerAsync(
            IPvPSoundPlayerFactory soundPlayerFactory,
            ISoundKey firingSound,
            int burstSize,
            ISettingsManager settingsManager)
        {
            Assert.IsNotNull(soundPlayerFactory);
            return Task.FromResult(soundPlayerFactory.DummyPlayer);
        }
    }
}
