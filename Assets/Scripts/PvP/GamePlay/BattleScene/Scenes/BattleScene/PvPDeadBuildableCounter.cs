using System;
using UnityEngine;
using System.Collections.Generic;
namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public class PvPDeadBuildableCounter
    {
        private long count;
        private long credits;
        private float playedTime;

        public PvPDeadBuildableCounter()
        {
            credits = 0;
            count = 0;
            playedTime = 0f;
        }

        public void AddDeadBuildable(int value)
        {
            count++;
            credits += value;
        }

        public void AddPlayedTime(float dt)
        {
            playedTime += dt;
        }
        public long GetTotalDestroyed()
        {
            return count;
        }
        public float GetPlayedTime()
        {
            return playedTime;
        }


        public long GetTotalDamageInCredits()
        {
            return credits;
        }

        public override string ToString()
        {
            return "Count: " + count + ", Credits: " + credits;
        }
    }
}