using UnityCommon.Properties;
using UnityEngine;

namespace BattleCruisers.Utils.BattleScene
{
    public class LifetimeEventBroadcaster : MonoBehaviour, ILifetimeEventBroadcaster
    {
        private ISettableBroadcastingProperty<bool> _isPaused;
        public IBroadcastingProperty<bool> IsPaused { get; private set; }

        public void Initialise()
        {
            _isPaused = new SettableBroadcastingProperty<bool>(initialValue: false);
            IsPaused = new BroadcastingProperty<bool>(_isPaused);
        }

        void OnApplicationFocus(bool hasFocus)
        {
            Logging.Log(Tags.LIFETIME_EVENTS, $"hasFocus: {hasFocus}");
            _isPaused.Value = !hasFocus;
        }

        void OnApplicationPause(bool pauseStatus)
        {
            Logging.Log(Tags.LIFETIME_EVENTS, $"pauseStatus: {pauseStatus}");
            _isPaused.Value = pauseStatus;
        }
    }
}