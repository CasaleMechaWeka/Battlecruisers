using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using Buildings = BattleCruisers.Data.Static.StaticPrefabKeys.Buildings;
using Units = BattleCruisers.Data.Static.StaticPrefabKeys.Units;
using Hulls = BattleCruisers.Data.Static.StaticPrefabKeys.Hulls;
using Exos = BattleCruisers.Data.Static.StaticPrefabKeys.CaptainExos;
using BackgroundMusic = BattleCruisers.Data.Static.SoundKeys.Music.Background;
using BattleCruisers.Data.Static.LevelLoot;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine.Assertions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies.Helper;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using System;

namespace BattleCruisers.Data.Static
{
    public class StaticData : IStaticData
    {
        private ReadOnlyDictionary<BuildingKey, int> _buildingToUnlockedLevel
        = new ReadOnlyDictionary<BuildingKey, int>(new Dictionary<BuildingKey, int>()
        {
            // Factories
            { Buildings.AirFactory, 1 },  //The number represents the first *main story* level you get this item, so it unlocks when you win the previous level.
            { Buildings.NavalFactory, 1 },
            { Buildings.DroneStation, 1 },
            { Buildings.DroneStation4, 27 },
            { Buildings.DroneStation6, 95 },
            { Buildings.DroneStation8, 31 },

            // Tactical
            { Buildings.ShieldGenerator, 2 },
            { Buildings.LocalBooster, 9 },
            { Buildings.StealthGenerator, 17 },
            { Buildings.SpySatelliteLauncher, 18 },
            { Buildings.ControlTower, 24 },
            { Buildings.GrapheneBarrier, 95 },

            // Defence
            { Buildings.AntiShipTurret, 1 },
            { Buildings.AntiAirTurret, 1 },
            { Buildings.Mortar, 3 },
            { Buildings.SamSite, 5 },
            { Buildings.TeslaCoil, 21 },
            { Buildings.Coastguard, 39 },
            { Buildings.FlakTurret, 95 },
            { Buildings.CIWS, 95 },

            // Offence
            { Buildings.Artillery, 1 },
            { Buildings.RocketLauncher, 20 },
            { Buildings.Railgun, 6 },
            { Buildings.MLRS, 29},
            { Buildings.GatlingMortar, 32},
            { Buildings.MissilePod, 36 },
            { Buildings.IonCannon, 38 },
            { Buildings.Cannon, 95 },
            { Buildings.BlastVLS, 95 },
            { Buildings.FirecrackerVLS, 95 },

            // Ultras
            { Buildings.DeathstarLauncher, 7 },
            { Buildings.NukeLauncher, 10 },
            { Buildings.Ultralisk, 14 },
            { Buildings.KamikazeSignal, 22 },
            { Buildings.Broadsides, 25 },
            { Buildings.NovaArtillery, 33 },
            { Buildings.UltraCIWS, 95 },
            { Buildings.GlobeShield, 95 },
            { Buildings.Sledgehammer, 95 } //Set to 95: way past the highest main story level, so that the sidequest unlocks it instead.
        });
        private ReadOnlyDictionary<UnitKey, int> _unitToUnlockedLevel
        = new ReadOnlyDictionary<UnitKey, int>(new Dictionary<UnitKey, int>()
        {
            // Aircraft
            { Units.Bomber, 1 },
            { Units.Gunship, 5 },
            { Units.Fighter, 12 },
            { Units.SteamCopter, 28 },
            { Units.Broadsword, 41 },
            { Units.StratBomber, 95 },
            { Units.SpyPlane, 95 },
            { Units.MissileFighter, 95 },
            
            // Ships
            { Units.AttackBoat, 1 },
            { Units.Frigate, 3 },
            { Units.Destroyer, 13 },
            { Units.SiegeDestroyer, 95 },
            { Units.GlassCannoneer, 95 },
            { Units.GunBoat, 95 },
            { Units.ArchonBattleship, 16 },
            { Units.AttackRIB, 30 },
            { Units.RocketTurtle, 95 },
            { Units.FlakTurtle, 95 }
        });
        private ReadOnlyDictionary<HullKey, int> _hullToUnlockedLevel
        = new ReadOnlyDictionary<HullKey, int>(new Dictionary<HullKey, int>()
        {
            { Hulls.Trident, 1 },
            { Hulls.Raptor, 4 },
            { Hulls.Bullshark, 8 },
            { Hulls.Rockjaw, 11},
            { Hulls.Eagle, 15 },
            { Hulls.Hammerhead, 19 },
            { Hulls.Longbow, 23 },
            { Hulls.Megalodon, 26 },
            { Hulls.Megalith, 45 },
            { Hulls.Rickshaw, 34 },
            { Hulls.TasDevil, 35 },
            { Hulls.BlackRig, 37 },
            { Hulls.Yeti, 40 }
        });

        private ReadOnlyDictionary<BuildingKey, int> _buildingToCompletedSideQuest
        = new ReadOnlyDictionary<BuildingKey, int>(new Dictionary<BuildingKey, int>()
        {
            //Factories
            { Buildings.DroneStation6, 22 },

            // Defence
            { Buildings.MissilePod, 3 },
            { Buildings.Coastguard, 6 },
            { Buildings.CIWS, 10 },
            { Buildings.FlakTurret, 14 },

            //Tactical
            { Buildings.GrapheneBarrier, 11 },
            { Buildings.GlobeShield, 23 },

            // Offence
            { Buildings.IonCannon, 5 },
            { Buildings.Cannon, 28 },
            { Buildings.BlastVLS, 31 },
            { Buildings.FirecrackerVLS, 32 },

            // Ultras
            { Buildings.NovaArtillery, 0 },
            { Buildings.UltraCIWS, 18 },
            { Buildings.Sledgehammer, 30 }
        });
        private ReadOnlyDictionary<UnitKey, int> _unitToCompletedSideQuest
        = new ReadOnlyDictionary<UnitKey, int>(new Dictionary<UnitKey, int>()
        {
            // Aircraft
            { Units.Broadsword, 8 },
            { Units.StratBomber, 9 },
            { Units.MissileFighter, 24 },
            { Units.SpyPlane, 29 },
            
            // Ships
            { Units.GlassCannoneer, 16 },
            { Units.SiegeDestroyer, 17 },
            { Units.RocketTurtle, 19 },
            { Units.GunBoat, 20 },
            { Units.FlakTurtle, 21 }
        });
        private ReadOnlyDictionary<HullKey, int> _hullToCompletedSideQuest
        = new ReadOnlyDictionary<HullKey, int>(new Dictionary<HullKey, int>()
        {
            { Hulls.Rickshaw, 1 },
            { Hulls.TasDevil, 2 },
            { Hulls.BlackRig, 4 },
            { Hulls.Yeti, 7 },
            { Hulls.Microlodon, 12},
            { Hulls.Flea, 13 },
            { Hulls.Shepherd, 15},
            { Hulls.Pistol, 26},
            { Hulls.Goatherd, 27},
            { Hulls.Megalith, 25}
        });

        private const int MIN_AVAILABILITY_LEVEL_NUM = 2;
        public const int NUM_OF_LEVELS = 31;
        public const int NUM_OF_PvPLEVELS = 9;
        public const int NUM_OF_STANDARD_LEVELS = 31;
        public const int NUM_OF_LEVELS_IN_DEMO = 7;
        public const int NUM_OF_SIDEQUESTS = 31;

        public GameModel InitialGameModel { get; }
        public ReadOnlyCollection<ILevel> Levels { get; } = new ReadOnlyCollection<ILevel>(new List<ILevel>()
        {
            // Set 1:  Raptor
            new Level(1, Hulls.Raptor, BackgroundMusic.Bobby, SkyMaterials.Morning, Exos.GetCaptainExoKey(1)),
            new Level(2, Hulls.Hammerhead, BackgroundMusic.Juggernaut, SkyMaterials.Purple, Exos.GetCaptainExoKey(2)),
            new Level(3, Hulls.Raptor, BackgroundMusic.Experimental, SkyMaterials.Dusk, Exos.GetCaptainExoKey(3)),
            
            // Set 2:  Bullshark
            new Level(4, Hulls.Rockjaw, BackgroundMusic.Nothing, SkyMaterials.Cold, Exos.GetCaptainExoKey(4)),
            new Level(5, Hulls.Bullshark, BackgroundMusic.Confusion, SkyMaterials.Midday, Exos.GetCaptainExoKey(5)),
            new Level(6, Hulls.Raptor, BackgroundMusic.Sleeper, SkyMaterials.Midnight, Exos.GetCaptainExoKey(6)),
            new Level(7, Hulls.Bullshark, BackgroundMusic.Bobby, SkyMaterials.Sunrise, Exos.GetCaptainExoKey(7)),

            // Set 3:  Rockjaw
            new Level(8, Hulls.Hammerhead, BackgroundMusic.Nothing, SkyMaterials.Cold, Exos.GetCaptainExoKey(8)),
            new Level(9, Hulls.Eagle, BackgroundMusic.Juggernaut, SkyMaterials.Morning, Exos.GetCaptainExoKey(9)),
            new Level(10, Hulls.Rockjaw, BackgroundMusic.Againagain, SkyMaterials.Purple, Exos.GetCaptainExoKey(10)),

            // Set 4:  Eagle
            new Level(11, Hulls.Longbow, BackgroundMusic.Sleeper, SkyMaterials.Midnight, Exos.GetCaptainExoKey(11)),
            new Level(12, Hulls.Bullshark, BackgroundMusic.Nothing, SkyMaterials.Midday, Exos.GetCaptainExoKey(12)),
            new Level(13, Hulls.Rockjaw, BackgroundMusic.Confusion, SkyMaterials.Dusk, Exos.GetCaptainExoKey(13)),
            new Level(14, Hulls.Eagle, BackgroundMusic.Bobby, SkyMaterials.Sunrise, Exos.GetCaptainExoKey(14)),
            new Level(15, Hulls.ManOfWarBoss, BackgroundMusic.Juggernaut, SkyMaterials.Midnight, Exos.GetCaptainExoKey(15)),

            // Set 5:  Hammerhead
            new Level(16, Hulls.Longbow, BackgroundMusic.Experimental, SkyMaterials.Morning, Exos.GetCaptainExoKey(16)),
            new Level(17, Hulls.Hammerhead, BackgroundMusic.Nothing, SkyMaterials.Midday, Exos.GetCaptainExoKey(17)),
            new Level(18, Hulls.Rickshaw, BackgroundMusic.Juggernaut, SkyMaterials.Dusk, Exos.GetCaptainExoKey(18)),

            // Set 6:  Longbow
            new Level(19, Hulls.Eagle, BackgroundMusic.Sleeper, SkyMaterials.Purple, Exos.GetCaptainExoKey(19)),
            new Level(20, Hulls.Rockjaw, BackgroundMusic.Againagain, SkyMaterials.Midnight, Exos.GetCaptainExoKey(20)),
            new Level(21, Hulls.Hammerhead, BackgroundMusic.Nothing, SkyMaterials.Cold, Exos.GetCaptainExoKey(21)),
            new Level(22, Hulls.Longbow, BackgroundMusic.Confusion, SkyMaterials.Sunrise, Exos.GetCaptainExoKey(22)),

            // Set 7:  Megalodon
            new Level(23, Hulls.Bullshark, BackgroundMusic.Bobby, SkyMaterials.Dusk, Exos.GetCaptainExoKey(23)),
            new Level(24, Hulls.Longbow, BackgroundMusic.Juggernaut, SkyMaterials.Midnight, Exos.GetCaptainExoKey(24)),
            new Level(25, Hulls.Raptor, BackgroundMusic.Nothing, SkyMaterials.Morning, Exos.GetCaptainExoKey(25)),
            new Level(26, Hulls.Megalodon, BackgroundMusic.Confusion, SkyMaterials.Midday, Exos.GetCaptainExoKey(26)),
            
                // Set 8:  Huntress Prime
            new Level(27, Hulls.TasDevil, BackgroundMusic.Experimental, SkyMaterials.Purple, Exos.GetCaptainExoKey(27)),
            new Level(28, Hulls.BlackRig, BackgroundMusic.Juggernaut, SkyMaterials.Cold, Exos.GetCaptainExoKey(28)),
            new Level(29, Hulls.Rickshaw, BackgroundMusic.Againagain, SkyMaterials.Dusk, Exos.GetCaptainExoKey(29)),
            new Level(30, Hulls.Yeti, BackgroundMusic.Confusion, SkyMaterials.Midnight, Exos.GetCaptainExoKey(30)),
            new Level(31, Hulls.HuntressBoss, BackgroundMusic.Bobby, SkyMaterials.Sunrise, Exos.GetCaptainExoKey(31)), //HUNTRESS PRIME

                // Set 9:  Secret Levels
            new Level(32, Hulls.Trident, BackgroundMusic.Experimental, SkyMaterials.Purple, Exos.GetCaptainExoKey(32)),
            new Level(33, Hulls.Raptor, BackgroundMusic.Juggernaut, SkyMaterials.Cold, Exos.GetCaptainExoKey(33)),
            new Level(34, Hulls.Bullshark, BackgroundMusic.Againagain, SkyMaterials.Dusk, Exos.GetCaptainExoKey(34)),
            new Level(35, Hulls.Rockjaw, BackgroundMusic.Confusion, SkyMaterials.Midnight, Exos.GetCaptainExoKey(35)),
            new Level(36, Hulls.Eagle, BackgroundMusic.Bobby, SkyMaterials.Sunrise, Exos.GetCaptainExoKey(36)),
            new Level(37, Hulls.Hammerhead, BackgroundMusic.Sleeper, SkyMaterials.Midday, Exos.GetCaptainExoKey(37)),
            new Level(38, Hulls.Longbow, BackgroundMusic.Nothing, SkyMaterials.Morning, Exos.GetCaptainExoKey(38)),
            new Level(39, Hulls.Megalodon, BackgroundMusic.Juggernaut, SkyMaterials.Sunrise, Exos.GetCaptainExoKey(39)),
            new Level(40, Hulls.TasDevil, BackgroundMusic.Againagain, SkyMaterials.Midnight, Exos.GetCaptainExoKey(40)) //TODO: Change to new boss broadsword
        });
        public ReadOnlyCollection<ISideQuestData> SideQuests { get; } = new ReadOnlyCollection<ISideQuestData>(new List<ISideQuestData>()
        {
            //Set 1: Original Secret Levels
            new SideQuestData(false, Exos.GetCaptainExoKey(32), 32, -1, Hulls.Trident, BackgroundMusic.Experimental, SkyMaterials.Purple, false, 0),
            new SideQuestData(false, Exos.GetCaptainExoKey(33), 32, 0, Hulls.Raptor, BackgroundMusic.Juggernaut, SkyMaterials.Cold, false, 1),
            new SideQuestData(false, Exos.GetCaptainExoKey(34), 32, 1, Hulls.Bullshark, BackgroundMusic.Againagain, SkyMaterials.Dusk, false, 2),
            new SideQuestData(false, Exos.GetCaptainExoKey(35), 32, 2, Hulls.Rockjaw, BackgroundMusic.Confusion, SkyMaterials.Midnight, false, 3),
            new SideQuestData(false, Exos.GetCaptainExoKey(36), 32, 3, Hulls.Eagle, BackgroundMusic.Bobby, SkyMaterials.Sunrise, false, 4),
            new SideQuestData(false, Exos.GetCaptainExoKey(37), 32, 4, Hulls.Hammerhead, BackgroundMusic.Sleeper, SkyMaterials.Midday, false, 5),
            new SideQuestData(false, Exos.GetCaptainExoKey(38), 32, 5, Hulls.Longbow, BackgroundMusic.Nothing, SkyMaterials.Morning, false, 6),
            new SideQuestData(false, Exos.GetCaptainExoKey(39), 32, 6, Hulls.Megalodon, BackgroundMusic.Juggernaut, SkyMaterials.Sunrise, false, 7),
            new SideQuestData(false, Exos.GetCaptainExoKey(40), 32, 7, Hulls.TasDevil, BackgroundMusic.Againagain, SkyMaterials.Midnight, false, 8),

            //Set 2: new SideQuests of BC v6.3
            new SideQuestData(false, Exos.GetCaptainExoKey(41), 8, -1, Hulls.Rickshaw, BackgroundMusic.Bobby, SkyMaterials.Purple, false, 9),
            new SideQuestData(false, Exos.GetCaptainExoKey(42), 11, -1, Hulls.BlackRig, BackgroundMusic.Confusion, SkyMaterials.Midnight, false, 10),
            new SideQuestData(false, Exos.GetCaptainExoKey(2), 16, -1, Hulls.Longbow, BackgroundMusic.Againagain, SkyMaterials.Cold, false, 11),
            new SideQuestData(false, Exos.GetCaptainExoKey(8), 19, -1, Hulls.Microlodon, BackgroundMusic.Nothing, SkyMaterials.Dusk, false, 12),
            new SideQuestData(false, Exos.GetCaptainExoKey(15), 0, 3, Hulls.Flea, BackgroundMusic.Sleeper, SkyMaterials.Sunrise, false, 13),
            new SideQuestData(false, Exos.GetCaptainExoKey(17), 23, -1, Hulls.Shepherd, BackgroundMusic.Experimental, SkyMaterials.Morning, false, 14),
            new SideQuestData(false, Exos.GetCaptainExoKey(50), 0, 9, Hulls.Shepherd, BackgroundMusic.Juggernaut, SkyMaterials.Midday, false, 15),
            new SideQuestData(false, Exos.GetCaptainExoKey(25), 0, 14 , Hulls.Flea, BackgroundMusic.Bobby, SkyMaterials.Dusk, false, 16),
            new SideQuestData(false, Exos.GetCaptainExoKey(29), 27, -1 , Hulls.Microlodon, BackgroundMusic.Confusion, SkyMaterials.Purple, false, 17),
            new SideQuestData(false, Exos.GetCaptainExoKey(1), 0, 17 , Hulls.Rickshaw, BackgroundMusic.Againagain, SkyMaterials.Midnight, false, 18),
            new SideQuestData(false, Exos.GetCaptainExoKey(8), 27, -1 , Hulls.TasDevil, BackgroundMusic.Nothing, SkyMaterials.Cold, false, 19),
            new SideQuestData(false, Exos.GetCaptainExoKey(46), 32, -1 , Hulls.TasDevil, BackgroundMusic.Sleeper, SkyMaterials.Sunrise, false, 20),
            new SideQuestData(false, Exos.GetCaptainExoKey(47), 32, 20 , Hulls.Megalodon, BackgroundMusic.Experimental, SkyMaterials.Morning, false, 21),
            new SideQuestData(false, Exos.GetCaptainExoKey(48), 32, 21 , Hulls.Yeti, BackgroundMusic.Juggernaut, SkyMaterials.Midday, false, 22),
            new SideQuestData(false, Exos.GetCaptainExoKey(49), 32, 22 , Hulls.FortressPrime, BackgroundMusic.Fortress, SkyMaterials.Midnight, false, 23),

            //Set 3: new SideQuests of BC v6.4
            new SideQuestData(false, Exos.GetCaptainExoKey(06), 6, 28, Hulls.Shepherd, BackgroundMusic.Experimental, SkyMaterials.Morning, false, 24), //MissileFighter 
            new SideQuestData(false, Exos.GetCaptainExoKey(4), 11, -1, Hulls.Megalith, BackgroundMusic.Bobby, SkyMaterials.Purple, false, 25), //Megalith
            new SideQuestData(false, Exos.GetCaptainExoKey(13), 13, 10, Hulls.Pistol, BackgroundMusic.Confusion, SkyMaterials.Midnight, false, 26), //Pistol
            new SideQuestData(false, Exos.GetCaptainExoKey(16), 15, 10, Hulls.Goatherd, BackgroundMusic.Againagain, SkyMaterials.Cold, false, 27), //Goatherd
            new SideQuestData(false, Exos.GetCaptainExoKey(44), 4, -1, Hulls.Goatherd, BackgroundMusic.Nothing, SkyMaterials.Dusk, false, 28), //Cannon
            new SideQuestData(false, Exos.GetCaptainExoKey(45), 4, 27, Hulls.Megalith, BackgroundMusic.Sleeper, SkyMaterials.Sunrise, false, 29), //SpyPlane
            new SideQuestData(false, Exos.GetCaptainExoKey(11), 11, 29, Hulls.Pistol, BackgroundMusic.Juggernaut, SkyMaterials.Midday, false, 30) //Sledgehammer
        });
        public ReadOnlyDictionary<Map, IPvPLevel> PvPLevels { get; } = new ReadOnlyDictionary<Map, IPvPLevel>(new Dictionary<Map, IPvPLevel>()
        {
            // Practice Wreckyards
            {Map.PracticeWreckyards,  new PvPLevel(1, PvPStaticPrefabKeys.PvPHulls.PvPRaptor, BackgroundMusic.Bobby, SkyMaterials.Morning)},
            // Oz Penitentiary
            {Map.OzPenitentiary,   new PvPLevel(2, PvPStaticPrefabKeys.PvPHulls.PvPBullshark, BackgroundMusic.Juggernaut, SkyMaterials.Purple)},
            // San Francisco Fight Club
            {Map.SanFranciscoFightClub, new PvPLevel(3, PvPStaticPrefabKeys.PvPHulls.PvPRaptor, BackgroundMusic.Experimental, SkyMaterials.Dusk)},
            // UAC Battle Night
            {Map.UACBattleNight, new PvPLevel(4, PvPStaticPrefabKeys.PvPHulls.PvPRockjaw, BackgroundMusic.Nothing, SkyMaterials.Cold) },
            // Nuclear Dome
            {Map.NuclearDome,  new PvPLevel(5, PvPStaticPrefabKeys.PvPHulls.PvPBullshark, BackgroundMusic.Confusion, SkyMaterials.Midday)},
            // UAC Arena
            {Map.UACArena, new PvPLevel(6, PvPStaticPrefabKeys.PvPHulls.PvPRaptor, BackgroundMusic.Sleeper, SkyMaterials.Midnight)},
            // Rio Battlesport
            {Map.RioBattlesport, new PvPLevel(7, PvPStaticPrefabKeys.PvPHulls.PvPTasDevil, BackgroundMusic.Bobby, SkyMaterials.Sunrise)},
            // UAC Ultimate
            {Map.UACUltimate,  new PvPLevel(8, PvPStaticPrefabKeys.PvPHulls.PvPHammerhead, BackgroundMusic.Nothing, SkyMaterials.Cold)},
            // Mercenary One
            {Map.MercenaryOne,  new PvPLevel(9, PvPStaticPrefabKeys.PvPHulls.PvPEagle, BackgroundMusic.Juggernaut, SkyMaterials.Morning)},
        });
        public ReadOnlyCollection<HullKey> HullKeys { get; } = new ReadOnlyCollection<HullKey>(new List<HullKey>()
        {
            // In order they are available to the user.  Means the loadout
            // screen order is nice :)
            Hulls.Trident,
            Hulls.Bullshark,
            Hulls.Raptor,
            Hulls.Rockjaw,
            Hulls.Eagle,
            Hulls.Flea,
            Hulls.Goatherd,
            Hulls.Hammerhead,
            Hulls.Longbow,
            Hulls.Megalodon,
            Hulls.Megalith,
            Hulls.Microlodon,
            Hulls.BlackRig,
            Hulls.Pistol,
            Hulls.Rickshaw,
            Hulls.Shepherd,
            Hulls.TasDevil,
            Hulls.Yeti,
            Hulls.FortressPrime
        });
        public ReadOnlyCollection<UnitKey> UnitKeys { get; } = new ReadOnlyCollection<UnitKey>(new List<UnitKey>()
        {
            // Aircraft
            Units.Bomber,
            Units.Gunship,
            Units.Fighter,
            Units.SteamCopter,
            Units.Broadsword,
            Units.StratBomber,
            Units.SpyPlane,
            Units.MissileFighter,

            // Ships
            Units.AttackBoat,
            Units.Frigate,
            Units.Destroyer,
            Units.SiegeDestroyer,
            Units.GunBoat,
            Units.ArchonBattleship,
            Units.AttackRIB,
            Units.GlassCannoneer,
            Units.RocketTurtle,
            Units.FlakTurtle
    });
        public ReadOnlyCollection<BuildingKey> BuildingKeys { get; } = new ReadOnlyCollection<BuildingKey>(new List<BuildingKey>()
        {
            // Units in a category (eg:  Aircraft) are in the order they 
            // become available to the user.  Means the loadout screen order is nice :)
            // Factories
            Buildings.AirFactory,
            Buildings.NavalFactory,
            Buildings.DroneStation,
            Buildings.DroneStation4,
            Buildings.DroneStation6,
            Buildings.DroneStation8,

            // Tactical
            Buildings.ShieldGenerator,
            Buildings.LocalBooster,
            Buildings.ControlTower,
            Buildings.StealthGenerator,
            Buildings.SpySatelliteLauncher,
            Buildings.GrapheneBarrier,

            // Defence
            Buildings.AntiShipTurret,
            Buildings.AntiAirTurret,
            Buildings.Mortar,
            Buildings.SamSite,
            Buildings.TeslaCoil,
            Buildings.Coastguard,
            Buildings.FlakTurret,
            Buildings.CIWS,

            // Offence
            Buildings.Artillery,
            Buildings.Railgun,
            Buildings.RocketLauncher,
            Buildings.MLRS,
            Buildings.GatlingMortar,
            Buildings.IonCannon,
            Buildings.MissilePod,
            Buildings.Cannon,//new
            Buildings.BlastVLS,//new
            Buildings.FirecrackerVLS,//new

            // Ultras
            Buildings.DeathstarLauncher,
            Buildings.NukeLauncher,
            Buildings.Ultralisk,
            Buildings.KamikazeSignal,
            Buildings.Broadsides,
            Buildings.NovaArtillery,
            Buildings.UltraCIWS,
            Buildings.GlobeShield,
            Buildings.Sledgehammer//new
        });
        public ReadOnlyCollection<BuildingKey> AIBannedUltrakeys { get; } = new ReadOnlyCollection<BuildingKey>(new List<BuildingKey>()
        {
                // Don't want AI to try and build a kamikaze signal as an ultra,
                // as it is only effective if there are a certain number of planes.
                // Simpler to make the AI only build ultras that are always effective.
                Buildings.KamikazeSignal,

                // Don't want AI to try and build an ultralisk as an ultra,
                // because it is only super effective if building something else
                // afterwards (other offensives, ultras, or units).  As the AI's
                // strategy may be to win with a fast ultra (after which the AI
                // may do nothing), again only let the AI build ultras that
                // are always effective.
                Buildings.Ultralisk
        });
        public IReadOnlyList<HeckleData> Heckles { get; } = new ReadOnlyCollection<HeckleData>(new HeckleData[]
        {
            new HeckleData(15, 0), new HeckleData(15, 1), new HeckleData(15, 2), new HeckleData(15, 3), new HeckleData(15, 4),
            new HeckleData(15, 5), new HeckleData(15, 6), new HeckleData(15, 7), new HeckleData(15, 8), new HeckleData(15, 9),
            new HeckleData(15, 10), new HeckleData(15, 11), new HeckleData(15, 12), new HeckleData(15, 13), new HeckleData(15, 14),
            new HeckleData(15, 15), new HeckleData(15, 16), new HeckleData(15, 17), new HeckleData(15, 18), new HeckleData(15, 19),
            new HeckleData(15, 20), new HeckleData(15, 21), new HeckleData(15, 22), new HeckleData(15, 23), new HeckleData(15, 24),
            new HeckleData(15, 25), new HeckleData(15, 26), new HeckleData(15, 27), new HeckleData(15, 28), new HeckleData(15, 29),
            new HeckleData(15, 30), new HeckleData(15, 31), new HeckleData(15, 32), new HeckleData(15, 33), new HeckleData(15, 34),
            new HeckleData(15, 35), new HeckleData(15, 36), new HeckleData(15, 37), new HeckleData(15, 38), new HeckleData(15, 39),
            new HeckleData(15, 40), new HeckleData(15, 41), new HeckleData(15, 42), new HeckleData(15, 43), new HeckleData(15, 44),
            new HeckleData(15, 45), new HeckleData(15, 46), new HeckleData(15, 47), new HeckleData(15, 48), new HeckleData(15, 49),
            new HeckleData(15, 50), new HeckleData(15, 51), new HeckleData(15, 52), new HeckleData(15, 53), new HeckleData(15, 54),
            new HeckleData(15, 55), new HeckleData(15, 56), new HeckleData(15, 57), new HeckleData(15, 58), new HeckleData(15, 59),
            new HeckleData(15, 60), new HeckleData(15, 61), new HeckleData(15, 62), new HeckleData(15, 63), new HeckleData(15, 64),
            new HeckleData(15, 65), new HeckleData(15, 66), new HeckleData(15, 67), new HeckleData(15, 68), new HeckleData(15, 69),
            new HeckleData(15, 70), new HeckleData(15, 71), new HeckleData(15, 72), new HeckleData(15, 73), new HeckleData(15, 74),
            new HeckleData(15, 75), new HeckleData(15, 76), new HeckleData(15, 77), new HeckleData(15, 78), new HeckleData(15, 79),
            new HeckleData(15, 80), new HeckleData(15, 81), new HeckleData(15, 82), new HeckleData(15, 83), new HeckleData(15, 84),
            new HeckleData(15, 85), new HeckleData(15, 86), new HeckleData(15, 87), new HeckleData(15, 88), new HeckleData(15, 89),
            new HeckleData(15, 90), new HeckleData(15, 91), new HeckleData(15, 92), new HeckleData(15, 93), new HeckleData(15, 94),
            new HeckleData(15, 95), new HeckleData(15, 96), new HeckleData(15, 97), new HeckleData(15, 98), new HeckleData(15, 99),
            new HeckleData(15, 100), new HeckleData(15, 101), new HeckleData(15, 102), new HeckleData(15, 103), new HeckleData(15, 104),
            new HeckleData(15, 105), new HeckleData(15, 106), new HeckleData(15, 107), new HeckleData(15, 108), new HeckleData(15, 109),
            new HeckleData(15, 110), new HeckleData(15, 111), new HeckleData(15, 112), new HeckleData(15, 113), new HeckleData(15, 114),
            new HeckleData(15, 115), new HeckleData(15, 116), new HeckleData(15, 117), new HeckleData(15, 118), new HeckleData(15, 119),
            new HeckleData(15, 120), new HeckleData(15, 121), new HeckleData(15, 122), new HeckleData(15, 123), new HeckleData(15, 124),
            new HeckleData(15, 125), new HeckleData(15, 126), new HeckleData(15, 127), new HeckleData(15, 128), new HeckleData(15, 129),
            new HeckleData(15, 130), new HeckleData(15, 131), new HeckleData(15, 132), new HeckleData(15, 133), new HeckleData(15, 134),
            new HeckleData(15, 135), new HeckleData(15, 136), new HeckleData(15, 137), new HeckleData(15, 138), new HeckleData(15, 139),
            new HeckleData(15, 140), new HeckleData(15, 141), new HeckleData(15, 142), new HeckleData(15, 143), new HeckleData(15, 144),
            new HeckleData(15, 145), new HeckleData(15, 146), new HeckleData(15, 147), new HeckleData(15, 148), new HeckleData(15, 149),
            new HeckleData(15, 150), new HeckleData(15, 151), new HeckleData(15, 152), new HeckleData(15, 153), new HeckleData(15, 154),
            new HeckleData(15, 155), new HeckleData(15, 156), new HeckleData(15, 157), new HeckleData(15, 158), new HeckleData(15, 159),
            new HeckleData(15, 160), new HeckleData(15, 161), new HeckleData(15, 162), new HeckleData(15, 163), new HeckleData(15, 164),
            new HeckleData(15, 165), new HeckleData(15, 166), new HeckleData(15, 167), new HeckleData(15, 168), new HeckleData(15, 169),
            new HeckleData(15, 170), new HeckleData(15, 171), new HeckleData(15, 172), new HeckleData(15, 173), new HeckleData(15, 174),
            new HeckleData(15, 175), new HeckleData(15, 176), new HeckleData(15, 177), new HeckleData(15, 178), new HeckleData(15, 179),
            new HeckleData(15, 180), new HeckleData(15, 181), new HeckleData(15, 182), new HeckleData(15, 183), new HeckleData(15, 184),
            new HeckleData(15, 185), new HeckleData(15, 186), new HeckleData(15, 187), new HeckleData(15, 188), new HeckleData(15, 189),
            new HeckleData(15, 190), new HeckleData(15, 191), new HeckleData(15, 192), new HeckleData(15, 193), new HeckleData(15, 194),
            new HeckleData(15, 195), new HeckleData(15, 196), new HeckleData(15, 197), new HeckleData(15, 198), new HeckleData(15, 199),
            new HeckleData(15, 200), new HeckleData(15, 201), new HeckleData(15, 202), new HeckleData(15, 203), new HeckleData(15, 204),
            new HeckleData(15, 205), new HeckleData(15, 206), new HeckleData(15, 207), new HeckleData(15, 208), new HeckleData(15, 209),
            new HeckleData(15, 210), new HeckleData(15, 211), new HeckleData(15, 212), new HeckleData(15, 213), new HeckleData(15, 214),
            new HeckleData(15, 215), new HeckleData(15, 216), new HeckleData(15, 217), new HeckleData(15, 218), new HeckleData(15, 219),
            new HeckleData(15, 220), new HeckleData(15, 221), new HeckleData(15, 222), new HeckleData(15, 223), new HeckleData(15, 224),
            new HeckleData(15, 225), new HeckleData(15, 226), new HeckleData(15, 227), new HeckleData(15, 228), new HeckleData(15, 229),
            new HeckleData(15, 230), new HeckleData(15, 231), new HeckleData(15, 232), new HeckleData(15, 233), new HeckleData(15, 234),
            new HeckleData(15, 235), new HeckleData(15, 236), new HeckleData(15, 237), new HeckleData(15, 238), new HeckleData(15, 239),
            new HeckleData(15, 240), new HeckleData(15, 241), new HeckleData(15, 242), new HeckleData(15, 243), new HeckleData(15, 244),
            new HeckleData(15, 245), new HeckleData(15, 246), new HeckleData(15, 247), new HeckleData(15, 248), new HeckleData(15, 249),
            new HeckleData(15, 250), new HeckleData(15, 251), new HeckleData(15, 252), new HeckleData(15, 253), new HeckleData(15, 254),
            new HeckleData(15, 255), new HeckleData(15, 256), new HeckleData(15, 257), new HeckleData(15, 258), new HeckleData(15, 259),
            new HeckleData(15, 260), new HeckleData(15, 261), new HeckleData(15, 262), new HeckleData(15, 263), new HeckleData(15, 264),
            new HeckleData(15, 265), new HeckleData(15, 266), new HeckleData(15, 267), new HeckleData(15, 268), new HeckleData(15, 269),
            new HeckleData(15, 270), new HeckleData(15, 271), new HeckleData(15, 272), new HeckleData(15, 273), new HeckleData(15, 274),
            new HeckleData(15, 275), new HeckleData(15, 276), new HeckleData(15, 277), new HeckleData(15, 278), new HeckleData(15, 279),
        });
        public IReadOnlyList<CaptainData> Captains { get; } = new ReadOnlyCollection<CaptainData>(new CaptainData[]
        {
            new CaptainData(0, 0), new CaptainData(50, 1), new CaptainData(40, 2), new CaptainData(60, 3),
            new CaptainData(120, 4), new CaptainData(340, 5), new CaptainData(60, 6), new CaptainData(80, 7),
            new CaptainData(750, 8), new CaptainData(200, 9), new CaptainData(100, 10), new CaptainData(80, 11),
            new CaptainData(320, 12), new CaptainData(900, 13), new CaptainData(220, 14), new CaptainData(420, 15),
            new CaptainData(90, 16), new CaptainData(120, 17), new CaptainData(100, 18), new CaptainData(140, 19),
            new CaptainData(80, 20), new CaptainData(360, 21), new CaptainData(440, 22), new CaptainData(280, 23),
            new CaptainData(420, 24), new CaptainData(140, 25), new CaptainData(2732, 26), new CaptainData(180, 27),
            new CaptainData(150, 28), new CaptainData(200, 29), new CaptainData(30, 30), new CaptainData(2500, 31),
            new CaptainData(110, 32), new CaptainData(360, 33), new CaptainData(220, 34), new CaptainData(240, 35),
            new CaptainData(140, 36), new CaptainData(220, 37), new CaptainData(160, 38), new CaptainData(50, 39),
            new CaptainData(640, 40), new CaptainData(2000, 41), new CaptainData(2000, 42), new CaptainData(200, 43),
            new CaptainData(960, 44), new CaptainData(1400, 45), new CaptainData(777, 46), new CaptainData(777, 47),
            new CaptainData(777, 48), new CaptainData(777, 49), new CaptainData(560, 50)
        });
        public IReadOnlyList<BodykitData> Bodykits { get; } = new ReadOnlyCollection<BodykitData>(new BodykitData[]
        {
            new BodykitData(nameBase: "Bodykit000", descriptionBase : "BodykitDescription000", cost: 999999, id: 0),
            new BodykitData(nameBase: "Bodykit001", descriptionBase : "BodykitDescription001", cost: 480, id: 1),
            new BodykitData(nameBase: "Bodykit002", descriptionBase : "BodykitDescription002", cost: 1000, id: 2),
            new BodykitData(nameBase: "Bodykit003", descriptionBase : "BodykitDescription003", cost: 480, id: 3),
            new BodykitData(nameBase: "Bodykit004", descriptionBase : "BodykitDescription004", cost: 500, id: 4),
            new BodykitData(nameBase: "Bodykit005", descriptionBase : "BodykitDescription005", cost: 1250, id: 5),
            new BodykitData(nameBase: "Bodykit006", descriptionBase : "BodykitDescription006", cost: 300, id: 6),
            new BodykitData(nameBase: "Bodykit007", descriptionBase : "BodykitDescription007", cost: 450, id: 7),
            new BodykitData(nameBase: "Bodykit008", descriptionBase : "BodykitDescription008", cost: 1000, id: 8),
            new BodykitData(nameBase: "Bodykit009", descriptionBase : "BodykitDescription009", cost: 1000, id: 9),
            new BodykitData(nameBase: "Bodykit010", descriptionBase : "BodykitDescription010", cost: 800, id: 10),
            new BodykitData(nameBase: "Bodykit011", descriptionBase : "BodykitDescription011", cost: 1150, id: 11),
            new BodykitData(nameBase: "Bodykit012", descriptionBase : "BodykitDescription012", cost: 650, id: 12),
            new BodykitData(nameBase: "Bodykit013", descriptionBase : "BodykitDescription013", cost: 800, id: 13),
            new BodykitData(nameBase: "Bodykit014", descriptionBase : "BodykitDescription014", cost: 450, id: 14),
            new BodykitData(nameBase: "Bodykit015", descriptionBase : "BodykitDescription015", cost: 800, id: 15),
            new BodykitData(nameBase: "Bodykit016", descriptionBase : "BodykitDescription016", cost: 300, id: 16),
            new BodykitData(nameBase: "Bodykit017", descriptionBase : "BodykitDescription017", cost: 600, id: 17),
            new BodykitData(nameBase: "Bodykit018", descriptionBase : "BodykitDescription018", cost: 900, id: 18),
            new BodykitData(nameBase: "Bodykit019", descriptionBase : "BodykitDescription019", cost: 300, id: 19),
            new BodykitData(nameBase: "Bodykit020", descriptionBase : "BodykitDescription020", cost: 700, id: 20),
            new BodykitData(nameBase: "Bodykit021", descriptionBase : "BodykitDescription021", cost: 750, id: 21),
            new BodykitData(nameBase: "Bodykit022", descriptionBase : "BodykitDescription022", cost: 600, id: 22),
            new BodykitData(nameBase: "Bodykit023", descriptionBase : "BodykitDescription023", cost: 700, id: 23),
            new BodykitData(nameBase: "Bodykit024", descriptionBase : "BodykitDescription024", cost: 1150, id: 24),
            new BodykitData(nameBase: "Bodykit025", descriptionBase : "BodykitDescription025", cost: 1150, id: 25),
            new BodykitData(nameBase: "Bodykit026", descriptionBase : "BodykitDescription026", cost: 1100, id: 26),
            new BodykitData(nameBase: "Bodykit027", descriptionBase : "BodykitDescription027", cost: 1300, id: 27),
            new BodykitData(nameBase: "Bodykit028", descriptionBase : "BodykitDescription028", cost: 900, id: 28),
            new BodykitData(nameBase: "Bodykit029", descriptionBase : "BodykitDescription029", cost: 1100, id: 29),
            new BodykitData(nameBase: "Bodykit030", descriptionBase : "BodykitDescription030", cost: 450, id: 30),
            new BodykitData(nameBase: "Bodykit031", descriptionBase : "BodykitDescription031", cost: 500, id: 31),
            new BodykitData(nameBase: "Bodykit032", descriptionBase : "BodykitDescription032", cost: 1300, id: 32),
            new BodykitData(nameBase: "Bodykit033", descriptionBase : "BodykitDescription033", cost: 1100, id: 33),
            new BodykitData(nameBase: "Bodykit034", descriptionBase : "BodykitDescription034", cost: 1000, id: 34),
            new BodykitData(nameBase: "Bodykit035", descriptionBase : "BodykitDescription035", cost: 1050, id: 35),
            new BodykitData(nameBase: "Bodykit036", descriptionBase : "BodykitDescription036", cost: 1500, id: 36),
            new BodykitData(nameBase: "Bodykit037", descriptionBase : "BodykitDescription037", cost: 720, id: 37),
            new BodykitData(nameBase: "Bodykit038", descriptionBase : "BodykitDescription038", cost: 1040, id: 38),
            new BodykitData(nameBase: "Bodykit039", descriptionBase : "BodykitDescription039", cost: 960, id: 39),
            new BodykitData(nameBase: "Bodykit040", descriptionBase : "BodykitDescription040", cost: 1040, id: 40),
            new BodykitData(nameBase: "Bodykit041", descriptionBase : "BodykitDescription041", cost: 1040, id: 41),
            new BodykitData(nameBase: "Bodykit042", descriptionBase : "BodykitDescription042", cost: 1040, id: 42),
            new BodykitData(nameBase: "Bodykit043", descriptionBase : "BodykitDescription043", cost: 1040, id: 43),
            new BodykitData(nameBase: "Bodykit044", descriptionBase : "BodykitDescription044", cost: 960, id: 44),
            new BodykitData(nameBase: "Bodykit045", descriptionBase : "BodykitDescription045", cost: 960, id: 45),
            new BodykitData(nameBase: "Bodykit046", descriptionBase : "BodykitDescription046", cost: 850, id: 46),
            new BodykitData(nameBase: "Bodykit047", descriptionBase : "BodykitDescription047", cost: 1650, id: 47),
            new BodykitData(nameBase: "Bodykit048", descriptionBase : "BodykitDescription048", cost: 1120, id: 48),
            new BodykitData(nameBase: "Bodykit049", descriptionBase : "BodykitDescription049", cost: 1000, id: 49),
            new BodykitData(nameBase: "Bodykit050", descriptionBase : "BodykitDescription050", cost: 1400, id: 50),
            new BodykitData(nameBase: "Bodykit051", descriptionBase : "BodykitDescription051", cost: 1000, id: 50),
            new BodykitData(nameBase: "Bodykit052", descriptionBase : "BodykitDescription052", cost: 1200, id: 50),
        });
        public IReadOnlyList<VariantData> Variants { get; } = new ReadOnlyCollection<VariantData>(new VariantData[]
        {
            new VariantData(variantNameBase: "DoubleShot", coins: 0, credits: 662, id: 0), new VariantData(variantNameBase: "TripleShot", coins: 0, credits: 662, id: 1),
            new VariantData(variantNameBase: "QuickBuild", coins: 0, credits: 662, id: 2), new VariantData(variantNameBase: "QuickBuild", coins: 0, credits: 662, id: 3),
            new VariantData(variantNameBase: "RapidFire", coins: 0, credits: 662, id: 4), new VariantData(variantNameBase: "Robust", coins: 0, credits: 662, id: 5),
            new VariantData(variantNameBase: "Robust", coins: 0, credits: 662, id: 6), new VariantData(variantNameBase: "Refined", coins: 0, credits: 662, id: 7),
            new VariantData(variantNameBase: "RapidFire", coins: 0, credits: 662, id: 8), new VariantData(variantNameBase: "RapidFire", coins: 0, credits: 662, id: 9),
            new VariantData(variantNameBase: "Refined", coins: 0, credits: 662, id: 10), new VariantData(variantNameBase: "Damaging", coins: 0, credits: 662, id: 11),
            new VariantData(variantNameBase: "Robust", coins: 0, credits: 662, id: 12), new VariantData(variantNameBase: "RapidFire", coins: 0, credits: 662, id: 13),
            new VariantData(variantNameBase: "QuickBuild", coins: 0, credits: 662, id: 14), new VariantData(variantNameBase: "RapidFire", coins: 0, credits: 662, id: 15),
            new VariantData(variantNameBase: "RapidFire", coins: 0, credits: 662, id: 16), new VariantData(variantNameBase: "Damaging", coins: 0, credits: 662, id: 17),
            new VariantData(variantNameBase: "QuickBuild", coins: 0, credits: 662, id: 18), new VariantData(variantNameBase: "Damaging", coins: 0, credits: 662, id: 19),
            new VariantData(variantNameBase: "LongRange", coins: 0, credits: 662, id: 20), new VariantData(variantNameBase: "LongRange", coins: 0, credits: 662, id: 21),
            new VariantData(variantNameBase: "LongRange", coins: 0, credits: 662, id: 22), new VariantData(variantNameBase: "Damaging", coins: 0, credits: 662, id: 23),
            new VariantData(variantNameBase: "Damaging", coins: 0, credits: 662, id: 24), new VariantData(variantNameBase: "Refined", coins: 0, credits: 662, id: 25),
            new VariantData(variantNameBase: "Damaging", coins: 0, credits: 662, id: 26), new VariantData(variantNameBase: "QuickBuild", coins: 0, credits: 662, id: 27),
            new VariantData(variantNameBase: "Refined", coins: 0, credits: 662, id: 28), new VariantData(variantNameBase: "Refined", coins: 0, credits: 662, id: 29),
            new VariantData(variantNameBase: "Refined", coins: 0, credits: 662, id: 30), new VariantData(variantNameBase: "Damaging", coins: 0, credits: 662, id: 31),
            new VariantData(variantNameBase: "Damaging", coins: 0, credits: 662, id: 32), new VariantData(variantNameBase: "LongRange", coins: 0, credits: 662, id: 33),
            new VariantData(variantNameBase: "RapidFire", coins: 0, credits: 662, id: 34), new VariantData(variantNameBase: "Damaging", coins: 0, credits: 662, id: 35),
            new VariantData(variantNameBase: "DoubleShot", coins: 0, credits: 662, id: 36), new VariantData(variantNameBase: "TripleShot", coins: 0, credits: 662, id: 37),
            new VariantData(variantNameBase: "RapidFire", coins: 0, credits: 662, id: 38), new VariantData(variantNameBase: "Sniper", coins: 0, credits: 662, id: 39),
            new VariantData(variantNameBase: "LongRange", coins: 0, credits: 662, id: 40), new VariantData(variantNameBase: "QuickBuild", coins: 0, credits: 662, id: 41),
            new VariantData(variantNameBase: "Robust", coins: 0, credits: 662, id: 42), new VariantData(variantNameBase: "RapidFire", coins: 0, credits: 662, id: 43),
            new VariantData(variantNameBase: "Sniper", coins: 0, credits: 662, id: 44), new VariantData(variantNameBase: "LongRange", coins: 0, credits: 662, id: 45),
            new VariantData(variantNameBase: "Sniper", coins: 0, credits: 662, id: 46), new VariantData(variantNameBase: "TripleShot", coins: 0, credits: 662, id: 47),
            new VariantData(variantNameBase: "DoubleShot", coins: 0, credits: 662, id: 48), new VariantData(variantNameBase: "QuickBuild", coins: 0, credits: 662, id: 49),
            new VariantData(variantNameBase: "Robust", coins: 0, credits: 662, id: 50), new VariantData(variantNameBase: "Damaging", coins: 0, credits: 662, id: 51),
            new VariantData(variantNameBase: "Refined", coins: 0, credits: 662, id: 52), new VariantData(variantNameBase: "Refined", coins: 0, credits: 662, id: 53),
            new VariantData(variantNameBase: "Damaging", coins: 0, credits: 662, id: 54), new VariantData(variantNameBase: "Robust", coins: 0, credits: 662, id: 55),
            new VariantData(variantNameBase: "Damaging", coins: 0, credits: 662, id: 56), new VariantData(variantNameBase: "Refined", coins: 0, credits: 662, id: 57),
            new VariantData(variantNameBase: "Sniper", coins: 0, credits: 662, id: 58), new VariantData(variantNameBase: "TripleShot", coins: 0, credits: 662, id: 59),
            new VariantData(variantNameBase: "DoubleShot", coins: 0, credits: 662, id: 60), new VariantData(variantNameBase: "QuickBuild", coins: 0, credits: 662, id: 61),
            new VariantData(variantNameBase: "Robust", coins: 0, credits: 662, id: 62), new VariantData(variantNameBase: "RapidFire", coins: 0, credits: 662, id: 63),
            new VariantData(variantNameBase: "LongRange", coins: 0, credits: 662, id: 64), new VariantData(variantNameBase: "Refined", coins: 0, credits: 662, id: 65),
            new VariantData(variantNameBase: "Sniper", coins: 0, credits: 662, id: 66), new VariantData(variantNameBase: "RapidFire", coins: 0, credits: 662, id: 67),
            new VariantData(variantNameBase: "Robust", coins: 0, credits: 662, id: 68), new VariantData(variantNameBase: "Refined", coins: 0, credits: 662, id: 69),
            new VariantData(variantNameBase: "Sniper", coins: 0, credits: 662, id: 70), new VariantData(variantNameBase: "TripleShot", coins: 0, credits: 662, id: 71),
            new VariantData(variantNameBase: "DoubleShot", coins: 0, credits: 662, id: 72), new VariantData(variantNameBase: "QuickBuild", coins: 0, credits: 662, id: 73),
            new VariantData(variantNameBase: "RapidFire", coins: 0, credits: 662, id: 74), new VariantData(variantNameBase: "QuickBuild", coins: 0, credits: 662, id: 75),
            new VariantData(variantNameBase: "Robust", coins: 0, credits: 662, id: 76), new VariantData(variantNameBase: "Sniper", coins: 0, credits: 662, id: 77),
            new VariantData(variantNameBase: "TripleShot", coins: 0, credits: 662, id: 78), new VariantData(variantNameBase: "DoubleShot", coins: 0, credits: 662, id: 79),
            new VariantData(variantNameBase: "Robust", coins: 0, credits: 662, id: 80), new VariantData(variantNameBase: "Refined", coins: 0, credits: 662, id: 81),
            new VariantData(variantNameBase: "Sniper", coins: 0, credits: 662, id: 82), new VariantData(variantNameBase: "RapidFire", coins: 0, credits: 662, id: 83),
            new VariantData(variantNameBase: "LongRange", coins: 0, credits: 662, id: 84), new VariantData(variantNameBase: "Damaging", coins: 0, credits: 662, id: 85),
            new VariantData(variantNameBase: "Sniper", coins: 0, credits: 662, id: 86), new VariantData(variantNameBase: "QuickBuild", coins: 0, credits: 662, id: 87),
            new VariantData(variantNameBase: "Robust", coins: 0, credits: 662, id: 88), new VariantData(variantNameBase: "LongRange", coins: 0, credits: 662, id: 89),
            new VariantData(variantNameBase: "Refined", coins: 0, credits: 662, id: 90), new VariantData(variantNameBase: "Sniper", coins: 0, credits: 662, id: 91),
            new VariantData(variantNameBase: "TripleShot", coins: 0, credits: 662, id: 92), new VariantData(variantNameBase: "DoubleShot", coins: 0, credits: 662, id: 93),
            new VariantData(variantNameBase: "QuickBuild", coins: 0, credits: 662, id: 94), new VariantData(variantNameBase: "Robust", coins: 0, credits: 662, id: 95),
            new VariantData(variantNameBase: "Damaging", coins: 0, credits: 662, id: 96), new VariantData(variantNameBase: "Refined", coins: 0, credits: 662, id: 97),
            new VariantData(variantNameBase: "Sniper", coins: 0, credits: 662, id: 98), new VariantData(variantNameBase: "RapidFire", coins: 0, credits: 662, id: 99),
            new VariantData(variantNameBase: "Robust", coins: 0, credits: 662, id: 100), new VariantData(variantNameBase: "Refined", coins: 0, credits: 662, id: 101),
            new VariantData(variantNameBase: "Damaging", coins: 0, credits: 662, id: 102), new VariantData(variantNameBase: "Sniper", coins: 0, credits: 662, id: 103),
            new VariantData(variantNameBase: "RapidFire", coins: 0, credits: 662, id: 104), new VariantData(variantNameBase: "Damaging", coins: 0, credits: 662, id: 105),
            new VariantData(variantNameBase: "Refined", coins: 0, credits: 662, id: 106), new VariantData(variantNameBase: "LongRange", coins: 0, credits: 662, id: 107),
            new VariantData(variantNameBase: "Damaging", coins: 0, credits: 662, id: 108), new VariantData(variantNameBase: "Sniper", coins: 0, credits: 662, id: 109),
            new VariantData(variantNameBase: "TripleShot", coins: 0, credits: 662, id: 110), new VariantData(variantNameBase: "DoubleShot", coins: 0, credits: 662, id: 111),
            new VariantData(variantNameBase: "QuickBuild", coins: 0, credits: 662, id: 112), new VariantData(variantNameBase: "Robust", coins: 0, credits: 662, id: 113),
            new VariantData(variantNameBase: "RapidFire", coins: 0, credits: 662, id: 114), new VariantData(variantNameBase: "Damaging", coins: 0, credits: 662, id: 115),
            new VariantData(variantNameBase: "Refined", coins: 0, credits: 662, id: 116), new VariantData(variantNameBase: "Sniper", coins: 0, credits: 662, id: 117),
            new VariantData(variantNameBase: "Robust", coins: 0, credits: 662, id: 118), new VariantData(variantNameBase: "RapidFire", coins: 0, credits: 662, id: 119),
            new VariantData(variantNameBase: "DoubleShot", coins: 0, credits: 662, id: 120), new VariantData(variantNameBase: "Refined", coins: 0, credits: 662, id: 121),
            new VariantData(variantNameBase: "LongRange", coins: 0, credits: 662, id: 122), new VariantData(variantNameBase: "TripleShot", coins: 0, credits: 662, id: 123),
            new VariantData(variantNameBase: "DoubleShot", coins: 0, credits: 662, id: 124), new VariantData(variantNameBase: "QuickBuild", coins: 0, credits: 662, id: 125),
            new VariantData(variantNameBase: "RapidFire", coins: 0, credits: 662, id: 126), new VariantData(variantNameBase: "Refined", coins: 0, credits: 662, id: 127),
            new VariantData(variantNameBase: "Sniper", coins: 0, credits: 662, id: 128), new VariantData(variantNameBase: "Robust", coins: 0, credits: 662, id: 129),
            new VariantData(variantNameBase: "Damaging", coins: 0, credits: 662, id: 130)
        });
        public IReadOnlyList<IAPData> IAPs { get; } = new ReadOnlyCollection<IAPData>(new IAPData[]
        {
            new IAPData(0, 0.99f, 100),
            new IAPData(0, 1.99f, 500),
            new IAPData(0, 2.99f, 1000),
            new IAPData(0, 3.99f, 5000)
        });
        public List<Arena> Arenas { get; set; } = new List<Arena>
        {
                new Arena(),
                new Arena("PracticeWreckyards", prizecredits: 100),
                new Arena("OzPenitentiary", prizecoins:1),
                new Arena("SanFranciscoFightClub", costcoins:1, prizecoins:3, prizecredits: 500),
                new Arena("UACBattleNight", costcredits:100, prizecredits: 500),
                new Arena("NuclearDome", costcoins:3, prizecoins:4,prizecredits:400, prizenukes: 1, consolationnukes: 1),
                new Arena("UACArena", costcredits:1500, prizecredits: 400),
                new Arena("RioBattlesport", costcoins:10, prizecoins:15, prizecredits:2000, consolationcredits:2000),
                new Arena("UACUltimate", costcoins: 10000, prizecredits:20000),
                new Arena("MercenaryOne", costcoins:50, prizecredits:50000, prizenukes: 1)
        };
        public Dictionary<string, int> GameConfigs { get; set; } = new Dictionary<string, int>
        {
            { "scoredivider", 10 },
            { "creditdivider", 100 },
            { "coin1threshold", 1000 },
            { "coin2threshold", 2000 },
            { "coin3threshold", 3000 },
            { "coin4threshold", 4000 },
            { "coin5threshold", 5000 },
            { "creditmax", 1250 }
        };
        public int MinCPUCores { get; set; }
        public int MinCPUFrequency { get; set; }
        public int MaxLatency { get; set; }
        public int LastLevelWithLoot => 40;
        public ILevelStrategies Strategies { get; }
        public ILevelStrategies SideQuestStrategies { get; }
        public IPvPLevelStrategies PvPStrategies { get; }

        public StaticData()
        {
            Strategies = new LevelStrategies();
            SideQuestStrategies = new SideQuestStrategies();
            PvPStrategies = new PvPLevelStrategies();

            InitialGameModel = CreateInitialGameModel();
            //Assert.IsTrue(Levels.Count == NUM_OF_LEVELS,
            //"NUM_OF_LEVELS does not match Levels.Count: " + NUM_OF_LEVELS.ToString() + " | expected: " + Levels.Count);
            Assert.IsTrue(SideQuests.Count == NUM_OF_SIDEQUESTS,
            "NUM_OF_SIDEQUESTS does not match SideQuests.Count: " + NUM_OF_SIDEQUESTS.ToString() + " | expected: " + SideQuests.Count);
        }

        /// <summary>
        /// Creates the initial game model.
        /// 
        /// NOTE:  Do NOT share key list objects between Loadout and GameModel, otherwise
        /// both will share the same list.  In that case if the Loadout deletes one of its
        /// buildings the building will also be deleted from the GameModel.
        /// </summary>
        private GameModel CreateInitialGameModel()
        {
            // TEMP  For final game, don't add ALL the prefabs :D
            //Loadout playerLoadout = new Loadout(initialHull, AllBuildingKeys(), AllUnitKeys());

            Loadout playerLoadout = new Loadout(InitialHull, GetInitialBuildings(), GetInitialUnits(), GetInitialbuildingLimit(), GetInitialUnitLimit());

            bool hasAttemptedTutorial = false;
            bool HasSyncdShop = false;

            GameModel game = new GameModel(
                HasSyncdShop,
                hasAttemptedTutorial,
                0,
                0,
                playerLoadout,
                lastBattleResult: null,
                // TEMP  Do not unlock all hulls & buildables at the game start :P
                unlockedHulls: new List<HullKey>() { InitialHull },
                unlockedBuildings: GetInitialBuildings(),
                unlockedUnits: GetInitialUnits()
                );

            foreach (int i in playerLoadout.CurrentHeckles)
                game.AddHeckle(i);

            return game;
            //unlockedHulls: AllHullKeys(),
            //unlockedBuildings: AllBuildingKeys(),
            //unlockedUnits: AllUnitKeys());
        }

        private Dictionary<BuildingCategory, List<BuildingKey>> GetInitialbuildingLimit()
        {
            List<BuildingKey> limit = GetInitialBuildings();
            List<BuildingKey> factories = new List<BuildingKey>();
            List<BuildingKey> defence = new List<BuildingKey>();
            List<BuildingKey> offense = new List<BuildingKey>();
            List<BuildingKey> tactical = new List<BuildingKey>();
            List<BuildingKey> Ultra = new List<BuildingKey>();
            foreach (BuildingKey key in limit)
            {
                switch (key.BuildingCategory)
                {
                    case BuildingCategory.Factory:
                        factories.Add(key);
                        break;
                    case BuildingCategory.Defence:
                        defence.Add(key);
                        break;
                    case BuildingCategory.Offence:
                        offense.Add(key);
                        break;
                    case BuildingCategory.Tactical:
                        tactical.Add(key);
                        break;
                    case BuildingCategory.Ultra:
                        Ultra.Add(key);
                        break;
                    default:
                        break;
                }
            }
            Dictionary<BuildingCategory, List<BuildingKey>> buildables = new()
            {
                { BuildingCategory.Factory, factories },
                { BuildingCategory.Defence, defence },
                { BuildingCategory.Offence, offense },
                { BuildingCategory.Tactical, tactical },
                { BuildingCategory.Ultra, Ultra }
            };
            return buildables;
        }

        private Dictionary<UnitCategory, List<UnitKey>> GetInitialUnitLimit()
        {
            List<UnitKey> units = GetInitialUnits();
            List<UnitKey> ships = new();
            List<UnitKey> aircraft = new();
            foreach (UnitKey unit in units)
            {
                if (unit.UnitCategory == UnitCategory.Naval)
                    ships.Add(unit);
                else if (unit.UnitCategory == UnitCategory.Aircraft)
                    aircraft.Add(unit);
                else
                    break;
            }
            Dictionary<UnitCategory, List<UnitKey>> unitlimit = new()
            {
                {UnitCategory.Naval, ships },
                {UnitCategory.Aircraft, aircraft }
            };
            return unitlimit;
        }
        private readonly HullKey InitialHull = Hulls.Trident;

        private List<BuildingKey> GetInitialBuildings()
        {
            return GetBuildingsUnlockedInLevel(levelFirstAvailableIn: 1).ToList();
        }

        private List<UnitKey> GetInitialUnits()
        {
            return GetUnitsUnlockedInLevel(levelFirstAvailableIn: 1).ToList();
        }

        /// <summary>
        /// Availability level number:  The first level that prefab is available.
        /// Loot level number (level completed):  The level that unlocks the prefab when you complete
        /// it successfully.
        /// 
        /// Availability level number = loot level number + 1
        /// </summary>

        public ILoot GetLevelLoot(int levelCompleted)
        {
            int availabilityLevelNum = levelCompleted + 1;

            Assert.IsTrue(availabilityLevelNum >= MIN_AVAILABILITY_LEVEL_NUM);
            Assert.IsTrue(availabilityLevelNum <= Levels.Count + 1);

            return
                new Loot(
                    hullKeys: GetHullsUnlockedInLevel(availabilityLevelNum),
                    unitKeys: GetUnitsUnlockedInLevel(availabilityLevelNum),
                    buildingKeys: GetBuildingsUnlockedInLevel(availabilityLevelNum));
        }

        public ILoot GetSideQuestLoot(int sideQuestID)
        {
            int availabilitySideQuestNum = sideQuestID;
            //hardcoded values while testing
            Assert.IsTrue(availabilitySideQuestNum >= 0);
            Assert.IsTrue(availabilitySideQuestNum <= NUM_OF_SIDEQUESTS);

            return
                new Loot(
                    hullKeys: GetHullsUnlockedInSideQuest(availabilitySideQuestNum),
                    unitKeys: GetUnitsUnlockedInSideQuest(availabilitySideQuestNum),
                    buildingKeys: GetBuildingsUnlockedInSideQuest(availabilitySideQuestNum));
        }

        private IList<UnitKey> GetUnitsUnlockedInLevel(int levelFirstAvailableIn)
        {
            return GetBuildablesUnlockedInLevel(_unitToUnlockedLevel, levelFirstAvailableIn);
        }

        private IList<BuildingKey> GetBuildingsUnlockedInLevel(int levelFirstAvailableIn)
        {
            return GetBuildablesUnlockedInLevel(_buildingToUnlockedLevel, levelFirstAvailableIn);
        }

        /// <summary>
        /// List should always have 0 or 1 entry, unless levelFirstAvailableIn is 1
        /// (ie, the starting level, where we have multiple buildables available).
        /// </summary>
        private IList<TKey> GetBuildablesUnlockedInLevel<TKey>(IDictionary<TKey, int> buildableToUnlockedLevel, int levelFirstAvailableIn)
            where TKey : IPrefabKey
        {
            return
                buildableToUnlockedLevel
                    .Where(buildableToLevel => buildableToLevel.Value == levelFirstAvailableIn)
                    .Select(buildableToLevel => buildableToLevel.Key)
                    .ToList();
        }

        private IList<HullKey> GetHullsUnlockedInLevel(int levelFirstAvailableIn)
        {
            return
                _hullToUnlockedLevel
                    .Where(hullToLevel => hullToLevel.Value == levelFirstAvailableIn)
                    .Select(hullToLevel => hullToLevel.Key)
                    .ToList();
        }

        public int UnitUnlockLevel(UnitKey unitKey)
        {
            Assert.IsTrue(_unitToUnlockedLevel.ContainsKey(unitKey));
            return _unitToUnlockedLevel[unitKey];
        }

        public int BuildingUnlockLevel(BuildingKey buildingKey)
        {
            //Assert.IsTrue(_buildingToUnlockedLevel.ContainsKey(buildingKey));
            return _buildingToUnlockedLevel[buildingKey];
        }

        private IList<UnitKey> GetUnitsUnlockedInSideQuest(int requiredSideQuestID)
        {
            return GetBuildablesUnlockedInSideQuest(_unitToCompletedSideQuest, requiredSideQuestID);
        }

        private IList<BuildingKey> GetBuildingsUnlockedInSideQuest(int requiredSideQuestID)
        {
            return GetBuildablesUnlockedInSideQuest(_buildingToCompletedSideQuest, requiredSideQuestID);
        }

        private IList<TKey> GetBuildablesUnlockedInSideQuest<TKey>(IDictionary<TKey, int> buildableToCompletedSideQuest, int requiredSideQuestID)
            where TKey : IPrefabKey
        {
            return
                buildableToCompletedSideQuest
                    .Where(buildableToSideQuest => buildableToSideQuest.Value == requiredSideQuestID)
                    .Select(buildableToSideQuest => buildableToSideQuest.Key)
                    .ToList();
        }

        private IList<HullKey> GetHullsUnlockedInSideQuest(int requiredSideQuestID)
        {
            return
                _hullToCompletedSideQuest
                    .Where(hullToSideQuest => hullToSideQuest.Value == requiredSideQuestID)
                    .Select(hullToSideQuest => hullToSideQuest.Key)
                    .ToList();
        }

        public int UnitUnlockSideQuest(UnitKey unitKey)
        {
            Assert.IsTrue(_unitToCompletedSideQuest.ContainsKey(unitKey));
            return _unitToCompletedSideQuest[unitKey];
        }

        public int BuildingSideQuest(BuildingKey buildingKey)
        {
            Assert.IsTrue(_buildingToCompletedSideQuest.ContainsKey(buildingKey));
            return _buildingToCompletedSideQuest[buildingKey];
        }
    }
}
