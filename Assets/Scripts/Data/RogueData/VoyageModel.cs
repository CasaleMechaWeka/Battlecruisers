using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Data.Models
{
    public class VoyageModel : IVoyageModel
    {

        public int VoyageNumber { get; set; } = 1;

        /* The stage of the voyage
        public int LegNumber { get; set; } = 1;

        // The number of the battle within the stage
        public int BattleNumber { get; set; } = 1;

        // The number of battles won so far in the current voyage
        public int BattlesWon { get; set; } = 0;

        // Is there a voyage in progress?
        public bool VoyageInProgress { get; set; } = false; */


    }
}