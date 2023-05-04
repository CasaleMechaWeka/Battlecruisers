using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils
{
    public enum PvPChangeDirection
    {
        Down, Up, Both
    }

    public interface IPvPRandomGenerator
    {
        // Random value between 0 and 1 inclusive.
        float Value { get; }

        bool NextBool();
        float RangeFromCenter(float center, float radius);
        float Range(IPvPRange<float> range);
        float Range(float minInclusive, float maxInclusive);
        int Range(int minInclusive, int maxInclusive);
        float Randomise(float baseValue, float maxChangeByProportionOfBaseValue, PvPChangeDirection changeDirection);
        TItem RandomItem<TItem>(IList<TItem> items);
    }
}
