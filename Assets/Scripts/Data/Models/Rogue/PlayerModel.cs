using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.Data.Models
{
    public class PlayerModel : IPlayerModel
    {
        public int TotalUpgrades { get; set; } = 0;
        public int TotalPerks { get; set; } = 0;
        public int TotalBuildables { get; set; } = 0;
        public int PlayerBounty { get; set; } = 0;
        public int PlayerLevel { get; set; } = 1;
        public int CurrentLuck { get; set; } = 0;
        public int BaseLuck { get; set; } = 0;
        public int ExtraDrones { get; set; } = 0;
        public int CurrentHP { get; set; } = 0;
        public int MaxHP { get; set; } = 1;
        public int Credits { get; set; } = 0;

        public int Coins { get; set; } = 0;

//        public List<Perk> Perks { get; set; }
        public int TotalVoyages { get; set; } = 0;
    }
}
