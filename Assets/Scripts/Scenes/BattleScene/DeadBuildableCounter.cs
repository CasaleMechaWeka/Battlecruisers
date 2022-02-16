using System;
using UnityEngine;
using System.Collections.Generic;
namespace BattleCruisers.Scenes.BattleScene
{
    public class DeadBuildableCounter
    {
        private int count;
        private long dollars;

        public DeadBuildableCounter()
        {
            dollars = 0;
            count = 0;
        }

        public void AddDeadBuildable(int value)
        {
            count++;
            dollars += value;
        }

        public int GetTotalDestroyed()
        {
            return count;
        }

        public long GetTotalDamageInCredits()
        {
            return dollars;
        }
    }
}