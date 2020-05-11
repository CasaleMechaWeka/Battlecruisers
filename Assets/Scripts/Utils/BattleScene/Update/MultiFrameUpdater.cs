using System;
using UnityCommon.PlatformAbstractions.Time;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene.Update
{
    /// <summary>
    /// Emits the Updated event during the first frame after the specified amount of time.
    /// </summary>
    public class MultiFrameUpdater : IUpdater
    {
        private readonly IDeltaTimeProvider _timeProvider;
        private readonly float _intervalInS;

        public float DeltaTime { get; private set; }

        public event EventHandler Updated;

        public MultiFrameUpdater(IUpdater perFrameUpdater, IDeltaTimeProvider timeProvider, float intervalInS)
        {
            Helper.AssertIsNotNull(perFrameUpdater, timeProvider);
            Assert.IsTrue(intervalInS > 0);

            _timeProvider = timeProvider;
            _intervalInS = intervalInS;
            DeltaTime = 0;

            perFrameUpdater.Updated += PerFrameUpdater_Updated;
        }

        private void PerFrameUpdater_Updated(object sender, EventArgs e)
        {
            DeltaTime += _timeProvider.DeltaTime;

            if (DeltaTime >= _intervalInS)
            {
                Updated?.Invoke(this, EventArgs.Empty);
                DeltaTime = 0;
            }
        }
    }
}