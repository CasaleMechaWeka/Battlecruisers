using System;
using UnityCommon.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene.Update
{
    /// <summary>
    /// Emits the Updated event during the first frame after the specified amount of time.
    /// </summary>
    /// FELIX  Use
    public class MultiFrameUpdater : IUpdater
    {
        private readonly IDeltaTimeProvider _timeProvider;
        private readonly float _intervalInS;
        private float _timeSinceUpdateInS;

        public event EventHandler Updated;

        public MultiFrameUpdater(IUpdater perFrameUpdater, IDeltaTimeProvider timeProvider, float intervalInS)
        {
            Helper.AssertIsNotNull(perFrameUpdater, timeProvider);
            Assert.IsTrue(intervalInS > 0);

            _timeProvider = timeProvider;
            _intervalInS = intervalInS;
            _timeSinceUpdateInS = 0;

            perFrameUpdater.Updated += PerFrameUpdater_Updated;
        }

        private void PerFrameUpdater_Updated(object sender, EventArgs e)
        {
            _timeSinceUpdateInS += _timeProvider.DeltaTime;

            if (_timeSinceUpdateInS >= _intervalInS)
            {
                Updated?.Invoke(this, EventArgs.Empty);
                _timeSinceUpdateInS = 0;
            }
        }
    }
}