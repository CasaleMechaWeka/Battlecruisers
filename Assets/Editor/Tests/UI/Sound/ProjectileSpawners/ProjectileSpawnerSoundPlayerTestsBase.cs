using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Sound.ProjectileSpawners
{
    public abstract class ProjectileSpawnerSoundPlayerTestsBase
    {
        protected IProjectileSpawnerSoundPlayer _soundPlayer;

        protected AudioClipWrapper _audioClip;
        protected IAudioSource _audioSource;

        [SetUp]
        public virtual void TestSetup()
        {
            _audioClip = Substitute.For<AudioClipWrapper>();
            _audioSource = Substitute.For<IAudioSource>();
        }
    }
}
