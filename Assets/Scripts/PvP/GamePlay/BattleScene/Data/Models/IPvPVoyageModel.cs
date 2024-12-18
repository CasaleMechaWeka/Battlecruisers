namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models
{

    public interface IPvPVoyageModel
    {

        // The stage of the voyage
        int LegNumber { get; set; }

        // The number of the battle within the stage
        int BattleNumber { get; set; }

        // The number of battles won so far in the current voyage
        int BattlesWon { get; set; }

        // Whether the current voyage is in progress
        bool VoyageInProgress { get; set; }
    }
}