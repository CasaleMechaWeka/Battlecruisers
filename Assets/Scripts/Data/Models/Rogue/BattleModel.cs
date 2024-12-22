using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel
{

    // The total number of upgrades
    public int TotalUpgrades { get; set; }

    // The total number of perks
    public int TotalPerks { get; set; }

    // The total number of buildables
    public int TotalBuildables { get; set; }

    // The player's bounty
    public int PlayerBounty { get; set; }

    // The player's level, based on the first four integers above
    public double PlayerLevel { get; set; }

    // The player's luck
    public int Luck { get; set; }

    // The starting number of drones
    public int StartingDrones { get; set; }

    // The player's current hit points
    public int CurrentHP { get; set; }

    // The player's maximum hit points
    public int MaxHP { get; set; }

    // The player's rank
    public int PlayerRank { get; set; }

    // The player's lifetime destruction score
    public int PlayerLifetimeDestruction { get; set; }

    // The player's credits
    public int Credits { get; set; }

    // The list of perks

    // The list of schematics in the loadout


    // The list of upgrades in the loadout

}