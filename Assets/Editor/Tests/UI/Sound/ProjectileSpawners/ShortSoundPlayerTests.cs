using BattleCruisers.UI.Sound.ProjectileSpawners;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Sound.ProjectileSpawners
{
    public class ShortSoundPlayerTests : ProjectileSpawnerSoundPlayerTestsBase
    {
        [SetUp]
        public override void TestSetup()
        {
            base.TestSetup();

            _soundPlayer = new ShortSoundPlayer(_audioClip, _audioSource);
        }

        [Test]
        public void OnProjectileFired()
        {
            _soundPlayer.OnProjectileFired();
            _audioSource.Received().Play();
        }
    }
}
