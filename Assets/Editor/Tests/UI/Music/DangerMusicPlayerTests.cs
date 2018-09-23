using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;
using BattleCruisers.UI.Music;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Tests.UI.Music
{
    public class DangerMusicPlayerTests
    {
        private DangerMusicPlayer _dangerMusicPlayer;
        private IMusicPlayer _musicPlayer;
        private IDangerMonitor _dangerMonitor;
        private IVariableDelayDeferrer _deferrer;

        [SetUp]
        public void TestSetup()
        {
            _musicPlayer = Substitute.For<IMusicPlayer>();
            _dangerMonitor = Substitute.For<IDangerMonitor>();
            _deferrer = Substitute.For<IVariableDelayDeferrer>();

            _dangerMusicPlayer = new DangerMusicPlayer(_musicPlayer, _dangerMonitor, _deferrer);
        }

        [Test]
        public void SweetTest()
        {
        }
    }
}