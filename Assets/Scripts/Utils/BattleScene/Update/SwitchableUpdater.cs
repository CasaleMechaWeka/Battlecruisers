using System;
using UnityEngine;

namespace BattleCruisers.Utils.BattleScene.Update
{
    public class SwitchableUpdater : MonoBehaviour, ISwitchableUpdater
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