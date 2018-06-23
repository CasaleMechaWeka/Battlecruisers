namespace BattleCruisers.UI.Sound.ProjectileSpawners
{
    public interface IProjectileSoundPlayerInitialiser
    {
        IProjectileSpawnerSoundPlayer CreateSoundPlayer(ISoundPlayerFactory soundPlayerFactory, ISoundKey firingSound, int burstSize);
    }
}
