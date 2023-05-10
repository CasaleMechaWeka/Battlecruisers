using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update
{
    /// <summary>
    /// Emits the Updated event during the first frame after the specified amount of time.
    /// </summary>
    public class PvPMultiFrameUpdater : IPvPUpdater
    {
        private readonly IPvPDeltaTimeProvider _timeProvider;
        private readonly float _intervalInS;

        public float DeltaTime { get; private set; }

        public event EventHandler Updated;

        public PvPMultiFrameUpdater(IPvPUpdater perFrameUpdater, IPvPDeltaTimeProvider timeProvider, float intervalInS)
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