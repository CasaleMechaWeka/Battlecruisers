using BattleCruisers.Buildables.Boost;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost
{
    public class PvPBoostProvider : IBoostProvider
    {
        private readonly IList<IBoostConsumer> _boostConsumers;

        public float BoostMultiplier { get; }

        public PvPBoostProvider(float boostMultiplier)
        {
            BoostMultiplier = boostMultiplier;
            _boostConsumers = new List<IBoostConsumer>();
        }

        public void AddBoostConsumer(IBoostConsumer boostConsumer)
        {
            _boostConsumers.Add(boostConsumer);
            boostConsumer.AddBoostProvider(this);
        }

        public void RemoveBoostConsumer(IBoostConsumer boostConsumer)
        {
            Assert.IsTrue(_boostConsumers.Contains(boostConsumer));

            _boostConsumers.Remove(boostConsumer);
            boostConsumer.RemoveBoostProvider(this);
        }
    }
}
