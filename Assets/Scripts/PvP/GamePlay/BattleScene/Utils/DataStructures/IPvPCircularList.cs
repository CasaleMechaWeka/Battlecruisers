using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures
{
    public interface IPvPCircularList<T>
    {
        int Index { get; set; }

        T Next();
        T Current();
        ReadOnlyCollection<T> Items { get; }
    }
}
