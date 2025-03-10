using BattleCruisers.Utils.BattleScene.Update;
using System;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update
{
    public class PvPPhysicsUpdater : MonoBehaviour, IUpdater
    {
        public float DeltaTime => Time.deltaTime;

        public event EventHandler Updated;

        void FixedUpdate()
        {
            Updated?.Invoke(this, EventArgs.Empty);
        }
    }
}