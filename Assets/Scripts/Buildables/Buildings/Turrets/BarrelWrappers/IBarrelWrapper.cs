using System;
using System.Collections.Generic;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Targets;
using BattleCruisers.Utils;

namespace BattleCruisers.Buildables.Buildings.Turrets.BarrelWrappers
{
    public interface IBarrelWrapper : ITargetConsumer, IDisposable
	{
        TurretStats TurretStats { get; }

        void StaticInitialise();
        void Initialise(IFactoryProvider factoryProvider, Faction enemyFaction, IList<TargetType> attackCapabilities);
        void StartAttackingTargets();
	}
}
