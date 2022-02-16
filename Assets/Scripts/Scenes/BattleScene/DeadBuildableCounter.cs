using System;
using UnityEngine;
using System.Collections.Generic;
namespace BattleCruisers.Scenes.BattleScene
{
    public class DeadBuildableCounter
    {
        private long count;
        private long credits;

        public DeadBuildableCounter()
        {
            credits = 0;
            count = 0;
        }

        public void AddDeadBuildable(int value)
        {
            count++;
            credits += value;
        }

        public long GetTotalDestroyed()
        {
            return count;
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