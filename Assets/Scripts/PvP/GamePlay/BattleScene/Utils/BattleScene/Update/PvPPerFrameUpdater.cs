using System;
using UnityEngine;
using BattleCruisers.Utils.BattleScene.Update;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update
{
    public class PvPPerFrameUpdater : MonoBehaviour, IUpdater
    {
        public float DeltaTime => Time.deltaTime;

        public event EventHandler Updated;

        void Update()
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }
    }
}