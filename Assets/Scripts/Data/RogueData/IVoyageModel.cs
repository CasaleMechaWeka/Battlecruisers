using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data.Models
{

    public interface IVoyageModel
    {
        int VoyageNumber { get; set; }

        /* TODO The stage of the voyage
        int LegNumber { get; set; }

        // The number of the battle within the stage
        int BattleNumber { get; set; }

        // The number of battles won so far in the current voyage
        int BattlesWon { get; set; }

        // Whether the current voyage is in progress
        bool VoyageInProgress { get; set; } */

    }
}