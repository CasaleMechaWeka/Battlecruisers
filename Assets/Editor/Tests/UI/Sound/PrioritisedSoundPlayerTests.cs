using BattleCruisers.Data.Static;
using BattleCruisers.UI.Sound;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Sound
{
    public class PrioritisedSoundPlayerTests
    {
        private IPrioritisedSoundPlayer _prioritisedSoundPlayer;
        private ISingleSoundPlayer _singleSoundPlayer;
        private PrioritisedSoundKey _lowPrioritySound, _highPrioritySound;

        [SetUp]
        public void TestSetup()
        {
            _singleSoundPlayer = Substitute.For<ISingleSoundPlayer>();
            _prioritisedSoundPlayer = new PrioritisedSoundPlayer(_singleSoundPlayer);

            _lowPrioritySound = new PrioritisedSoundKey(SoundKeys.Completed.SpySatellite, SoundPriority.Low);
            _highPrioritySound = new PrioritisedSoundKey(SoundKeys.Completed.Ultra, SoundPriority.High);
        }

        [Test]
        public void PlaySound_FirstSound_Plays()
        {
            _prioritisedSoundPlayer.PlaySound(_lowPrioritySound);
            _singleSoundPlayer.Received().PlaySound(_lowPrioritySound.Key);
        }

        [Test]
        public void PlaySound_SecondSound_FirstSoundCompleted_Plays()
        {
            // First sound
            _prioritisedSoundPlayer.PlaySound(_lowPrioritySound);
            _singleSoundPlayer.Received().PlaySound(_lowPrioritySound.Key);

            // Second sound
            _singleSoundPlayer.IsPlayingSound.Returns(true);
            _prioritisedSoundPlayer.PlaySound(_lowPrioritySound);
            _singleSoundPlayer.Received().PlaySound(_lowPrioritySound.Key);
        }

        [Test]
        public void PlaySound_SecondSound_FirstSoundInProgress_SecondSoundHigherPriority_Plays()
        {
            // First sound
            _prioritisedSoundPlayer.PlaySound(_lowPrioritySound);
            _singleSoundPlayer.Received().PlaySound(_lowPrioritySound.Key);

            // Second sound
            _singleSoundPlayer.IsPlayingSound.Returns(true);
            _prioritisedSoundPlayer.PlaySound(_highPrioritySound);
            _singleSoundPlayer.Received().PlaySound(_highPrioritySound.Key);
        }

        [Test]
        public void PlaySound_SecondSound_FirstSoundInProgress_SecondSoundSamePriority_DoesNotPlay()
        {
            // First sound
            _prioritisedSoundPlayer.PlaySound(_lowPrioritySound);
            _singleSoundPlayer.Received().PlaySound(_lowPrioritySound.Key);

            // Second sound
            _singleSoundPlayer.ClearReceivedCalls();
            _singleSoundPlayer.IsPlayingSound.Returns(true);
            _prioritisedSoundPlayer.PlaySound(_lowPrioritySound);
            _singleSoundPlayer.DidNotReceiveWithAnyArgs().PlaySound(null);
        }
    }
}