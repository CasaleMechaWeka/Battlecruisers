using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting
{
    public interface IPvPBuildableSorter<TBuildable> where TBuildable : class, IPvPBuildable
    {
        IList<IPvPBuildableWrapper<TBuildable>> Sort(IList<IPvPBuildableWrapper<TBuildable>> buildables);
    }
}
