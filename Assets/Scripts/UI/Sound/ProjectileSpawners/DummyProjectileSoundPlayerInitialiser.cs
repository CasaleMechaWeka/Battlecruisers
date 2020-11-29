using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public class DummyProjectileSoundPlayerInitialiser : MonoBehaviour, IProjectileSoundPlayerInitialiser
    {
        public Task<IProjectileSpawnerSoundPlayer> CreateSoundPlayerAsync(ISoundPlayerFactory soundPlayerFactory, ISoundKey firingSound, int burstSize)
        {
            return Task.FromResult(soundPlayerFactory.DummyPlayer);
        }
    }
}
