using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Targets;
using BattleCruisers.UI.Sound;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public interface IPvPBarrelWrapper : ITargetConsumer, IPvPManagedDisposable
    {
        IPvPDamageCapability DamageCapability { get; }
        float RangeInM { get; }
        Vector2 Position { get; }
        IList<SpriteRenderer> Renderers { get; }

        void StaticInitialise();

        void Initialise(
            IPvPBuildable parent,
            IPvPFactoryProvider factoryProvider,
            IPvPCruiserSpecificFactories cruiserSpecificFactories,
            ISoundKey firingSound = null,
            ObservableCollection<IPvPBoostProvider> localBoostProviders = null,
            ObservableCollection<IPvPBoostProvider> globalFireRateBoostProviders = null,
            IPvPAnimation barrelFiringAnimation = null);

        void Initialise(
            IPvPBuildable parent,
            IPvPFactoryProvider factoryProvider,
            ISoundKey firingSound = null,
            IPvPAnimation barrelFiringAnimation = null);

        void ApplyVariantStats(IPvPBuilding building);
        void ApplyVariantStats(IPvPUnit unit);
    }
}
