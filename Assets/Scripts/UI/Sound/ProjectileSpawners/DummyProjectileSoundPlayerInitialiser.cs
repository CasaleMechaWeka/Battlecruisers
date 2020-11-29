using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public class DummyProjectileSoundPlayerInitialiser : MonoBehaviour, IProjectileSoundPlayerInitialiser
    {
        public Task<IProjectileSpawnerSoundPlayer> CreateSoundPlayerAsync(ISoundPlayerFactory soundPlayerFactory, ISoundKey firingSound, int burstSize)
        {
            Assert.IsNotNull(soundPlayerFactory);
            return Task.FromResult(soundPlayerFactory.DummyPlayer);
        }
    }
}
