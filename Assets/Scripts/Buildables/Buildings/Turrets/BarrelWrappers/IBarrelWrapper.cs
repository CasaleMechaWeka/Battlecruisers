using System;
using System.Collections.Generic;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Targets;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public interface IBarrelWrapper : ITargetConsumer, IDisposable, IBoostable
	{
        TurretStats TurretStats { get; }
        Vector2 Position { get; }
        Renderer[] Renderers { get; }

		void StaticInitialise();
        void Initialise(IFactoryProvider factoryProvider, Faction enemyFaction, IList<TargetType> attackCapabilities);
        void StartAttackingTargets();
	}
}
