using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
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
            public static PvPBuildingKey PvPCoastguard { get; } = new PvPBuildingKey(PvPBuildingCategory.Defence, "PvPCoastguard");

            // Offence
            public static PvPBuildingKey PvPArtillery { get; } = new PvPBuildingKey(PvPBuildingCategory.Offence, "PvPArtillery");
            public static PvPBuildingKey PvPRocketLauncher { get; } = new PvPBuildingKey(PvPBuildingCategory.Offence, "PvPRocketLauncher");
            public static PvPBuildingKey PvPRailgun { get; } = new PvPBuildingKey(PvPBuildingCategory.Offence, "PvPRailgun");
            public static PvPBuildingKey PvPMLRS { get; } = new PvPBuildingKey(PvPBuildingCategory.Offence, "PvPMLRS");
            public static PvPBuildingKey PvPGatlingMortar { get; } = new PvPBuildingKey(PvPBuildingCategory.Offence, "PvPGatlingMortar");
            public static PvPBuildingKey PvPIonCannon { get; } = new PvPBuildingKey(PvPBuildingCategory.Offence, "PvPIonCannon");
            public static PvPBuildingKey PvPMissilePod { get; } = new PvPBuildingKey(PvPBuildingCategory.Offence, "PvPMissilePod");


            // Ultras
            public static PvPBuildingKey PvPDeathstarLauncher { get; } = new PvPBuildingKey(PvPBuildingCategory.Ultra, "PvPDeathstarLauncher");
            public static PvPBuildingKey PvPNukeLauncher { get; } = new PvPBuildingKey(PvPBuildingCategory.Ultra, "PvPNukeLauncher");
            public static PvPBuildingKey PvPUltralisk { get; } = new PvPBuildingKey(PvPBuildingCategory.Ultra, "PvPUltralisk");
            public static PvPBuildingKey PvPKamikazeSignal { get; } = new PvPBuildingKey(PvPBuildingCategory.Ultra, "PvPKamikazeSignal");
            public static PvPBuildingKey PvPBroadsides { get; } = new PvPBuildingKey(PvPBuildingCategory.Ultra, "PvPBroadsides");
            public static PvPBuildingKey PvPNovaArtillery { get; } = new PvPBuildingKey(PvPBuildingCategory.Ultra, "PvPNovaArtillery");

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
                        PvPAntiShipTurret, PvPAntiAirTurret, PvPMortar, PvPSamSite, PvPTeslaCoil,PvPCoastguard,
                        // Offence
                        PvPArtillery, PvPRocketLauncher, PvPRailgun, PvPMLRS, PvPGatlingMortar,PvPIonCannon,PvPMissilePod,
                        // Ultras
                        PvPDeathstarLauncher, PvPNukeLauncher, PvPUltralisk, PvPKamikazeSignal, PvPBroadsides, PvPNovaArtillery
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
            public static PvPUnitKey PvPBroadsword { get; } = new PvPUnitKey(PvPUnitCategory.Aircraft, "PvPBroadsword");
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
                        PvPBomber, PvPFighter, PvPGunship, PvPSteamCopter, PvPBroadsword, PvPTestAircraft,
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
            public static PvPExplosionKey PvPBulletImpact { get; } = new PvPExplosionKey("PvPBulletImpact");
            public static PvPExplosionKey PvPHighCalibreBulletImpact { get; } = new PvPExplosionKey("PvPHighCalibreBulletImpact");
            public static PvPExplosionKey PvPTinyBulletImpact { get; } = new PvPExplosionKey("PvPTinyBulletImpact");
            public static PvPExplosionKey PvPNovaShellImpact { get; } = new PvPExplosionKey("PvPNovaShellImpact");
            public static PvPExplosionKey PvPRocketShellImpact { get; } = new PvPExplosionKey("PvPRocketShellImpact");
            public static PvPExplosionKey PvPBombExplosion { get; } = new PvPExplosionKey("PvPExplosionBomb");
            public static PvPExplosionKey PvPFlakExplosion { get; } = new PvPExplosionKey("PvPExplosionSAM");
            public static PvPExplosionKey PvPExplosion75 { get; } = new PvPExplosionKey("PvPExplosion0.75");
            public static PvPExplosionKey PvPExplosion100 { get; } = new PvPExplosionKey("PvPExplosion1.0");
            public static PvPExplosionKey PvPExplosion150 { get; } = new PvPExplosionKey("PvPExplosion1.5");
            public static PvPExplosionKey PvPExplosion500 { get; } = new PvPExplosionKey("PvPExplosion5.0");

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

            public static PvPProjectileKey PvPBomb { get; } = new PvPProjectileKey("PvPBomb");
            public static PvPProjectileKey PvPNuke { get; } = new PvPProjectileKey("PvPNuke");
            public static PvPProjectileKey PvPRocket { get; } = new PvPProjectileKey("PvPRocket");
            public static PvPProjectileKey PvPRocketSmall { get; } = new PvPProjectileKey("PvPRocketSmall");

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
            public static PvPShipDeathKey PvPAttackBoat { get; } = new PvPShipDeathKey("PvPAttackBoat");
            public static PvPShipDeathKey PvPAttackRIB { get; } = new PvPShipDeathKey("PvPAttackRIB");
            public static PvPShipDeathKey PvPFrigate { get; } = new PvPShipDeathKey("PvPFrigate");
            public static PvPShipDeathKey PvPDestroyer { get; } = new PvPShipDeathKey("PvPDestroyer");
            public static PvPShipDeathKey PvPArchon { get; } = new PvPShipDeathKey("PvPArchon");

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

        public static IPvPPrefabKey AudioSource { get; } = new PvPGenericKey("PvPAudioSource", "UI/Sound");

        public static class PvPBuildableOutlines
        {
            public static PvPBuildableOutlineKey PvPAirFactoryOutline { get; } = new PvPBuildableOutlineKey("PvPAirFactoryOutline");
            public static PvPBuildableOutlineKey PvPAntiAirTurretOutline { get; } = new PvPBuildableOutlineKey("PvPAntiAirTurretOutline");
            public static PvPBuildableOutlineKey PvPAntiShipTurretOutline { get; } = new PvPBuildableOutlineKey("PvPAntiShipTurretOutline");
            public static PvPBuildableOutlineKey PvPArtilleryOutline { get; } = new PvPBuildableOutlineKey("PvPArtilleryOutline");
            public static PvPBuildableOutlineKey PvPBroadsidesOutline { get; } = new PvPBuildableOutlineKey("PvPBroadsidesOutline");
            public static PvPBuildableOutlineKey PvPControlTowerOutline { get; } = new PvPBuildableOutlineKey("PvPControlTowerOutline");
            public static PvPBuildableOutlineKey PvPDeathstarLauncherOutline { get; } = new PvPBuildableOutlineKey("PvPDeathstarLauncherOutline");
            public static PvPBuildableOutlineKey PvPEngineeringBay4Outline { get; } = new PvPBuildableOutlineKey("PvPEngineeringBay4Outline");
            public static PvPBuildableOutlineKey PvPEngineeringBay8Outline { get; } = new PvPBuildableOutlineKey("PvPEngineeringBay8Outline");
            public static PvPBuildableOutlineKey PvPEngineeringBayOutline { get; } = new PvPBuildableOutlineKey("PvPEngineeringBayOutline");
            public static PvPBuildableOutlineKey PvPGatlingMortarOutline { get; } = new PvPBuildableOutlineKey("PvPGatlingMortarOutline");
            public static PvPBuildableOutlineKey PvPKamikazeSignalOutline { get; } = new PvPBuildableOutlineKey("PvPKamikazeSignalOutline");
            public static PvPBuildableOutlineKey PvPLocalBoosterOutline { get; } = new PvPBuildableOutlineKey("PvPLocalBoosterOutline");
            public static PvPBuildableOutlineKey PvPMLRSOutline { get; } = new PvPBuildableOutlineKey("PvPMLRSOutline");
            public static PvPBuildableOutlineKey PvPMortarOutline { get; } = new PvPBuildableOutlineKey("PvPMortarOutline");
            public static PvPBuildableOutlineKey PvPNavalFactoryOutline { get; } = new PvPBuildableOutlineKey("PvPNavalFactoryOutline");
            public static PvPBuildableOutlineKey PvPNukeLauncherOutline { get; } = new PvPBuildableOutlineKey("PvPNukeLauncherOutline");
            public static PvPBuildableOutlineKey PvPRailgunOutline { get; } = new PvPBuildableOutlineKey("PvPRailgunOutline");
            public static PvPBuildableOutlineKey PvPRocketLauncherOutline { get; } = new PvPBuildableOutlineKey("PvPRocketLauncherOutline");
            public static PvPBuildableOutlineKey PvPSamSiteOutline { get; } = new PvPBuildableOutlineKey("PvPSamSiteOutline");
            public static PvPBuildableOutlineKey PvPShieldGeneratorOutline { get; } = new PvPBuildableOutlineKey("PvPShieldGeneratorOutline");
            public static PvPBuildableOutlineKey PvPStealthGeneratorOutline { get; } = new PvPBuildableOutlineKey("PvPStealthGeneratorOutline");
            public static PvPBuildableOutlineKey PvPTeslaCoilOutline { get; } = new PvPBuildableOutlineKey("PvPTeslaCoilOutline");
            public static PvPBuildableOutlineKey PvPUltraliskOutline { get; } = new PvPBuildableOutlineKey("PvPUltraliskOutline");

            public static IList<IPvPPrefabKey> AllKeys
            {
                get
                {
                    return new List<IPvPPrefabKey>()
                    {
                        PvPAirFactoryOutline,
                        PvPAntiAirTurretOutline,
                        PvPAntiShipTurretOutline,
                        PvPArtilleryOutline,
                        PvPBroadsidesOutline,
                        PvPControlTowerOutline,
                        PvPDeathstarLauncherOutline,
                        PvPEngineeringBay4Outline,
                        PvPEngineeringBay8Outline,
                        PvPEngineeringBayOutline,
                        PvPGatlingMortarOutline,
                        PvPKamikazeSignalOutline,
                        PvPLocalBoosterOutline,
                        PvPMLRSOutline,
                        PvPMortarOutline,
                        PvPNavalFactoryOutline,
                        PvPNukeLauncherOutline,
                        PvPRailgunOutline,
                        PvPRocketLauncherOutline,
                        PvPSamSiteOutline,
                        PvPShieldGeneratorOutline,
                        PvPStealthGeneratorOutline,
                        PvPTeslaCoilOutline,
                        PvPUltraliskOutline
                    };
                }
            }
        }
    }
}
