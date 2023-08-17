using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using System.Collections.Generic;

namespace BattleCruisers.Data.Static
{
    public static class StaticPrefabKeys
    {
        public static class Buildings
        {
            // Factories
            public static BuildingKey AirFactory { get; } = new BuildingKey(BuildingCategory.Factory, "AirFactory");
            public static BuildingKey NavalFactory { get; } = new BuildingKey(BuildingCategory.Factory, "NavalFactory");
            public static BuildingKey DroneStation { get; } = new BuildingKey(BuildingCategory.Factory, "EngineeringBay");
            public static BuildingKey DroneStation4 { get; } = new BuildingKey(BuildingCategory.Factory, "EngineeringBay4");
            public static BuildingKey DroneStation8 { get; } = new BuildingKey(BuildingCategory.Factory, "EngineeringBay8");

            // Tactical
            public static BuildingKey ShieldGenerator { get; } = new BuildingKey(BuildingCategory.Tactical, "ShieldGenerator");
            public static BuildingKey StealthGenerator { get; } = new BuildingKey(BuildingCategory.Tactical, "StealthGenerator");
            public static BuildingKey SpySatelliteLauncher { get; } = new BuildingKey(BuildingCategory.Tactical, "SpySatelliteLauncher");
            public static BuildingKey LocalBooster { get; } = new BuildingKey(BuildingCategory.Tactical, "LocalBooster");
            public static BuildingKey ControlTower { get; } = new BuildingKey(BuildingCategory.Tactical, "ControlTower");

            // Defence
            public static BuildingKey AntiShipTurret { get; } = new BuildingKey(BuildingCategory.Defence, "AntiShipTurret");
            public static BuildingKey AntiAirTurret { get; } = new BuildingKey(BuildingCategory.Defence, "AntiAirTurret");
            public static BuildingKey Mortar { get; } = new BuildingKey(BuildingCategory.Defence, "Mortar");
            public static BuildingKey SamSite { get; } = new BuildingKey(BuildingCategory.Defence, "SamSite");
            public static BuildingKey TeslaCoil { get; } = new BuildingKey(BuildingCategory.Defence, "TeslaCoil");
            public static BuildingKey Coastguard { get; } = new BuildingKey(BuildingCategory.Defence, "Coastguard");//new

            // Offence
            public static BuildingKey Artillery { get; } = new BuildingKey(BuildingCategory.Offence, "Artillery");
            public static BuildingKey RocketLauncher { get; } = new BuildingKey(BuildingCategory.Offence, "RocketLauncher");
            public static BuildingKey Railgun { get; } = new BuildingKey(BuildingCategory.Offence, "Railgun");
            public static BuildingKey MLRS { get; } = new BuildingKey(BuildingCategory.Offence, "MLRS");
            public static BuildingKey GatlingMortar { get; } = new BuildingKey(BuildingCategory.Offence, "GatlingMortar");
            public static BuildingKey IonCannon { get; } = new BuildingKey(BuildingCategory.Offence, "IonCannon");//new
            public static BuildingKey MissilePod { get; } = new BuildingKey(BuildingCategory.Offence, "MissilePod");//new


            // Ultras
            public static BuildingKey DeathstarLauncher { get; } = new BuildingKey(BuildingCategory.Ultra, "DeathstarLauncher");
            public static BuildingKey NukeLauncher { get; } = new BuildingKey(BuildingCategory.Ultra, "NukeLauncher");
            public static BuildingKey Ultralisk { get; } = new BuildingKey(BuildingCategory.Ultra, "Ultralisk");
            public static BuildingKey KamikazeSignal { get; } = new BuildingKey(BuildingCategory.Ultra, "KamikazeSignal");
            public static BuildingKey Broadsides { get; } = new BuildingKey(BuildingCategory.Ultra, "Broadsides");
            public static BuildingKey NovaArtillery { get; } = new BuildingKey(BuildingCategory.Ultra, "NovaArtillery");//new

            public static IList<IPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPrefabKey>()
                    {
                        // Factories
                        AirFactory, NavalFactory, DroneStation, DroneStation4, DroneStation8,
                        // Tactical
                        ShieldGenerator, StealthGenerator, SpySatelliteLauncher, LocalBooster, ControlTower,
                        // Defence
                        AntiShipTurret, AntiAirTurret, Mortar, SamSite, TeslaCoil, Coastguard,
                        // Offence
                        Artillery, RocketLauncher, Railgun, MLRS, GatlingMortar, MissilePod, IonCannon, //railgun = LasCannon! 
                        // Ultras
                        DeathstarLauncher, NukeLauncher, Ultralisk, KamikazeSignal, Broadsides, NovaArtillery,
                    };
                }
            }
        }


        public static class Units
        {
            // Aircraft
            public static UnitKey Bomber { get; } = new UnitKey(UnitCategory.Aircraft, "Bomber");
            public static UnitKey Fighter { get; } = new UnitKey(UnitCategory.Aircraft, "Fighter");
            public static UnitKey Gunship { get; } = new UnitKey(UnitCategory.Aircraft, "Gunship");
            public static UnitKey SteamCopter { get; } = new UnitKey(UnitCategory.Aircraft, "SteamCopter");
            public static UnitKey Broadsword { get; } = new UnitKey(UnitCategory.Aircraft, "Broadsword");
            public static UnitKey TestAircraft { get; } = new UnitKey(UnitCategory.Aircraft, "TestAircraft");

            // Ships
            public static UnitKey AttackBoat { get; } = new UnitKey(UnitCategory.Naval, "AttackBoat");
            public static UnitKey AttackRIB { get; } = new UnitKey(UnitCategory.Naval, "AttackRIB");
            public static UnitKey Frigate { get; } = new UnitKey(UnitCategory.Naval, "Frigate");
            public static UnitKey Destroyer { get; } = new UnitKey(UnitCategory.Naval, "Destroyer");
            public static UnitKey ArchonBattleship { get; } = new UnitKey(UnitCategory.Naval, "ArchonBattleship");

            public static IList<IPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPrefabKey>()
                    {
                        // Aircraft
                        Bomber, Fighter, Gunship, SteamCopter, Broadsword, TestAircraft,
                        // Ships
                        AttackBoat, AttackRIB, Frigate, Destroyer, ArchonBattleship
                    };
                }
            }
        }

        public static class Hulls
        {
            public static HullKey Bullshark { get; } = new HullKey("Bullshark");
            public static HullKey Eagle { get; } = new HullKey("Eagle");
            public static HullKey Hammerhead { get; } = new HullKey("Hammerhead");
            public static HullKey Longbow { get; } = new HullKey("Longbow");
            public static HullKey Megalodon { get; } = new HullKey("Megalodon");
            public static HullKey Raptor { get; } = new HullKey("Raptor");
            public static HullKey Rockjaw { get; } = new HullKey("Rockjaw");
            public static HullKey Trident { get; } = new HullKey("Trident");
            public static HullKey ManOfWarBoss { get; } = new HullKey("ManOfWarBoss");
            public static HullKey HuntressBoss { get; } = new HullKey("HuntressBoss");
            public static HullKey TasDevil { get; } = new HullKey("TasDevil");
            public static HullKey Yeti { get; } = new HullKey("Yeti");
            public static HullKey Rickshaw { get; } = new HullKey("Rickshaw");
            public static HullKey BlackRig { get; } = new HullKey("BlackRig");



            public static IList<IPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPrefabKey>()
                    {
                        Bullshark, Eagle, Hammerhead, Longbow, Megalodon, Raptor, Rockjaw, Trident, ManOfWarBoss, HuntressBoss, BlackRig, Yeti, Rickshaw, TasDevil
                    };
                }
            }

            public static IList<HullKey> AllKeysExplicit
            {
                get
                {
                    return new List<HullKey>()
                    {
                        Bullshark, Eagle, Hammerhead, Longbow, Megalodon, Raptor, Rockjaw, Trident, BlackRig, Yeti, Rickshaw, TasDevil
                    };
                }
            }
        }

        public static class Ranks
        {
            // Ranks
            public static RankData rank00 { get; } = new RankData("Rank00BoatThief", "00", "Rank0");
            public static RankData rank01 { get; } = new RankData("Rank01Wrecker", "01", "Rank1");
            public static RankData rank02 { get; } = new RankData("Rank02Saboteur", "002", "Rank2");
            public static RankData rank03 { get; } = new RankData("Rank03Pirate", "03", "Rank3");
            public static RankData rank04 { get; } = new RankData("Rank04Recruit", "04", "Rank4");
            public static RankData rank05 { get; } = new RankData("Rank05Apprentice", "05", "Rank5");
            public static RankData rank06 { get; } = new RankData("Rank06Seabot", "06", "Rank6");
            public static RankData rank07 { get; } = new RankData("Rank07Private", "07", "Rank7");
            public static RankData rank08 { get; } = new RankData("Rank08LanceCorporal", "08", "Rank8");
            public static RankData rank09 { get; } = new RankData("Rank09Corporal", "09", "Rank9");
            public static RankData rank10 { get; } = new RankData("Rank10Sergeant", "10", "Rank10");
            public static RankData rank11 { get; } = new RankData("Rank11StaffSergeant", "11", "Rank11");
            public static RankData rank12 { get; } = new RankData("Rank12GunnerySergeant", "12", "Rank12");
            public static RankData rank13 { get; } = new RankData("Rank13FirstSergeant", "13", "Rank13");
            public static RankData rank14 { get; } = new RankData("Rank14MasterSergeant", "14", "Rank14");
            public static RankData rank15 { get; } = new RankData("Rank15SergeantMajor", "15", "Rank15");
            public static RankData rank16 { get; } = new RankData("Rank16WarrantOfficer", "16", "Rank16");
            public static RankData rank17 { get; } = new RankData("Rank17ChiefOfficer", "17", "Rank17");
            public static RankData rank18 { get; } = new RankData("Rank18SeniorChief", "18", "Rank18");
            public static RankData rank19 { get; } = new RankData("Rank19MasterChief", "19", "Rank19");
            public static RankData rank20 { get; } = new RankData("Rank20Colonel", "20", "Rank20");
            public static RankData rank21 { get; } = new RankData("Rank21Lieutennant", "21", "Rank21");
            public static RankData rank22 { get; } = new RankData("Rank22ExecutiveOfficer", "22", "Rank22");
            public static RankData rank23 { get; } = new RankData("Rank23Commander", "23", "Rank23");
            public static RankData rank24 { get; } = new RankData("Rank24Captain", "24", "Rank24");
            public static RankData rank25 { get; } = new RankData("Rank25RearAdmiral", "25", "Rank25");
            public static RankData rank26 { get; } = new RankData("Rank26ViceAdmiral", "26", "Rank26");
            public static RankData rank27 { get; } = new RankData("Rank27Admiral", "27", "Rank27");
            public static RankData rank28 { get; } = new RankData("Rank28FleetAdmiral", "28", "Rank28");
            public static RankData rank29 { get; } = new RankData("Rank29MajorAdmiral", "29", "Rank29");
            public static RankData rank30 { get; } = new RankData("Rank30Admiral3Star", "30", "Rank30");
            public static RankData rank31 { get; } = new RankData("Rank31Admiral4Star", "31", "Rank31");
            public static RankData rank32 { get; } = new RankData("Rank32Admiral5Star", "32", "Rank32");
            public static RankData rank33 { get; } = new RankData("Rank33SupremeCommander", "33", "Rank33");
            public static IList<IRankData> AllRanks
            {
                get
                {
                    return new List<IRankData>()
                    {
                        rank00, rank01, rank02, rank03, rank04, rank05, rank06, rank07, rank08, rank09,
                        rank10, rank11, rank12, rank13, rank14, rank15, rank16, rank17, rank18, rank19,
                        rank20, rank21, rank22, rank23, rank24, rank25, rank26, rank27, rank28, rank29,
                        rank30, rank31, rank32, rank33
                    };
                }
            }
        }

        public static Dictionary<string, int> CaptainItems = new Dictionary<string, int>
        {
            { "CAPTAINEXO000", 0},{ "CAPTAINEXO001", 1},{ "CAPTAINEXO002", 2},{ "CAPTAINEXO003", 3},{ "CAPTAINEXO004", 4},{ "CAPTAINEXO005", 5},{ "CAPTAINEXO006", 6},{ "CAPTAINEXO007", 7},{ "CAPTAINEXO008", 8},{ "CAPTAINEXO009", 9},
            { "CAPTAINEXO010", 10},{ "CAPTAINEXO011", 11},{ "CAPTAINEXO012", 12},{ "CAPTAINEXO013", 13}, { "CAPTAINEXO014", 14},{ "CAPTAINEXO015", 15}, { "CAPTAINEXO016", 16}, { "CAPTAINEXO017", 17},{ "CAPTAINEXO018", 18},{ "CAPTAINEXO019", 19},
            { "CAPTAINEXO020", 20},{ "CAPTAINEXO021", 21}, { "CAPTAINEXO022", 22}, { "CAPTAINEXO023", 23}, { "CAPTAINEXO024", 24},  { "CAPTAINEXO025", 25}, { "CAPTAINEXO026", 26}, { "CAPTAINEXO027", 27}, { "CAPTAINEXO028", 28},{ "CAPTAINEXO029", 29},
            { "CAPTAINEXO030", 30},{ "CAPTAINEXO031", 31},{ "CAPTAINEXO032", 32},{ "CAPTAINEXO033", 33},{ "CAPTAINEXO034", 34},{ "CAPTAINEXO035", 35},{ "CAPTAINEXO036", 36},{ "CAPTAINEXO037", 37},{ "CAPTAINEXO038", 38},{ "CAPTAINEXO039", 39},{ "CAPTAINEXO040", 40},
        };

        public static Dictionary<string, int> HeckleItems = new Dictionary<string, int>
        {
            { "HECKLE000", 0},
            { "HECKLE001", 1},
            { "HECKLE002", 2},
            { "HECKLE003", 3},
            { "HECKLE004", 4},
            { "HECKLE005", 5},
            { "HECKLE006", 6},
            { "HECKLE007", 7},
            { "HECKLE008", 8},
            { "HECKLE009", 9},
            { "HECKLE010", 10},
            { "HECKLE011", 11},
            { "HECKLE012", 12},
            { "HECKLE013", 13},
            { "HECKLE014", 14},
            { "HECKLE015", 15},
            { "HECKLE016", 16},
            { "HECKLE017", 17},
            { "HECKLE018", 18},
            { "HECKLE019", 19},
            { "HECKLE020", 20},
            { "HECKLE021", 21},
            { "HECKLE022", 22},
            { "HECKLE023", 23},
            { "HECKLE024", 24},
            { "HECKLE025", 25},
            { "HECKLE026", 26},
            { "HECKLE027", 27},
            { "HECKLE028", 28},
            { "HECKLE029", 29},
            { "HECKLE030", 30},
            { "HECKLE031", 31},
            { "HECKLE032", 32},
            { "HECKLE033", 33},
            { "HECKLE034", 34},
            { "HECKLE035", 35},
            { "HECKLE036", 36},
            { "HECKLE037", 37},
            { "HECKLE038", 38},
            { "HECKLE039", 39},
            { "HECKLE040", 40},
            { "HECKLE041", 41},
            { "HECKLE042", 42},
            { "HECKLE043", 43},
            { "HECKLE044", 44},
            { "HECKLE045", 45},
            { "HECKLE046", 46},
            { "HECKLE047", 47},
            { "HECKLE048", 48},
            { "HECKLE049", 49},
            { "HECKLE050", 50},
            { "HECKLE051", 51},
            { "HECKLE052", 52},
            { "HECKLE053", 53},
            { "HECKLE054", 54},
            { "HECKLE055", 55},
            { "HECKLE056", 56},
            { "HECKLE057", 57},
            { "HECKLE058", 58},
            { "HECKLE059", 59},
            { "HECKLE060", 60},
            { "HECKLE061", 61},
            { "HECKLE062", 62},
            { "HECKLE063", 63},
            { "HECKLE064", 64},
            { "HECKLE065", 65},
            { "HECKLE066", 66},
            { "HECKLE067", 67},
            { "HECKLE068", 68},
            { "HECKLE069", 69},
            { "HECKLE070", 70},
            { "HECKLE071", 71},
            { "HECKLE072", 72},
            { "HECKLE073", 73},
            { "HECKLE074", 74},
            { "HECKLE075", 75},
            { "HECKLE076", 76},
            { "HECKLE077", 77},
            { "HECKLE078", 78},
            { "HECKLE079", 79},
            { "HECKLE080", 80},
            { "HECKLE081", 81},
            { "HECKLE082", 82},
            { "HECKLE083", 83},
            { "HECKLE084", 84},
            { "HECKLE085", 85},
            { "HECKLE086", 86},
            { "HECKLE087", 87},
            { "HECKLE088", 88},
            { "HECKLE089", 89},
            { "HECKLE090", 90},
            { "HECKLE091", 91},
            { "HECKLE092", 92},
            { "HECKLE093", 93},
            { "HECKLE094", 94},
            { "HECKLE095", 95},
            { "HECKLE096", 96},
            { "HECKLE097", 97},
            { "HECKLE098", 98},
            { "HECKLE099", 99},
            { "HECKLE100", 100},
            { "HECKLE101", 101},
            { "HECKLE102", 102},
            { "HECKLE103", 103},
            { "HECKLE104", 104},
            { "HECKLE105", 105},
            { "HECKLE106", 106},
            { "HECKLE107", 107},
            { "HECKLE108", 108},
            { "HECKLE109", 109},
            { "HECKLE110", 110},
            { "HECKLE111", 111},
            { "HECKLE112", 112},
            { "HECKLE113", 113},
            { "HECKLE114", 114},
            { "HECKLE115", 115},
            { "HECKLE116", 116},
            { "HECKLE117", 117},
            { "HECKLE118", 118},
            { "HECKLE119", 119},
            { "HECKLE120", 120},
            { "HECKLE121", 121},
            { "HECKLE122", 122},
            { "HECKLE123", 123},
            { "HECKLE124", 124},
            { "HECKLE125", 125},
            { "HECKLE126", 126},
            { "HECKLE127", 127},
            { "HECKLE128", 128},
            { "HECKLE129", 129},
            { "HECKLE130", 130},
            { "HECKLE131", 131},
            { "HECKLE132", 132},
            { "HECKLE133", 133},
            { "HECKLE134", 134},
            { "HECKLE135", 135},
            { "HECKLE136", 136},
            { "HECKLE137", 137},
            { "HECKLE138", 138},
            { "HECKLE139", 139},
            { "HECKLE140", 140},
            { "HECKLE141", 141},
            { "HECKLE142", 142},
            { "HECKLE143", 143},
            { "HECKLE144", 144},
            { "HECKLE145", 145},
            { "HECKLE146", 146},
            { "HECKLE147", 147},
            { "HECKLE148", 148},
            { "HECKLE149", 149},
            { "HECKLE150", 150},
            { "HECKLE151", 151},
            { "HECKLE152", 152},
            { "HECKLE153", 153},
            { "HECKLE154", 154},
            { "HECKLE155", 155},
            { "HECKLE156", 156},
            { "HECKLE157", 157},
            { "HECKLE158", 158},
            { "HECKLE159", 159},
            { "HECKLE160", 160},
            { "HECKLE161", 161},
            { "HECKLE162", 162},
            { "HECKLE163", 163},
            { "HECKLE164", 164},
            { "HECKLE165", 165},
            { "HECKLE166", 166},
            { "HECKLE167", 167},
            { "HECKLE168", 168},
            { "HECKLE169", 169},
            { "HECKLE170", 170},
            { "HECKLE171", 171},
            { "HECKLE172", 172},
            { "HECKLE173", 173},
            { "HECKLE174", 174},
            { "HECKLE175", 175},
            { "HECKLE176", 176},
            { "HECKLE177", 177},
            { "HECKLE178", 178},
            { "HECKLE179", 179},
            { "HECKLE180", 180},
            { "HECKLE181", 181},
            { "HECKLE182", 182},
            { "HECKLE183", 183},
            { "HECKLE184", 184},
            { "HECKLE185", 185},
            { "HECKLE186", 186},
            { "HECKLE187", 187},
            { "HECKLE188", 188},
            { "HECKLE189", 189},
            { "HECKLE190", 190},
            { "HECKLE191", 191},
            { "HECKLE192", 192},
            { "HECKLE193", 193},
            { "HECKLE194", 194},
            { "HECKLE195", 195},
            { "HECKLE196", 196},
            { "HECKLE197", 197},
            { "HECKLE198", 198},
            { "HECKLE199", 199},
            { "HECKLE200", 200},
            { "HECKLE201", 201},
            { "HECKLE202", 202},
            { "HECKLE203", 203},
            { "HECKLE204", 204},
            { "HECKLE205", 205},
            { "HECKLE206", 206},
            { "HECKLE207", 207},
            { "HECKLE208", 208},
            { "HECKLE209", 209},
            { "HECKLE210", 210},
            { "HECKLE211", 211},
            { "HECKLE212", 212},
            { "HECKLE213", 213},
            { "HECKLE214", 214},
            { "HECKLE215", 215},
            { "HECKLE216", 216},
            { "HECKLE217", 217},
            { "HECKLE218", 218},
            { "HECKLE219", 219},
            { "HECKLE220", 220},
            { "HECKLE221", 221},
            { "HECKLE222", 222},
            { "HECKLE223", 223},
            { "HECKLE224", 224},
            { "HECKLE225", 225},
            { "HECKLE226", 226},
            { "HECKLE227", 227},
            { "HECKLE228", 228},
            { "HECKLE229", 229},
            { "HECKLE230", 230},
            { "HECKLE231", 231},
            { "HECKLE232", 232},
            { "HECKLE233", 233},
            { "HECKLE234", 234},
            { "HECKLE235", 235},
            { "HECKLE236", 236},
            { "HECKLE237", 237},
            { "HECKLE238", 238},
            { "HECKLE239", 239},
            { "HECKLE240", 240},
            { "HECKLE241", 241},
            { "HECKLE242", 242},
            { "HECKLE243", 243},
            { "HECKLE244", 244},
            { "HECKLE245", 245},
            { "HECKLE246", 246},
            { "HECKLE247", 247},
            { "HECKLE248", 248},
            { "HECKLE249", 249},
            { "HECKLE250", 250},
            { "HECKLE251", 251},
            { "HECKLE252", 252},
            { "HECKLE253", 253},
            { "HECKLE254", 254},
            { "HECKLE255", 255},
            { "HECKLE256", 256},
            { "HECKLE257", 257},
            { "HECKLE258", 258},
            { "HECKLE259", 259},
            { "HECKLE260", 260},
            { "HECKLE261", 261},
            { "HECKLE262", 262},
            { "HECKLE263", 263},
            { "HECKLE264", 264},
            { "HECKLE265", 265},
            { "HECKLE266", 266},
            { "HECKLE267", 267},
            { "HECKLE268", 268},
            { "HECKLE269", 269},
            { "HECKLE270", 270},
            { "HECKLE271", 271},
            { "HECKLE272", 272},
            { "HECKLE273", 273},
            { "HECKLE274", 274},
            { "HECKLE275", 275},
            { "HECKLE276", 276},
            { "HECKLE277", 277},
            { "HECKLE278", 278},
            { "HECKLE279", 279},
        };
        public static class CaptainExos
        {
            // Captains
            public static CaptainExoKey CaptainExo000 { get; } = new CaptainExoKey("CaptainExo000");
            public static CaptainExoKey CaptainExo001 { get; } = new CaptainExoKey("CaptainExo001");
            public static CaptainExoKey CaptainExo002 { get; } = new CaptainExoKey("CaptainExo002");
            public static CaptainExoKey CaptainExo003 { get; } = new CaptainExoKey("CaptainExo003");
            public static CaptainExoKey CaptainExo004 { get; } = new CaptainExoKey("CaptainExo004");
            public static CaptainExoKey CaptainExo005 { get; } = new CaptainExoKey("CaptainExo005");
            public static CaptainExoKey CaptainExo006 { get; } = new CaptainExoKey("CaptainExo006");
            public static CaptainExoKey CaptainExo007 { get; } = new CaptainExoKey("CaptainExo007");
            public static CaptainExoKey CaptainExo008 { get; } = new CaptainExoKey("CaptainExo008");
            public static CaptainExoKey CaptainExo009 { get; } = new CaptainExoKey("CaptainExo009");
            public static CaptainExoKey CaptainExo010 { get; } = new CaptainExoKey("CaptainExo010");
            public static CaptainExoKey CaptainExo011 { get; } = new CaptainExoKey("CaptainExo011");
            public static CaptainExoKey CaptainExo012 { get; } = new CaptainExoKey("CaptainExo012");
            public static CaptainExoKey CaptainExo013 { get; } = new CaptainExoKey("CaptainExo013");
            public static CaptainExoKey CaptainExo014 { get; } = new CaptainExoKey("CaptainExo014");
            public static CaptainExoKey CaptainExo015 { get; } = new CaptainExoKey("CaptainExo015");
            public static CaptainExoKey CaptainExo016 { get; } = new CaptainExoKey("CaptainExo016");
            public static CaptainExoKey CaptainExo017 { get; } = new CaptainExoKey("CaptainExo017");
            public static CaptainExoKey CaptainExo018 { get; } = new CaptainExoKey("CaptainExo018");
            public static CaptainExoKey CaptainExo019 { get; } = new CaptainExoKey("CaptainExo019");
            public static CaptainExoKey CaptainExo020 { get; } = new CaptainExoKey("CaptainExo020");
            public static CaptainExoKey CaptainExo021 { get; } = new CaptainExoKey("CaptainExo021");
            public static CaptainExoKey CaptainExo022 { get; } = new CaptainExoKey("CaptainExo022");
            public static CaptainExoKey CaptainExo023 { get; } = new CaptainExoKey("CaptainExo023");
            public static CaptainExoKey CaptainExo024 { get; } = new CaptainExoKey("CaptainExo024");
            public static CaptainExoKey CaptainExo025 { get; } = new CaptainExoKey("CaptainExo025");
            public static CaptainExoKey CaptainExo026 { get; } = new CaptainExoKey("CaptainExo026");
            public static CaptainExoKey CaptainExo027 { get; } = new CaptainExoKey("CaptainExo027");
            public static CaptainExoKey CaptainExo028 { get; } = new CaptainExoKey("CaptainExo028");
            public static CaptainExoKey CaptainExo029 { get; } = new CaptainExoKey("CaptainExo029");
            public static CaptainExoKey CaptainExo030 { get; } = new CaptainExoKey("CaptainExo030");
            public static CaptainExoKey CaptainExo031 { get; } = new CaptainExoKey("CaptainExo031");
            public static CaptainExoKey CaptainExo032 { get; } = new CaptainExoKey("CaptainExo032");
            public static CaptainExoKey CaptainExo033 { get; } = new CaptainExoKey("CaptainExo033");
            public static CaptainExoKey CaptainExo034 { get; } = new CaptainExoKey("CaptainExo034");
            public static CaptainExoKey CaptainExo035 { get; } = new CaptainExoKey("CaptainExo035");
            public static CaptainExoKey CaptainExo036 { get; } = new CaptainExoKey("CaptainExo036");
            public static CaptainExoKey CaptainExo037 { get; } = new CaptainExoKey("CaptainExo037");
            public static CaptainExoKey CaptainExo038 { get; } = new CaptainExoKey("CaptainExo038");
            public static CaptainExoKey CaptainExo039 { get; } = new CaptainExoKey("CaptainExo039");
            public static CaptainExoKey CaptainExo040 { get; } = new CaptainExoKey("CaptainExo040");

            public static IList<IPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPrefabKey>()
                    {
                        CaptainExo000, CaptainExo001, CaptainExo002, CaptainExo003, CaptainExo004, CaptainExo005,
                        CaptainExo006, CaptainExo007, CaptainExo008, CaptainExo009, CaptainExo010, CaptainExo011,
                        CaptainExo012, CaptainExo013, CaptainExo014, CaptainExo015, CaptainExo016, CaptainExo017,
                        CaptainExo018, CaptainExo019, CaptainExo020, CaptainExo021, CaptainExo022, CaptainExo023,
                        CaptainExo024, CaptainExo025, CaptainExo026, CaptainExo027, CaptainExo028, CaptainExo029,
                        CaptainExo030, CaptainExo031, CaptainExo032, CaptainExo033, CaptainExo034, CaptainExo035,
                        CaptainExo036, CaptainExo037, CaptainExo038, CaptainExo039, CaptainExo040
                    };
                }
            }
        }

        public static class Effects
        {
            public static EffectKey BuilderDrone { get; } = new EffectKey("BuilderDrone");
        }

        public static class Explosions
        {
            public static ExplosionKey BulletImpact { get; } = new ExplosionKey("BulletImpact");
            public static ExplosionKey HighCalibreBulletImpact { get; } = new ExplosionKey("HighCalibreBulletImpact");
            public static ExplosionKey TinyBulletImpact { get; } = new ExplosionKey("TinyBulletImpact");
            public static ExplosionKey NovaShellImpact { get; } = new ExplosionKey("NovaShellImpact");
            public static ExplosionKey RocketShellImpact { get; } = new ExplosionKey("RocketShellImpact");
            public static ExplosionKey BombExplosion { get; } = new ExplosionKey("ExplosionBomb");
            public static ExplosionKey FlakExplosion { get; } = new ExplosionKey("ExplosionSAM");
            public static ExplosionKey Explosion75 { get; } = new ExplosionKey("Explosion0.75");
            public static ExplosionKey Explosion100 { get; } = new ExplosionKey("Explosion1.0");
            public static ExplosionKey Explosion150 { get; } = new ExplosionKey("Explosion1.5");
            public static ExplosionKey Explosion500 { get; } = new ExplosionKey("Explosion5.0");

            public static IList<IPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPrefabKey>()
                    {
                        BulletImpact, HighCalibreBulletImpact, TinyBulletImpact, NovaShellImpact, RocketShellImpact, BombExplosion, FlakExplosion, Explosion75, Explosion100, Explosion150, Explosion500
                    };
                }
            }
        }

        public static class Projectiles
        {
            public static ProjectileKey Bullet { get; } = new ProjectileKey("Bullet");
            public static ProjectileKey HighCalibreBullet { get; } = new ProjectileKey("HighCalibreBullet");
            public static ProjectileKey TinyBullet { get; } = new ProjectileKey("TinyBullet");
            public static ProjectileKey ShellSmall { get; } = new ProjectileKey("ShellSmall");
            public static ProjectileKey ShellLarge { get; } = new ProjectileKey("ShellLarge");
            public static ProjectileKey NovaShell { get; } = new ProjectileKey("NovaShell");
            public static ProjectileKey RocketShell { get; } = new ProjectileKey("RocketShell");
            public static ProjectileKey MissileSmall { get; } = new ProjectileKey("MissileSmall");
            public static ProjectileKey MissileMedium { get; } = new ProjectileKey("MissileMedium");
            public static ProjectileKey MissileLarge { get; } = new ProjectileKey("MissileLarge");
            public static ProjectileKey MissileSmart { get; } = new ProjectileKey("MissileSmart");

            public static ProjectileKey Bomb { get; } = new ProjectileKey("Bomb");
            public static ProjectileKey Nuke { get; } = new ProjectileKey("Nuke");
            public static ProjectileKey Rocket { get; } = new ProjectileKey("Rocket");
            public static ProjectileKey RocketSmall { get; } = new ProjectileKey("RocketSmall");

            public static IList<IPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPrefabKey>()
                    {
                        Bullet, HighCalibreBullet, TinyBullet, ShellSmall, ShellLarge, NovaShell, RocketShell,
                        MissileSmall, MissileMedium, MissileLarge, MissileSmart,
                        Bomb, Nuke, Rocket, RocketSmall
                    };
                }
            }
        }

        public static class ShipDeaths
        {
            public static ShipDeathKey AttackBoat { get; } = new ShipDeathKey("AttackBoat");
            public static ShipDeathKey AttackRIB { get; } = new ShipDeathKey("AttackRIB");
            public static ShipDeathKey Frigate { get; } = new ShipDeathKey("Frigate");
            public static ShipDeathKey Destroyer { get; } = new ShipDeathKey("Destroyer");
            public static ShipDeathKey Archon { get; } = new ShipDeathKey("Archon");

            public static IList<IPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPrefabKey>()
                    {
                        AttackBoat,
                        Frigate,
                        Destroyer,
                        Archon,
                        AttackRIB
                    };
                }
            }
        }

        public static IPrefabKey AudioSource { get; } = new GenericKey("AudioSource", "UI/Sound");
    }
}
