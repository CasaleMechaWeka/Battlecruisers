using System;
using System.Collections.Generic;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Targets;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public interface IBarrelWrapper : ITargetConsumer, IDisposable, IBoostable
	{
        float DamagePerS { get; }
        float RangeInM { get; }
        Vector2 Position { get; }
        IList<Renderer> Renderers { get; }

		void StaticInitialise();
        void Initialise(IFactoryProvider factoryProvider, Faction enemyFaction, IList<TargetType> attackCapabilities);
        void StartAttackingTargets();
	}
}
