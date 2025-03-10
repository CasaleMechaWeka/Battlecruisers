using System;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine.Assertions;
using BattleCruisers.Utils.BattleScene.Update;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update
{
    /// <summary>
    /// Emits the Updated event during the first frame after the specified amount of time.
    /// </summary>
    public class PvPMultiFrameUpdater : IUpdater
    {
        private readonly IDeltaTimeProvider _timeProvider;
        private readonly float _intervalInS;

        public float DeltaTime { get; private set; }

        public event EventHandler Updated;

        public PvPMultiFrameUpdater(IUpdater perFrameUpdater, IDeltaTimeProvider timeProvider, float intervalInS)
        {
            PvPHelper.AssertIsNotNull(perFrameUpdater, timeProvider);
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