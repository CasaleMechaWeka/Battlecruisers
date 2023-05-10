using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update
{
    public class PvPPerFrameUpdater : MonoBehaviour, IPvPUpdater
    {
        public float DeltaTime => Time.deltaTime;

        public event EventHandler Updated;

        void Update()
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }
    }
}