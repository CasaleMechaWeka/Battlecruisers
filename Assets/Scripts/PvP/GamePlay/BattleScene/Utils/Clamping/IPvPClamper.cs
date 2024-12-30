using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Clamping
{
    public interface IPvPClamper
    {
        float Clamp(float value, IRange<float> validRange);
    }
}