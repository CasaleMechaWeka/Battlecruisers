using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost
{
    public class PvPBoostableGroup : IPvPBoostableGroup
    {
        private readonly IPvPBoostConsumer _boostConsumer;
        private readonly IList<IPvPBoostable> _boostables;
        private bool _isCleanedUp;

        // Need multiple boost provider lists.  Eg:
        // 1. List of local boosters for building
        // 2. List of global boosters that apply to building (eg, cruiser benefit
        //      that improves all turrets).
        private readonly IList<ObservableCollection<IPvPBoostProvider>> _boostProviders;

        public event EventHandler BoostChanged;

        public PvPBoostableGroup(IPvPBoostFactory boostFactory)
        {
            Assert.IsNotNull(boostFactory);

            _boostConsumer = boostFactory.CreateBoostConsumer();
            _boostables = new List<IPvPBoostable>();
            _boostProviders = new List<ObservableCollection<IPvPBoostProvider>>();
            _isCleanedUp = false;

            _boostConsumer.BoostChanged += _boostConsumer_BoostChanged;
        }

        private void _boostConsumer_BoostChanged(object sender, EventArgs e)
        {
            foreach (IPvPBoostable boostable in _boostables)
            {
                boostable.BoostMultiplier = _boostConsumer.CumulativeBoost;
            }

            BoostChanged?.Invoke(this, EventArgs.Empty);
        }

        public void AddBoostable(IPvPBoostable boostable)
        {
            Assert.IsFalse(_boostables.Contains(boostable), "Not allowed to add duplicates, tsk tsk tsk");

            _boostables.Add(boostable);
            boostable.BoostMultiplier = _boostConsumer.CumulativeBoost;
        }

        public bool RemoveBoostable(IPvPBoostable boostable)
        {
            Assert.IsTrue(_boostables.Contains(boostable));
            return _boostables.Remove(boostable);
        }

        public void AddBoostProvidersList(ObservableCollection<IPvPBoostProvider> boostProviders)
        {
            Assert.IsFalse(_boostProviders.Contains(boostProviders));
            _boostProviders.Add(boostProviders);

            foreach (IPvPBoostProvider provider in boostProviders)
            {
                provider.AddBoostConsumer(_boostConsumer);
            }

            boostProviders.CollectionChanged += BoostProviders_CollectionChanged;
        }

        private void BoostProviders_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Assert.AreEqual(1, e.NewItems.Count);
                    ((IPvPBoostProvider)e.NewItems[0]).AddBoostConsumer(_boostConsumer);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    Assert.AreEqual(1, e.OldItems.Count);
                    ((IPvPBoostProvider)e.OldItems[0]).RemoveBoostConsumer(_boostConsumer);
                    break;
            }
        }

        public void CleanUp()
        {
            Assert.IsFalse(_isCleanedUp, "CleanUp() should only be called once.");

            foreach (ObservableCollection<IPvPBoostProvider> boostProviders in _boostProviders)
            {
                foreach (IPvPBoostProvider provider in boostProviders)
                {
                    provider.RemoveBoostConsumer(_boostConsumer);
                }

                boostProviders.CollectionChanged -= BoostProviders_CollectionChanged;
            }

            _boostConsumer.BoostChanged -= _boostConsumer_BoostChanged;

            _isCleanedUp = true;
        }
    }
}
