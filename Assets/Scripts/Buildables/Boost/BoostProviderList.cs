using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Boost
{
    public class BoostProviderList : IBoostProviderList
    {
        private IList<IBoostProvider> _boostProviders;
        public ReadOnlyCollection<IBoostProvider> BoostProviders { get; private set; }

        public event EventHandler ProvidersChanged;

        public BoostProviderList()
        {
            _boostProviders = new List<IBoostProvider>();
            BoostProviders = new ReadOnlyCollection<IBoostProvider>(_boostProviders);
		}

        public void AddBoostProvider(IBoostProvider boostProvider)
        {
            Assert.IsFalse(_boostProviders.Contains(boostProvider));
            _boostProviders.Add(boostProvider);
            EmitChangedEvent();
        }

        public void RemoveBoostProvider(IBoostProvider boostProvider)
        {
            Assert.IsTrue(_boostProviders.Contains(boostProvider));
            _boostProviders.Remove(boostProvider);
            EmitChangedEvent();
        }

        private void EmitChangedEvent()
        {
            if (ProvidersChanged != null)
            {
                ProvidersChanged.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
