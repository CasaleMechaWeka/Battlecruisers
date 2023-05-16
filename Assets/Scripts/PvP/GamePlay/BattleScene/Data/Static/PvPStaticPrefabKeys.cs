using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static
{
    public static class PvPStaticPrefabKeys
    {
        public static class PvPBuildings
        {
            // Factories
            public static PvPBuildingKey PvPAirFactory { get; } = new PvPBuildingKey(PvPBuildingCategory.Factory, "PvPAirFactory");
            public static PvPBuildingKey PvPNavalFactory { get; } = new PvPBuildingKey(PvPBuildingCategory.Factory, "PvPNavalFactory");
            public static PvPBuildingKey PvPDroneStation { get; } = new PvPBuildingKey(PvPBuildingCategory.Factory, "PvPEngineeringBay");
            public static PvPBuildingKey PvPDroneStation4 { get; } = new PvPBuildingKey(PvPBuildingCategory.Factory, "PvPEngineeringBay4");
            public static PvPBuildingKey PvPDroneStation8 { get; } = new PvPBuildingKey(PvPBuildingCategory.Factory, "PvPEngineeringBay8");

            // Tactical
            public static PvPBuildingKey PvPShieldGenerator { get; } = new PvPBuildingKey(PvPBuildingCategory.Tactical, "PvPShieldGenerator");
            public static PvPBuildingKey PvPStealthGenerator { get; } = new PvPBuildingKey(PvPBuildingCategory.Tactical, "PvPStealthGenerator");
            public static PvPBuildingKey PvPSpySatelliteLauncher { get; } = new PvPBuildingKey(PvPBuildingCategory.Tactical, "PvPSpySatelliteLauncher");
            public static PvPBuildingKey PvPLocalBooster { get; } = new PvPBuildingKey(PvPBuildingCategory.Tactical, "PvPLocalBooster");
            public static PvPBuildingKey PvPControlTower { get; } = new PvPBuildingKey(PvPBuildingCategory.Tactical, "PvPControlTower");

            // Defence
            public static PvPBuildingKey PvPAntiShipTurret { get; } = new PvPBuildingKey(PvPBuildingCategory.Defence, "PvPAntiShipTurret");
            public static PvPBuildingKey PvPAntiAirTurret { get; } = new PvPBuildingKey(PvPBuildingCategory.Defence, "PvPAntiAirTurret");
            public static PvPBuildingKey PvPMortar { get; } = new PvPBuildingKey(PvPBuildingCategory.Defence, "PvPMortar");
            public static PvPBuildingKey PvPSamSite { get; } = new PvPBuildingKey(PvPBuildingCategory.Defence, "PvPSamSite");
            public static PvPBuildingKey PvPTeslaCoil { get; } = new PvPBuildingKey(PvPBuildingCategory.Defence, "PvPTeslaCoil");

            // Offence
            public static PvPBuildingKey PvPArtillery { get; } = new PvPBuildingKey(PvPBuildingCategory.Offence, "PvPArtillery");
            public static PvPBuildingKey PvPRocketLauncher { get; } = new PvPBuildingKey(PvPBuildingCategory.Offence, "PvPRocketLauncher");
            public static PvPBuildingKey PvPRailgun { get; } = new PvPBuildingKey(PvPBuildingCategory.Offence, "PvPRailgun");
            public static PvPBuildingKey PvPMLRS { get; } = new PvPBuildingKey(PvPBuildingCategory.Offence, "PvPMLRS");
            public static PvPBuildingKey PvPGatlingMortar { get; } = new PvPBuildingKey(PvPBuildingCategory.Offence, "PvPGatlingMortar");


            // Ultras
            public static PvPBuildingKey PvPDeathstarLauncher { get; } = new PvPBuildingKey(PvPBuildingCategory.Ultra, "PvPDeathstarLauncher");
            public static PvPBuildingKey PvPNukeLauncher { get; } = new PvPBuildingKey(PvPBuildingCategory.Ultra, "PvPNukeLauncher");
            public static PvPBuildingKey PvPUltralisk { get; } = new PvPBuildingKey(PvPBuildingCategory.Ultra, "PvPUltralisk");
            public static PvPBuildingKey PvPKamikazeSignal { get; } = new PvPBuildingKey(PvPBuildingCategory.Ultra, "PvPKamikazeSignal");
            public static PvPBuildingKey PvPBroadsides { get; } = new PvPBuildingKey(PvPBuildingCategory.Ultra, "PvPBroadsides");

            public static IList<IPvPPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPvPPrefabKey>()
                    {
                        // Factories
                        PvPAirFactory, PvPNavalFactory, PvPDroneStation, PvPDroneStation4, PvPDroneStation8,
                        // Tactical
                        PvPShieldGenerator, PvPStealthGenerator, PvPSpySatelliteLauncher, PvPLocalBooster, PvPControlTower,
                        // Defence
                        PvPAntiShipTurret, PvPAntiAirTurret, PvPMortar, PvPSamSite, PvPTeslaCoil,
                        // Offence
                        PvPArtillery, PvPRocketLauncher, PvPRailgun, PvPMLRS, PvPGatlingMortar,
                        // Ultras
                        PvPDeathstarLauncher, PvPNukeLauncher, PvPUltralisk, PvPKamikazeSignal, PvPBroadsides
                    };
                }
            }
        }


        public static class PvPUnits
        {
            // Aircraft
            public static PvPUnitKey PvPBomber { get; } = new PvPUnitKey(PvPUnitCategory.Aircraft, "PvPBomber");
            public static PvPUnitKey PvPFighter { get; } = new PvPUnitKey(PvPUnitCategory.Aircraft, "PvPFighter");
            public static PvPUnitKey PvPGunship { get; } = new PvPUnitKey(PvPUnitCategory.Aircraft, "PvPGunship");
            public static PvPUnitKey PvPSteamCopter { get; } = new PvPUnitKey(PvPUnitCategory.Aircraft, "PvPSteamCopter");
            public static PvPUnitKey PvPTestAircraft { get; } = new PvPUnitKey(PvPUnitCategory.Aircraft, "PvPTestAircraft");

            // Ships
            public static PvPUnitKey PvPAttackBoat { get; } = new PvPUnitKey(PvPUnitCategory.Naval, "PvPAttackBoat");
            public static PvPUnitKey PvPAttackRIB { get; } = new PvPUnitKey(PvPUnitCategory.Naval, "PvPAttackRIB");
            public static PvPUnitKey PvPFrigate { get; } = new PvPUnitKey(PvPUnitCategory.Naval, "PvPFrigate");
            public static PvPUnitKey PvPDestroyer { get; } = new PvPUnitKey(PvPUnitCategory.Naval, "PvPDestroyer");
            public static PvPUnitKey PvPArchonBattleship { get; } = new PvPUnitKey(PvPUnitCategory.Naval, "PvPArchonBattleship");

            public static IList<IPvPPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPvPPrefabKey>()
                    {
                        // Aircraft
                        PvPBomber, PvPFighter, PvPGunship, PvPSteamCopter, PvPTestAircraft,
                        // Ships
                        PvPAttackBoat, PvPAttackRIB, PvPFrigate, PvPDestroyer, PvPArchonBattleship
                    };
                }
            }
        }

        public static class PvPHulls
        {
            public static PvPHullKey PvPBullshark { get; } = new PvPHullKey("PvPBullshark");
            public static PvPHullKey PvPEagle { get; } = new PvPHullKey("PvPEagle");
            public static PvPHullKey PvPHammerhead { get; } = new PvPHullKey("PvPHammerhead");
            public static PvPHullKey PvPLongbow { get; } = new PvPHullKey("PvPLongbow");
            public static PvPHullKey PvPMegalodon { get; } = new PvPHullKey("PvPMegalodon");
            public static PvPHullKey PvPRaptor { get; } = new PvPHullKey("PvPRaptor");
            public static PvPHullKey PvPRockjaw { get; } = new PvPHullKey("PvPRockjaw");
            public static PvPHullKey PvPTrident { get; } = new PvPHullKey("PvPTrident");
            public static PvPHullKey PvPManOfWarBoss { get; } = new PvPHullKey("PvPManOfWarBoss");
            public static PvPHullKey PvPHuntressBoss { get; } = new PvPHullKey("PvPHuntressBoss");
            public static PvPHullKey PvPTasDevil { get; } = new PvPHullKey("PvPTasDevil");
            public static PvPHullKey PvPYeti { get; } = new PvPHullKey("PvPYeti");
            public static PvPHullKey PvPRickshaw { get; } = new PvPHullKey("PvPRickshaw");
            public static PvPHullKey PvPBlackRig { get; } = new PvPHullKey("PvPBlackRig");


            public static IList<IPvPPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPvPPrefabKey>()
                    {
                        PvPBullshark, PvPEagle, PvPHammerhead, PvPLongbow, PvPMegalodon, PvPRaptor, PvPRockjaw, PvPTrident, PvPManOfWarBoss, PvPHuntressBoss, PvPBlackRig, PvPYeti, PvPRickshaw, PvPTasDevil
                    };
                }
            }

            public static IList<PvPHullKey> AllKeysExplicit
            {
                get
                {
                    return new List<PvPHullKey>()
                    {
                        PvPBullshark, PvPEagle, PvPHammerhead, PvPLongbow, PvPMegalodon, PvPRaptor, PvPRockjaw, PvPTrident, PvPBlackRig, PvPYeti, PvPRickshaw, PvPTasDevil
                    };
                }
            }
        }

        public static class PvPEffects
        {
            public static PvPEffectKey PvPBuilderDrone { get; } = new PvPEffectKey("PvPBuilderDrone");
        }

        public static class PvPExplosions
        {
            public static PvPExplosionKey PvPBulletImpact { get; } = new PvPExplosionKey("BulletImpact");
            public static PvPExplosionKey PvPHighCalibreBulletImpact { get; } = new PvPExplosionKey("HighCalibreBulletImpact");
            public static PvPExplosionKey PvPTinyBulletImpact { get; } = new PvPExplosionKey("TinyBulletImpact");
            public static PvPExplosionKey PvPNovaShellImpact { get; } = new PvPExplosionKey("NovaShellImpact");
            public static PvPExplosionKey PvPRocketShellImpact { get; } = new PvPExplosionKey("RocketShellImpact");
            public static PvPExplosionKey PvPBombExplosion { get; } = new PvPExplosionKey("ExplosionBomb");
            public static PvPExplosionKey PvPFlakExplosion { get; } = new PvPExplosionKey("ExplosionSAM");
            public static PvPExplosionKey PvPExplosion75 { get; } = new PvPExplosionKey("Explosion0.75");
            public static PvPExplosionKey PvPExplosion100 { get; } = new PvPExplosionKey("Explosion1.0");
            public static PvPExplosionKey PvPExplosion150 { get; } = new PvPExplosionKey("Explosion1.5");
            public static PvPExplosionKey PvPExplosion500 { get; } = new PvPExplosionKey("Explosion5.0");

            public static IList<IPvPPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPvPPrefabKey>()
                    {
                        PvPBulletImpact, PvPHighCalibreBulletImpact, PvPTinyBulletImpact, PvPNovaShellImpact, PvPRocketShellImpact, PvPBombExplosion, PvPFlakExplosion, PvPExplosion75, PvPExplosion100, PvPExplosion150, PvPExplosion500
                    };
                }
            }
        }

        public static class PvPProjectiles
        {
            public static PvPProjectileKey PvPBullet { get; } = new PvPProjectileKey("PvPBullet");
            public static PvPProjectileKey PvPHighCalibreBullet { get; } = new PvPProjectileKey("PvPHighCalibreBullet");
            public static PvPProjectileKey PvPTinyBullet { get; } = new PvPProjectileKey("PvPTinyBullet");
            public static PvPProjectileKey PvPShellSmall { get; } = new PvPProjectileKey("PvPShellSmall");
            public static PvPProjectileKey PvPShellLarge { get; } = new PvPProjectileKey("PvPShellLarge");
            public static PvPProjectileKey PvPNovaShell { get; } = new PvPProjectileKey("PvPNovaShell");
            public static PvPProjectileKey PvPRocketShell { get; } = new PvPProjectileKey("PvPRocketShell");
            public static PvPProjectileKey PvPMissileSmall { get; } = new PvPProjectileKey("PvPMissileSmall");
            public static PvPProjectileKey PvPMissileMedium { get; } = new PvPProjectileKey("PvPMissileMedium");
            public static PvPProjectileKey PvPMissileLarge { get; } = new PvPProjectileKey("PvPMissileLarge");
            public static PvPProjectileKey PvPMissileSmart { get; } = new PvPProjectileKey("PvPMissileSmart");

            public static PvPProjectileKey PvPBomb { get; } = new PvPProjectileKey("Bomb");
            public static PvPProjectileKey PvPNuke { get; } = new PvPProjectileKey("Nuke");
            public static PvPProjectileKey PvPRocket { get; } = new PvPProjectileKey("Rocket");
            public static PvPProjectileKey PvPRocketSmall { get; } = new PvPProjectileKey("RocketSmall");

            public static IList<IPvPPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPvPPrefabKey>()
                    {
                        PvPBullet, PvPHighCalibreBullet, PvPTinyBullet, PvPShellSmall, PvPShellLarge, PvPNovaShell, PvPRocketShell,
                        PvPMissileSmall, PvPMissileMedium, PvPMissileLarge, PvPMissileSmart,
                        PvPBomb, PvPNuke, PvPRocket, PvPRocketSmall
                    };
                }
            }
        }

        public static class PvPShipDeaths
        {
            public static PvPShipDeathKey PvPAttackBoat { get; } = new PvPShipDeathKey("AttackBoat");
            public static PvPShipDeathKey PvPAttackRIB { get; } = new PvPShipDeathKey("AttackRIB");
            public static PvPShipDeathKey PvPFrigate { get; } = new PvPShipDeathKey("Frigate");
            public static PvPShipDeathKey PvPDestroyer { get; } = new PvPShipDeathKey("Destroyer");
            public static PvPShipDeathKey PvPArchon { get; } = new PvPShipDeathKey("Archon");

            public static IList<IPvPPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPvPPrefabKey>()
                    {
                        PvPAttackBoat,
                        PvPFrigate,
                        PvPDestroyer,
                        PvPArchon,
                        PvPAttackRIB
                    };
                }
            }
        }

        public static IPvPPrefabKey AudioSource { get; } = new PvPGenericKey("AudioSource", "UI/Sound");
    }
}
