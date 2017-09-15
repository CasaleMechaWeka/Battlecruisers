using System;
using System.Collections.Generic;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Boost
{
    public class BoostableGroup : IBoostableGroup
	{
        private readonly IList<IBoostable> _boostables;

        // Need multiple boost provider lists.  Eg:
        // 1. List of local boosters for building
        // 2. List of global boosters that apply to building (eg, cruiser benefit
        //      that improves all turrets).
        private readonly IList<IObservableCollection<IBoostProvider>> _boostProviders;

        // FELIX  Make this private?  And not injected?
		public IBoostConsumer BoostConsumer { get; private set; }

        public BoostableGroup(IBoostConsumer boostConsumer)
        {
            Assert.IsNotNull(boostConsumer);

            BoostConsumer = boostConsumer;
            _boostables = new List<IBoostable>();
            _boostProviders = new List<IObservableCollection<IBoostProvider>>();

            BoostConsumer.BoostChanged += _boostConsumer_BoostChanged;
        }

        private void _boostConsumer_BoostChanged(object sender, EventArgs e)
        {
            foreach (IBoostable boostable in _boostables)
            {
                boostable.BoostMultiplier = BoostConsumer.CumulativeBoost;
            }
        }

        public void AddBoostable(IBoostable boostable)
        {
            Assert.IsFalse(_boostables.Contains(boostable), "Not allowed to add duplicates, tsk tsk tsk");

            _boostables.Add(boostable);
            boostable.BoostMultiplier = BoostConsumer.CumulativeBoost;
        }
		
		public bool RemoveBoostable(IBoostable boostable)
		{
            Assert.IsTrue(_boostables.Contains(boostable));
            return _boostables.Remove(boostable);
		}

        public void AddBoostProvidersList(IObservableCollection<IBoostProvider> boostProviders)
        {
            Assert.IsFalse(_boostProviders.Contains(boostProviders));
            _boostProviders.Add(boostProviders);

            foreach (IBoostProvider provider in boostProviders.Items)
            {
                provider.AddBoostConsumer(BoostConsumer);
            }

            boostProviders.Changed += BoostProviders_Changed;
        }

        private void BoostProviders_Changed(object sender, CollectionChangedEventArgs<IBoostProvider> e)
        {
			switch (e.Type)
			{
				case ChangeType.Add:
					e.Item.AddBoostConsumer(BoostConsumer);
					break;

				case ChangeType.Remove:
					e.Item.RemoveBoostConsumer(BoostConsumer);
					break;
			}
		}

		public void CleanUp()
		{
            Assert.IsNotNull(BoostConsumer, "CleanUp() should only be called once.");

            foreach (IObservableCollection<IBoostProvider> boostProviders in _boostProviders)
            {
                foreach (IBoostProvider provider in boostProviders.Items)
                {
                    provider.RemoveBoostConsumer(BoostConsumer);
                }

                boostProviders.Changed -= BoostProviders_Changed;
			}

			BoostConsumer.BoostChanged -= _boostConsumer_BoostChanged;

            BoostConsumer = null;
		}
    }
}
