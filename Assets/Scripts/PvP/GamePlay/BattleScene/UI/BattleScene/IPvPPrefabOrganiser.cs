using System.Collections.Generic;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    public interface IPvPPrefabOrganiser
    {
        IList<IPvPBuildingGroup> GetBuildingGroups();
        IDictionary<UnitCategory, IList<IPvPBuildableWrapper<IPvPUnit>>> GetUnits();
    }
}