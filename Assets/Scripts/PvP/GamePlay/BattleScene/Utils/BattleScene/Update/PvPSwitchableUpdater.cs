using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update
{
    public class PvPSwitchableUpdater : MonoBehaviour, IPvPSwitchableUpdater
    {
        public float DeltaTime => Time.deltaTime;
        public bool Enabled { get; set; } = true;

        public event EventHandler Updated;

        void Update()
        {
            if (Enabled)
            {
                Updated?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}