
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.LoadoutScreen.Comparisons
{
    public interface IPvPComparableItem
    {
        Sprite Sprite { get; }
        string Description { get; }
        string Name { get; }
    }
}

