using BattleCruisers.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.UI.Common.Click;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools
{
    public class PvPBuildingActivationArgs : PvPBuildableActivationArgs
    {
        public IPvPSlot ParentSlot { get; }
        public IDoubleClickHandler<IPvPBuilding> DoubleClickHandler { get; }

        public PvPBuildingActivationArgs(
            IPvPCruiser parentCruiser,
            IPvPCruiser enemyCruiser,
            IPvPCruiserSpecificFactories cruiserSpecificFactories,
            IPvPSlot parentSlot,
            IDoubleClickHandler<IPvPBuilding> doubleClickHandler,
            int variantIndex)
            : base(parentCruiser, enemyCruiser, cruiserSpecificFactories, variantIndex)
        {
            Helper.AssertIsNotNull(parentSlot, doubleClickHandler);

            ParentSlot = parentSlot;
            DoubleClickHandler = doubleClickHandler;
        }
    }
}
