using System;
using System.Collections.Generic;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Targets;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public interface IBarrelWrapper : ITargetConsumer, IDisposable, IBoostable
	{
        // FELIX  Replace with Damage class :)
        float DamagePerS { get; }

        float RangeInM { get; }
        Vector2 Position { get; }
        IList<Renderer> Renderers { get; }

		void StaticInitialise();

        void Initialise(
            ITarget parent, 
            IFactoryProvider factoryProvider, 
            Faction enemyFaction, 
            IList<TargetType> attackCapabilities,
            ISoundKey firingSound = null);

        void StartAttackingTargets();
	}
}
