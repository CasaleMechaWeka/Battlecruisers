using System;
using UnityEngine;

namespace BattleCruisers.Utils.BattleScene.Update
{
    public class PerFrameUpdater : MonoBehaviour, IUpdater
    {
        public float DeltaTime => Time.deltaTime;

        public event EventHandler Updated;

        void Update()
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }
    }
}