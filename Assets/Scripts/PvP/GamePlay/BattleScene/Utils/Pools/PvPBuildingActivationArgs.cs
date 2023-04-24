using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildable.Buildings;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils
{
    public class PvPBuildingActivationArgs : BuildableActivationArgs
    {
        public IPvPSlot ParentSlot { get; }
        public IDoubleClickHandler<IPvPBuilding> DoubleClickHandler { get; }

        public PvPBuildingActivationArgs(
            IPvPCruiser parentCruiser,
            IPvPCruiser enemyCruiser,
            IPvPCruiserSpecificFactories cruiserSpecificFactories,
            IPvPSlot parentSlot,
            IPvPDoubleClickHandler<IPvPBuilding> doubleClickHandler)
            : base(parentCruiser, enemyCruiser, cruiserSpecificFactories)
        {
            Helper.AssertIsNotNull(parentSlot, doubleClickHandler);

            ParentSlot = parentSlot;
            DoubleClickHandler = doubleClickHandler;
        }
    }
}
