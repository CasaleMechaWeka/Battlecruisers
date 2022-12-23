using System.Collections.Generic;
using System.Collections.ObjectModel;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Data.Models
{
    public interface IPlayerModel
    {
        // The number of upgrades in the player's current loadout
        int TotalUpgrades { get; set; }

        // The number of Perks applied to the player
        int TotalPerks { get; set; }

        // The number of buildables in the player's inventory
        int TotalBuildables { get; set; }

        // PlayerBounty
        int PlayerBounty { get; set; }

        // based on first 4 Integers above, resets each voyage, used to rank enemies and loot
        int PlayerLevel { get; set; }

        // base luck
        int BaseLuck { get; set; }

        // current luck
        int CurrentLuck { get; set; }

        // Starting number of extra drones
        int ExtraDrones { get; set; }

        // Current hit points
        int CurrentHP { get; set; }

        // Maximum hit points
        int MaxHP { get; set; }

        // Credits
        int Credits { get; set; }

        // List of perks
 //       List<Perk> Perks { get; set; } 

        // Total number of voyages
        int TotalVoyages { get; set; }

    }
}
