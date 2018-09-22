using BattleCruisers.UI.Sound;

namespace BattleCruisers.Data.Static
{
    public static class SoundKeys
    {
        public static class Deaths
        {
            public static ISoundKey Aircraft { get { return new SoundKey(SoundType.Deaths, "aircraft"); } }
            public static ISoundKey Ship { get { return new SoundKey(SoundType.Deaths, "ship"); } }
            public static ISoundKey Cruiser { get { return new SoundKey(SoundType.Deaths, "cruiser"); } }
            public static ISoundKey Building1 { get { return new SoundKey(SoundType.Deaths, "building1"); } }
            public static ISoundKey Building2 { get { return new SoundKey(SoundType.Deaths, "building2"); } }
            public static ISoundKey Building3 { get { return new SoundKey(SoundType.Deaths, "building3"); } }
            public static ISoundKey Building4 { get { return new SoundKey(SoundType.Deaths, "building4"); } }
            public static ISoundKey Building5 { get { return new SoundKey(SoundType.Deaths, "building5"); } }
        }

        public static class Engines
        {
            // Ships
			public static ISoundKey AtatckBoat{ get { return new SoundKey(SoundType.Engines, "attack-boat"); } }
			public static ISoundKey Frigate { get { return new SoundKey(SoundType.Engines, "frigate"); } }
			public static ISoundKey Destroyer { get { return new SoundKey(SoundType.Engines, "destroyer"); } }
            public static ISoundKey Archon { get { return new SoundKey(SoundType.Engines, "archon"); } }

            // Aircraft
            public static ISoundKey Bomber { get { return new SoundKey(SoundType.Engines, "bomber"); } }
            public static ISoundKey Gunship { get { return new SoundKey(SoundType.Engines, "gunship"); } }
            public static ISoundKey Fighter { get { return new SoundKey(SoundType.Engines, "fighter"); } }
        }

        public static class Firing
        {
            public static ISoundKey AntiAir { get { return new SoundKey(SoundType.Firing, "anti-air"); } }
            public static ISoundKey Artillery { get { return new SoundKey(SoundType.Firing, "artillery"); } }
            public static ISoundKey Broadsides { get { return new SoundKey(SoundType.Firing, "broadsides"); } }
			public static ISoundKey BigCannon { get { return new SoundKey(SoundType.Firing, "big-cannon"); } }
            public static ISoundKey Laser { get { return new SoundKey(SoundType.Firing, "laser"); } }
        }

        public static class Explosions
        {
            public static ISoundKey Bomb { get { return new SoundKey(SoundType.Explosions, "bomb"); } }
            public static ISoundKey Default { get { return new SoundKey(SoundType.Explosions, "default"); } }
        }

        public static class Completed
        {
            public static ISoundKey Aircraft { get { return new SoundKey(SoundType.Completed, "aircraft"); } }
            public static ISoundKey Building { get { return new SoundKey(SoundType.Completed, "building"); } }
            public static ISoundKey Ship { get { return new SoundKey(SoundType.Completed, "ship"); } }
            public static ISoundKey Ultra { get { return new SoundKey(SoundType.Completed, "ultra"); } }
        }

        public static class Events
        {
            // Cruiser
            public static ISoundKey CruiserUnderAttack { get { return new SoundKey(SoundType.Events, "cruiser-under-attack"); } }
            public static ISoundKey CruiserSignificantlyDamaged { get { return new SoundKey(SoundType.Events, "cruiser-significantly-damaged"); } }
            
            // Drones
            public static ISoundKey DronesNewDronesReady { get { return new SoundKey(SoundType.Events, "drones-new-drones-ready"); } }
            public static ISoundKey DronesIdle { get { return new SoundKey(SoundType.Events, "drones-idle"); } }

            public static ISoundKey EnemyStartedUltra { get { return new SoundKey(SoundType.Events, "enemy-started-ultra"); } }
        }

        public static class Music
        {
            // Kentient
            public static ISoundKey Kentient { get { return new SoundKey(SoundType.Music, "kentient"); } }
            public static ISoundKey KentientDanger { get { return new SoundKey(SoundType.Music, "kentient-danger"); } }
            public static ISoundKey KentientVictory { get { return new SoundKey(SoundType.Music, "kentient-victory"); } }

            // Other
            public static ISoundKey MainTheme { get { return new SoundKey(SoundType.Music, "main-theme"); } }
        }
    }
}
