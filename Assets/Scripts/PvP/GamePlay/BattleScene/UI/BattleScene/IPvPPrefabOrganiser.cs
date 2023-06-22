using System.Collections.Generic;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene
{
    public interface IPvPPrefabOrganiser
    {
        Task<IList<IPvPBuildingGroup>> GetBuildingGroups();
        IDictionary<PvPUnitCategory, IList<IPvPBuildableWrapper<IPvPUnit>>> GetUnits();
    }
}