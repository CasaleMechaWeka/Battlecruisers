using BattleCruisers.UI.Music;
using BattleCruisers.Utils.BattleScene;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Music
{
    public class LevelMusicPlayerTests
    {
#pragma warning disable CS0414  // Variable is assigned but never used
        private LevelMusicPlayer _levelMusicPlayer;
#pragma warning restore CS0414  // Variable is assigned but never used
        private ILayeredMusicPlayer _musicPlayer;
        private IDangerMonitorSummariser _dangerMonitorSummariser;
        private IBattleCompletionHandler _battleCompletionHandler;

        [SetUp]
        public void TestSetup()
        {
            _musicPlayer = Substitute.For<ILayeredMusicPlayer>();
            _dangerMonitorSummariser = Substitute.For<IDangerMonitorSummariser>();
            _battleCompletionHandler = Substitute.For<IBattleCompletionHandler>();

            _levelMusicPlayer = new LevelMusicPlayer(_musicPlayer, _dangerMonitorSummariser, _battleCompletionHandler);
        }

        [Test]
        public void Constructor()
        {
            _musicPlayer.Received().Play();
        }

        [Test]
        public void IsInDanger_ValueChanged_True()
        {
            _dangerMonitorSummariser.IsInDanger.Value.Returns(true);
            _dangerMonitorSummariser.IsInDanger.ValueChanged += Raise.Event();
            _musicPlayer.Received().PlaySecondary();
        }

        [Test]
        public void IsInDanger_ValueChanged_False()
        {
            _dangerMonitorSummariser.IsInDanger.Value.Returns(false);
            _dangerMonitorSummariser.IsInDanger.ValueChanged += Raise.Event();
            _musicPlayer.Received().StopSecondary();
        }

        [Test]
        public void BattleCompleted_StopsMusic()
        {
            _battleCompletionHandler.BattleCompleted += Raise.Event();
            _musicPlayer.Received().Stop();
            _musicPlayer.Received().DisposeManagedState();
        }
    }
}