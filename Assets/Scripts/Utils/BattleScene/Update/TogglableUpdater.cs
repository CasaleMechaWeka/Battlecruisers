using BattleCruisers.UI.Filters;
using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils.BattleScene.Update
{
    public class TogglableUpdater : MonoBehaviour, IUpdater
    {
        private IBroadcastingFilter _enabledFilter;

        public event EventHandler Updated;

        public void Initialise(IBroadcastingFilter enabledFilter)
        {
            Assert.IsNotNull(enabledFilter);
            _enabledFilter = enabledFilter;
        }

        void Update()
        {
            if (_enabledFilter.IsMatch)
            {
                Updated?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}