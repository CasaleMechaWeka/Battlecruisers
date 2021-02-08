using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Sound.ProjectileSpawners
{
    public abstract class ProjectileSpawnerSoundPlayerTestsBase
    {
        protected IProjectileSpawnerSoundPlayer _soundPlayer;

        protected IAudioClipWrapper _audioClip;
        protected IAudioSource _audioSource;

        [SetUp]
        public virtual void TestSetup()
        {
            _audioClip = Substitute.For<IAudioClipWrapper>();
            _audioSource = Substitute.For<IAudioSource>();
        }
    }
}
