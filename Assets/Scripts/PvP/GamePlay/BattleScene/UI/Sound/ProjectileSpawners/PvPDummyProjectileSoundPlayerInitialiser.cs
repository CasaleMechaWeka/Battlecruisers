using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.ProjectileSpawners
{
    public class PvPDummyProjectileSoundPlayerInitialiser : MonoBehaviour, IProjectileSoundPlayerInitialiser
    {
        public Task<IProjectileSpawnerSoundPlayer> CreateSoundPlayerAsync(
            ISoundPlayerFactory soundPlayerFactory,
            ISoundKey firingSound,
            int burstSize,
            ISettingsManager settingsManager)
        {
            Assert.IsNotNull(soundPlayerFactory);
            return Task.FromResult(soundPlayerFactory.DummyPlayer);
        }
    }
}
