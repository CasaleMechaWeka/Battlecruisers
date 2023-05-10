using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update
{
    public class PvPTogglableUpdater : MonoBehaviour, IPvPUpdater
    {
        private IPvPBroadcastingFilter _enabledFilter;

        public float DeltaTime => Time.deltaTime;

        public event EventHandler Updated;

        public void Initialise(IPvPBroadcastingFilter enabledFilter)
        {
            Assert.IsNotNull(enabledFilter);
            _enabledFilter = enabledFilter;
        }

        void Update()
        {
            if (_enabledFilter != null
                && _enabledFilter.IsMatch)
            {
                Updated?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}