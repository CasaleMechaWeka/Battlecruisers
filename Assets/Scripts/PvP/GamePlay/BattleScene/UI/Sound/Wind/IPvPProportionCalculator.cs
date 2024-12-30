using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Wind
{
    public interface IPvPProportionCalculator
    {
        float FindProportion(float value, IRange<float> range);
    }
}