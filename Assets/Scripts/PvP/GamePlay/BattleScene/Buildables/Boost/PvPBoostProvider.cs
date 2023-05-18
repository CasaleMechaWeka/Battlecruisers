using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost
{
    public class PvPBoostProvider : IPvPBoostProvider
    {
        private readonly IList<IPvPBoostConsumer> _boostConsumers;

        public float BoostMultiplier { get; }

        public PvPBoostProvider(float boostMultiplier)
        {
            BoostMultiplier = boostMultiplier;
            _boostConsumers = new List<IPvPBoostConsumer>();
        }

        public void AddBoostConsumer(IPvPBoostConsumer boostConsumer)
        {
            _boostConsumers.Add(boostConsumer);
            boostConsumer.AddBoostProvider(this);
        }

        public void RemoveBoostConsumer(IPvPBoostConsumer boostConsumer)
        {
            Assert.IsTrue(_boostConsumers.Contains(boostConsumer));

            _boostConsumers.Remove(boostConsumer);
            boostConsumer.RemoveBoostProvider(this);
        }
    }
}
