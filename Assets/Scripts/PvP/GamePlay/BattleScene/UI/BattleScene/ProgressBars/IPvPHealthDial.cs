using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.ProgressBars
{
    public interface IPvPHealthDial<TDamagable> where TDamagable : IPvPDamagable
    {
        TDamagable Damagable { set; }
    }
}