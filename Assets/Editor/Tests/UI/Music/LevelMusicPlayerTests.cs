using BattleCruisers.UI.Music;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Tests.UI.Music
{
    public class LevelMusicPlayerTests
    {
#pragma warning disable CS0414  // Variable is assigned but never used
        private LevelMusicPlayer _levelMusicPlayer;
#pragma warning restore CS0414  // Variable is assigned but never used
        private ILayeredMusicPlayer _musicPlayer;
        private IDangerMonitor _dangerMonitor;
        private IDeferrer _deferrer;
        private IBattleCompletionHandler _battleCompletionHandler;
        private IList<Action> _deferredActions;
        private const float EXPECTED_DEFER_TIME_IN_S = 15;

        [SetUp]
        public void TestSetup()
        {
            _musicPlayer = Substitute.For<ILayeredMusicPlayer>();
            _dangerMonitor = Substitute.For<IDangerMonitor>();
            _deferrer = Substitute.For<IDeferrer>();
            _battleCompletionHandler = Substitute.For<IBattleCompletionHandler>();

            _levelMusicPlayer = new LevelMusicPlayer(_musicPlayer, _dangerMonitor, _deferrer, _battleCompletionHandler);

            _deferredActions = new List<Action>();
            _deferrer.Defer(Arg.Do<Action>(action => _deferredActions.Add(action)), EXPECTED_DEFER_TIME_IN_S);
        }

        [Test]
        public void Constructor()
        {
            _musicPlayer.Received().Play();
        }

        [Test]
        public void DangerEvent_PlaysDangerMusic_DefersChangeMusicBack()
        {
            _dangerMonitor.DangerStart += Raise.Event();

            _musicPlayer.Received().PlaySecondary();
            Assert.AreEqual(1, _deferredActions.Count);

            _deferredActions[0].Invoke();
            _musicPlayer.Received().StopSecondary();
        }

        [Test]
        public void SecondDangerBeforeDeferralRuns_DoesNotStopDangerMusic()
        {
            // First danger event
            _dangerMonitor.DangerStart += Raise.Event();
            Assert.AreEqual(1, _deferredActions.Count);

            // Second danger event
            _dangerMonitor.DangerStart += Raise.Event();
            Assert.AreEqual(2, _deferredActions.Count);

            // Run first deferral => Does nothing
            _deferredActions[0].Invoke();
            _musicPlayer.DidNotReceive().StopSecondary();

            // Run second deferral => Stops danger music
            _deferredActions[1].Invoke();
            _musicPlayer.Received().StopSecondary();
        }

        [Test]
        public void BattleCompleted_StopsMusic()
        {
            _battleCompletionHandler.BattleCompleted += Raise.Event();
            _musicPlayer.Received().Stop();
        }
    }
}