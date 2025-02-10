using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using BattleCruisers.Utils.BattleScene.Lifetime;
using BattleCruisers.Utils.Properties;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Lifetime
{
    public class PvPLifetimeEventBroadcaster : MonoBehaviour, ILifetimeEventBroadcaster
    {
        private ISettableBroadcastingProperty<bool> _isPaused;
        public IBroadcastingProperty<bool> IsPaused { get; private set; }

        void Awake()
        {
            _isPaused = new PvPSettableBroadcastingProperty<bool>(initialValue: false);
            IsPaused = new PvPBroadcastingProperty<bool>(_isPaused);
        }

        void OnApplicationFocus(bool hasFocus)
        {
            // Logging.Log(Tags.LIFETIME_EVENTS, $"hasFocus: {hasFocus}");
            //_isPaused.Value = !hasFocus;
        }

        void OnApplicationPause(bool pauseStatus)
        {
            // Logging.Log(Tags.LIFETIME_EVENTS, $"pauseStatus: {pauseStatus}");
            _isPaused.Value = pauseStatus;
        }
    }
}