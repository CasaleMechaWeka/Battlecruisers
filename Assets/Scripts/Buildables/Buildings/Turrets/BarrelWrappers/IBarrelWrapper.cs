using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Effects;
using BattleCruisers.Targets;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public interface IBarrelWrapper : ITargetConsumer, IManagedDisposable
    {
        IDamageCapability DamageCapability { get; }
        float RangeInM { get; }
        Vector2 Position { get; }
        IList<SpriteRenderer> Renderers { get; }

		void StaticInitialise();

        void Initialise(
            ITarget parent,
            IFactoryProvider factoryProvider,
            ICruiserSpecificFactories cruiserSpecificFactories,
            Faction enemyFaction,
            ISoundKey firingSound = null,
            ObservableCollection<IBoostProvider> localBoostProviders = null,
            ObservableCollection<IBoostProvider> globalFireRateBoostProviders = null,
            IAnimation barrelFiringAnimation = null);
	}
}
