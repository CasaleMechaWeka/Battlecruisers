using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Targets;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.Factories;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public interface IBarrelWrapper : ITargetConsumer, IManagedDisposable
    {
        IDamageCapability DamageCapability { get; }
        float RangeInM { get; }
        Vector2 Position { get; }
        IList<Renderer> Renderers { get; }

		void StaticInitialise();

        void Initialise(
            ITarget parent, 
            IFactoryProvider factoryProvider, 
            Faction enemyFaction, 
            IObservableCollection<IBoostProvider> globalFireRateBoostProviders,
            ISoundKey firingSound = null,
            IObservableCollection<IBoostProvider> localBoostProviders = null);
	}
}
