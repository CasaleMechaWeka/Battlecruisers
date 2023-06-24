using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelWrappers
{
    public interface IPvPBarrelWrapper : IPvPTargetConsumer, IPvPManagedDisposable
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
            IPvPSoundKey firingSound = null,
            ObservableCollection<IPvPBoostProvider> localBoostProviders = null,
            ObservableCollection<IPvPBoostProvider> globalFireRateBoostProviders = null,
            IPvPAnimation barrelFiringAnimation = null);

        void Initialise(
            IPvPBuildable parent,
            IPvPFactoryProvider factoryProvider,
            IPvPSoundKey firingSound = null,
            IPvPAnimation barrelFiringAnimation = null);
    }
}
