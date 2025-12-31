using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static
{
    public static class PvPStaticPrefabKeys
    {
        public static class PvPBuildings
        {
            // Factories
            public static PvPBuildingKey PvPAirFactory { get; } = new PvPBuildingKey(BuildingCategory.Factory, "PvPAirFactory");
            public static PvPBuildingKey PvPNavalFactory { get; } = new PvPBuildingKey(BuildingCategory.Factory, "PvPNavalFactory");
            public static PvPBuildingKey PvPDroneStation { get; } = new PvPBuildingKey(BuildingCategory.Factory, "PvPEngineeringBay");
            public static PvPBuildingKey PvPDroneStation4 { get; } = new PvPBuildingKey(BuildingCategory.Factory, "PvPEngineeringBay4");
            public static PvPBuildingKey PvPDroneStation6 { get; } = new PvPBuildingKey(BuildingCategory.Factory, "PvPEngineeringBay6");
            public static PvPBuildingKey PvPDroneStation8 { get; } = new PvPBuildingKey(BuildingCategory.Factory, "PvPEngineeringBay8");
            public static PvPBuildingKey PvPDroneFactory { get; } = new PvPBuildingKey(BuildingCategory.Factory, "PvPDroneFactory");

            // Tactical
            public static PvPBuildingKey PvPShieldGenerator { get; } = new PvPBuildingKey(BuildingCategory.Tactical, "PvPShieldGenerator");
            public static PvPBuildingKey PvPStealthGenerator { get; } = new PvPBuildingKey(BuildingCategory.Tactical, "PvPStealthGenerator");
            public static PvPBuildingKey PvPSpySatelliteLauncher { get; } = new PvPBuildingKey(BuildingCategory.Tactical, "PvPSpySatelliteLauncher");
            public static PvPBuildingKey PvPLocalBooster { get; } = new PvPBuildingKey(BuildingCategory.Tactical, "PvPLocalBooster");
            public static PvPBuildingKey PvPControlTower { get; } = new PvPBuildingKey(BuildingCategory.Tactical, "PvPControlTower");
            public static PvPBuildingKey PvPGrapheneBarrier { get; } = new PvPBuildingKey(BuildingCategory.Tactical, "PvPGrapheneBarrier");

            // Defence
            public static PvPBuildingKey PvPAntiShipTurret { get; } = new PvPBuildingKey(BuildingCategory.Defence, "PvPAntiShipTurret");
            public static PvPBuildingKey PvPAntiAirTurret { get; } = new PvPBuildingKey(BuildingCategory.Defence, "PvPAntiAirTurret");
            public static PvPBuildingKey PvPMortar { get; } = new PvPBuildingKey(BuildingCategory.Defence, "PvPMortar");
            public static PvPBuildingKey PvPSamSite { get; } = new PvPBuildingKey(BuildingCategory.Defence, "PvPSamSite");
            public static PvPBuildingKey PvPTeslaCoil { get; } = new PvPBuildingKey(BuildingCategory.Defence, "PvPTeslaCoil");
            public static PvPBuildingKey PvPCoastguard { get; } = new PvPBuildingKey(BuildingCategory.Defence, "PvPCoastguard");
            public static PvPBuildingKey PvPFlakTurret { get; } = new PvPBuildingKey(BuildingCategory.Defence, "PvPFlakTurret");
            public static PvPBuildingKey PvPCIWS { get; } = new PvPBuildingKey(BuildingCategory.Defence, "PvPCIWS");

            // Offence
            public static PvPBuildingKey PvPArtillery { get; } = new PvPBuildingKey(BuildingCategory.Offence, "PvPArtillery");
            public static PvPBuildingKey PvPRocketLauncher { get; } = new PvPBuildingKey(BuildingCategory.Offence, "PvPRocketLauncher");
            public static PvPBuildingKey PvPRailgun { get; } = new PvPBuildingKey(BuildingCategory.Offence, "PvPRailgun");
            public static PvPBuildingKey PvPMLRS { get; } = new PvPBuildingKey(BuildingCategory.Offence, "PvPMLRS");
            public static PvPBuildingKey PvPGatlingMortar { get; } = new PvPBuildingKey(BuildingCategory.Offence, "PvPGatlingMortar");
            public static PvPBuildingKey PvPIonCannon { get; } = new PvPBuildingKey(BuildingCategory.Offence, "PvPIonCannon");
            public static PvPBuildingKey PvPMissilePod { get; } = new PvPBuildingKey(BuildingCategory.Offence, "PvPMissilePod");
            public static PvPBuildingKey PvPCannon { get; } = new PvPBuildingKey(BuildingCategory.Offence, "PvPCannon");
            public static PvPBuildingKey PvPBlastVLS { get; } = new PvPBuildingKey(BuildingCategory.Offence, "PvPBlastVLS");
            public static PvPBuildingKey PvPFirecrackerVLS { get; } = new PvPBuildingKey(BuildingCategory.Offence, "PvPFirecrackerVLS");

            // Ultras
            public static PvPBuildingKey PvPDeathstarLauncher { get; } = new PvPBuildingKey(BuildingCategory.Ultra, "PvPDeathstarLauncher");
            public static PvPBuildingKey PvPNukeLauncher { get; } = new PvPBuildingKey(BuildingCategory.Ultra, "PvPNukeLauncher");
            public static PvPBuildingKey PvPUltralisk { get; } = new PvPBuildingKey(BuildingCategory.Ultra, "PvPUltralisk");
            public static PvPBuildingKey PvPKamikazeSignal { get; } = new PvPBuildingKey(BuildingCategory.Ultra, "PvPKamikazeSignal");
            public static PvPBuildingKey PvPBroadsides { get; } = new PvPBuildingKey(BuildingCategory.Ultra, "PvPBroadsides");
            public static PvPBuildingKey PvPNovaArtillery { get; } = new PvPBuildingKey(BuildingCategory.Ultra, "PvPNovaArtillery");
            public static PvPBuildingKey PvPUltraCIWS { get; } = new PvPBuildingKey(BuildingCategory.Ultra, "PvPUltraCIWS");
            public static PvPBuildingKey PvPGlobeShield { get; } = new PvPBuildingKey(BuildingCategory.Ultra, "PvPGlobeShield");
            public static PvPBuildingKey PvPSledgehammer { get; } = new PvPBuildingKey(BuildingCategory.Ultra, "PvPSledgehammer");
            public static PvPBuildingKey PvPRailCannon { get; } = new PvPBuildingKey(BuildingCategory.Ultra, "PvPRailCannon");

            public static ReadOnlyCollection<IPrefabKey> AllKeys = new ReadOnlyCollection<IPrefabKey>(new List<IPrefabKey>()
            {
                // Factories
                PvPAirFactory, PvPNavalFactory, PvPDroneStation, PvPDroneStation4, PvPDroneStation6, PvPDroneStation8, PvPDroneFactory,
                // Tactical
                PvPShieldGenerator, PvPStealthGenerator, PvPSpySatelliteLauncher, PvPLocalBooster, PvPControlTower, PvPGrapheneBarrier,
                // Defence
                PvPAntiShipTurret, PvPAntiAirTurret, PvPMortar, PvPSamSite, PvPTeslaCoil, PvPCoastguard, PvPFlakTurret, PvPCIWS,
                // Offence
                PvPArtillery, PvPRocketLauncher, PvPRailgun, PvPMLRS, PvPGatlingMortar, PvPIonCannon, PvPMissilePod, PvPCannon, PvPBlastVLS, PvPFirecrackerVLS,
                // Ultras
                PvPDeathstarLauncher, PvPNukeLauncher, PvPUltralisk, PvPKamikazeSignal, PvPBroadsides, PvPNovaArtillery, PvPUltraCIWS, PvPGlobeShield, PvPSledgehammer, PvPRailCannon
            });
        }

        public static class PvPUnits
        {
            // Aircraft
            public static PvPUnitKey PvPBomber { get; } = new PvPUnitKey(UnitCategory.Aircraft, "PvPBomber");
            public static PvPUnitKey PvPFighter { get; } = new PvPUnitKey(UnitCategory.Aircraft, "PvPFighter");
            public static PvPUnitKey PvPGunship { get; } = new PvPUnitKey(UnitCategory.Aircraft, "PvPGunship");
            public static PvPUnitKey PvPSteamCopter { get; } = new PvPUnitKey(UnitCategory.Aircraft, "PvPSteamCopter");
            public static PvPUnitKey PvPBroadsword { get; } = new PvPUnitKey(UnitCategory.Aircraft, "PvPBroadsword");
            public static PvPUnitKey PvPStratBomber { get; } = new PvPUnitKey(UnitCategory.Aircraft, "PvPStratBomber");
            public static PvPUnitKey PvPSpyPlane { get; } = new PvPUnitKey(UnitCategory.Aircraft, "PvPSpyPlane");
            public static PvPUnitKey PvPTestAircraft { get; } = new PvPUnitKey(UnitCategory.Aircraft, "PvPTestAircraft");
            public static PvPUnitKey PvPSpySatellite { get; } = new PvPUnitKey(UnitCategory.Aircraft, "PvPSpySatellite");
            public static PvPUnitKey PvPMissileFighter { get; } = new PvPUnitKey(UnitCategory.Aircraft, "PvPMissileFighter");
            public static PvPUnitKey PvPDeathstar { get; } = new PvPUnitKey(UnitCategory.Aircraft, "PvPDeathstar");

            // Ships
            public static PvPUnitKey PvPAttackBoat { get; } = new PvPUnitKey(UnitCategory.Naval, "PvPAttackBoat");
            public static PvPUnitKey PvPAttackRIB { get; } = new PvPUnitKey(UnitCategory.Naval, "PvPAttackRIB");
            public static PvPUnitKey PvPFrigate { get; } = new PvPUnitKey(UnitCategory.Naval, "PvPFrigate");
            public static PvPUnitKey PvPDestroyer { get; } = new PvPUnitKey(UnitCategory.Naval, "PvPDestroyer");
            public static PvPUnitKey PvPSiegeDestroyer { get; } = new PvPUnitKey(UnitCategory.Naval, "PvPSiegeDestroyer");
            public static PvPUnitKey PvPArchonBattleship { get; } = new PvPUnitKey(UnitCategory.Naval, "PvPArchonBattleship");
            public static PvPUnitKey PvPGlassCannoneer { get; } = new PvPUnitKey(UnitCategory.Naval, "PvPGlassCannoneer");
            public static PvPUnitKey PvPGunBoat { get; } = new PvPUnitKey(UnitCategory.Naval, "PvPGunBoat");
            public static PvPUnitKey PvPRocketTurtle { get; } = new PvPUnitKey(UnitCategory.Naval, "PvPRocketTurtle");
            public static PvPUnitKey PvPFlakTurtle { get; } = new PvPUnitKey(UnitCategory.Naval, "PvPFlakTurtle");

            public static ReadOnlyCollection<IPrefabKey> AllKeys = new ReadOnlyCollection<IPrefabKey>(new List<IPrefabKey>()
            {
                // Aircraft
                PvPBomber, PvPFighter, PvPGunship, PvPSteamCopter, PvPBroadsword, PvPStratBomber, PvPSpyPlane, PvPTestAircraft, PvPSpySatellite, PvPMissileFighter, PvPDeathstar,
                // Ships
                PvPAttackBoat, PvPAttackRIB, PvPFrigate, PvPDestroyer, PvPSiegeDestroyer, PvPArchonBattleship, PvPGlassCannoneer, PvPGunBoat, PvPRocketTurtle, PvPFlakTurtle
            });
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
            public static PvPHullKey PvPTasDevil { get; } = new PvPHullKey("PvPTasDevil");
            public static PvPHullKey PvPYeti { get; } = new PvPHullKey("PvPYeti");
            public static PvPHullKey PvPRickshaw { get; } = new PvPHullKey("PvPRickshaw");
            public static PvPHullKey PvPBlackRig { get; } = new PvPHullKey("PvPBlackRig");
            public static PvPHullKey PvPFlea { get; } = new PvPHullKey("PvPFlea");
            public static PvPHullKey PvPShepherd { get; } = new PvPHullKey("PvPShepherd");
            public static PvPHullKey PvPMicrolodon { get; } = new PvPHullKey("PvPMicrolodon");
            public static PvPHullKey PvPPistol { get; } = new PvPHullKey("PvPPistol");
            public static PvPHullKey PvPGoatherd { get; } = new PvPHullKey("PvPGoatherd");
            public static PvPHullKey PvPMegalith { get; } = new PvPHullKey("PvPMegalith");
            public static PvPHullKey PvPBasicRig { get; } = new PvPHullKey("PvPBasicRig");
            public static PvPHullKey PvPCricket { get; } = new PvPHullKey("PvPCricket");

            const int MaxHullSupported = (int)HullType.Cricket;

            public static IPrefabKey HullKeyFromType(HullType hullType)
            {
                if((int)hullType > MaxHullSupported)
                {
                    Debug.LogError($"Tried to load PvP cruiser prefab for {hullType}, this is not available.");
                    return PvPTrident;
                }
                else
                    return AllKeys[(int)hullType];
            }

            public static ReadOnlyCollection<IPrefabKey> AllKeys = new ReadOnlyCollection<IPrefabKey>(new List<IPrefabKey>()
            {
                PvPBullshark, PvPEagle, PvPHammerhead, PvPLongbow, PvPMegalodon, PvPRaptor,
                PvPRockjaw, PvPTrident, PvPTasDevil, PvPYeti, PvPRickshaw, PvPBlackRig,
                PvPFlea, PvPShepherd, PvPMicrolodon, PvPPistol, PvPGoatherd, PvPMegalith,
                PvPBasicRig, PvPCricket
            });
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
            public static PvPExplosionKey PvPExplosionMF { get; } = new PvPExplosionKey("PvPExplosionMF");
            public static PvPExplosionKey PvPRailSlugImpact { get; } = new PvPExplosionKey("PvPRailSlugImpact");
            public static PvPExplosionKey PvPExplosionFirecracker { get; } = new PvPExplosionKey("PvPExplosionFirecracker");
            public static PvPExplosionKey PvPExplosion150 { get; } = new PvPExplosionKey("PvPExplosion1.5");
            public static PvPExplosionKey PvPExplosion500 { get; } = new PvPExplosionKey("PvPExplosion5.0");
            public static PvPExplosionKey PvPExplosionFiveShellCluster { get; } = new PvPExplosionKey("PvPExplosionFiveShellCluster");

            // don't change the order!!
            public static ReadOnlyCollection<IPrefabKey> AllKeys = new ReadOnlyCollection<IPrefabKey>(new List<IPrefabKey>()
            {
                PvPBulletImpact, PvPHighCalibreBulletImpact, PvPTinyBulletImpact, PvPNovaShellImpact, PvPRocketShellImpact, PvPBombExplosion,
                PvPFlakExplosion, PvPExplosion75, PvPExplosion100, PvPExplosionMF, PvPRailSlugImpact, PvPExplosionFirecracker, PvPExplosion150, PvPExplosion500, PvPExplosionFiveShellCluster
            });

            public static IPrefabKey GetKey(PvPExplosionType explosionType)
            {
                return AllKeys[(int)explosionType];
            }
        }

        public static class PvPProjectiles
        {
            public static PvPProjectileKey PvPBullet { get; } = new PvPProjectileKey("PvPBullet");
            public static PvPProjectileKey PvPHighCalibreBullet { get; } = new PvPProjectileKey("PvPHighCalibreBullet");
            public static PvPProjectileKey PvPTinyBullet { get; } = new PvPProjectileKey("PvPTinyBullet");
            public static PvPProjectileKey PvPFlakBullet { get; } = new PvPProjectileKey("PvPFlakBullet");
            public static PvPProjectileKey PvPShellLarge { get; } = new PvPProjectileKey("PvPShellLarge");
            public static PvPProjectileKey PvPNovaShell { get; } = new PvPProjectileKey("PvPNovaShell");
            public static PvPProjectileKey PvPFiveShellCluster { get; } = new PvPProjectileKey("PvPFiveShellCluster");
            public static PvPProjectileKey PvPRocketShell { get; } = new PvPProjectileKey("PvPRocketShell");
            public static PvPProjectileKey PvPShellSmall { get; } = new PvPProjectileKey("PvPShellSmall");
            public static PvPProjectileKey PvPBomb { get; } = new PvPProjectileKey("PvPBomb");
            public static PvPProjectileKey PvPStratBomb { get; } = new PvPProjectileKey("PvPStratBomb");
            public static PvPProjectileKey PvPRocket { get; } = new PvPProjectileKey("PvPRocket");
            public static PvPProjectileKey PvPRocketSmall { get; } = new PvPProjectileKey("PvPRocketSmall");
            public static PvPProjectileKey PvPMissileFirecracker { get; } = new PvPProjectileKey("PvPMissileFirecracker");
            public static PvPProjectileKey PvPNuke { get; } = new PvPProjectileKey("PvPNuke");
            public static PvPProjectileKey PvPMissileSmall { get; } = new PvPProjectileKey("PvPMissileSmall");
            public static PvPProjectileKey PvPMissileMedium { get; } = new PvPProjectileKey("PvPMissileMedium");
            public static PvPProjectileKey PvPMissileMF { get; } = new PvPProjectileKey("PvPMissileMF");
            // public static PvPProjectileKey PvPRailSlug { get; } = new PvPProjectileKey("PvPRailSlug");
            public static PvPProjectileKey PvPMissileLarge { get; } = new PvPProjectileKey("PvPMissileLarge");
            public static PvPProjectileKey PvPMissileSmart { get; } = new PvPProjectileKey("PvPMissileSmart");

            public static IPrefabKey[] Shells = new IPrefabKey[]
            {
                PvPBullet,
                PvPHighCalibreBullet,
                PvPTinyBullet,
                PvPFlakBullet,
                PvPShellLarge,
                PvPNovaShell,
                PvPFiveShellCluster,
                PvPRocketShell,
                PvPShellSmall
            };

            public static IPrefabKey[] Bombs = new IPrefabKey[]
            {
                PvPBomb,
                PvPStratBomb
            };

            public static IPrefabKey[] Rockets = new IPrefabKey[]
            {
                PvPRocket,
                PvPRocketSmall,
                PvPMissileFirecracker, // <-- yes, this is using a RocketController!
            };

            public static IPrefabKey[] Missiles = new IPrefabKey[]
            {
                PvPMissileSmall,
                PvPMissileMedium,
                PvPMissileMF,
                //PvPRailSlug,
                PvPMissileLarge,
            };

            public static IPrefabKey[] Nukes = new IPrefabKey[]
            {
                PvPNuke
            };

            public static IPrefabKey[] SmartMissiles = new IPrefabKey[]
            {
                PvPMissileSmart
            };

            public static IPrefabKey[][] AllKeysByCategory = new IPrefabKey[][]
            {
                Shells,
                Bombs,
                Rockets,
                Missiles,
                Nukes,
                SmartMissiles
            };

            public static PvPProjectileControllerType GetProjectileControllerType(PvPProjectileType projectileType)
            {
                for (int i = 0; i < AllKeysByCategory.Length; i++)
                    if (AllKeysByCategory[i].Contains(GetKey(projectileType)))
                        return (PvPProjectileControllerType)i;

                throw new Exception();
            }

            public static ReadOnlyCollection<IPrefabKey> AllKeys = new ReadOnlyCollection<IPrefabKey>(new List<IPrefabKey>()
            {
                PvPBullet, PvPHighCalibreBullet, PvPTinyBullet, PvPFlakBullet, PvPShellLarge, PvPNovaShell, PvPFiveShellCluster, PvPRocketShell, PvPShellSmall,
                PvPBomb, PvPStratBomb,
                PvPRocket, PvPRocketSmall, PvPMissileFirecracker, PvPNuke,
                PvPMissileSmall, PvPMissileMedium, PvPMissileMF, /*PvPRailSlug,*/ PvPMissileLarge,PvPMissileSmart
            });

            public static IPrefabKey GetKey(PvPProjectileType projectileType)
            {
                Debug.Log((int)projectileType);
                return AllKeys[(int)projectileType];
            }
        }

        public static class PvPShipDeaths
        {
            public static PvPShipDeathKey PvPAttackBoat { get; } = new PvPShipDeathKey("PvPShipDeathAttackBoat");
            public static PvPShipDeathKey PvPAttackRIB { get; } = new PvPShipDeathKey("PvPShipDeathAttackRIB");
            public static PvPShipDeathKey PvPFrigate { get; } = new PvPShipDeathKey("PvPShipDeathFrigate");
            public static PvPShipDeathKey PvPDestroyer { get; } = new PvPShipDeathKey("PvPShipDeathDestroyer");
            public static PvPShipDeathKey PvPSiegeDestroyer { get; } = new PvPShipDeathKey("PvPShipDeathSiegeDestroyer");
            public static PvPShipDeathKey PvPArchon { get; } = new PvPShipDeathKey("PvPShipDeathMannOWar");
            public static PvPShipDeathKey PvPGlassCannoneer { get; } = new PvPShipDeathKey("PvPShipDeathGlassCannoneer");
            public static PvPShipDeathKey PvPGunBoat { get; } = new PvPShipDeathKey("PvPShipDeathGunBoat");
            public static PvPShipDeathKey PvPTurtle { get; } = new PvPShipDeathKey("PvPShipDeathTurtle");

            public static ReadOnlyCollection<IPrefabKey> AllKeys = new ReadOnlyCollection<IPrefabKey>(new List<IPrefabKey>()
            {
                PvPAttackBoat, PvPAttackRIB, PvPFrigate, PvPDestroyer,PvPSiegeDestroyer, PvPArchon, PvPGlassCannoneer, PvPGunBoat, PvPTurtle
            });

            public static IPrefabKey GetKey(PvPShipDeathType deathType)
            {
                return AllKeys[(int)deathType];
            }
        }

        public static IPrefabKey AudioSource { get; } = new PvPGenericKey("PvPAudioSource", "UI/Sound");

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
            public static PvPBuildableOutlineKey PvPEngineeringBay6Outline { get; } = new PvPBuildableOutlineKey("PvPEngineeringBay6Outline");
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
            public static PvPBuildableOutlineKey PvPRailCannonOutline { get; } = new PvPBuildableOutlineKey("PvPRailCannonOutline");
            public static PvPBuildableOutlineKey PvPRocketLauncherOutline { get; } = new PvPBuildableOutlineKey("PvPRocketLauncherOutline");
            public static PvPBuildableOutlineKey PvPSamSiteOutline { get; } = new PvPBuildableOutlineKey("PvPSamSiteOutline");
            public static PvPBuildableOutlineKey PvPShieldGeneratorOutline { get; } = new PvPBuildableOutlineKey("PvPShieldGeneratorOutline");
            public static PvPBuildableOutlineKey PvPStealthGeneratorOutline { get; } = new PvPBuildableOutlineKey("PvPStealthGeneratorOutline");
            public static PvPBuildableOutlineKey PvPTeslaCoilOutline { get; } = new PvPBuildableOutlineKey("PvPTeslaCoilOutline");
            public static PvPBuildableOutlineKey PvPUltraliskOutline { get; } = new PvPBuildableOutlineKey("PvPUltraliskOutline");
            public static PvPBuildableOutlineKey PvPSpySatelliteLauncherOutline { get; } = new PvPBuildableOutlineKey("PvPSpySatelliteLauncherOutline");
            public static PvPBuildableOutlineKey PvPIonCannonOutline { get; } = new PvPBuildableOutlineKey("PvPIonCannonOutline");
            public static PvPBuildableOutlineKey PvPMissilePodOutline { get; } = new PvPBuildableOutlineKey("PvPMissilePodOutline");
            public static PvPBuildableOutlineKey PvPNovaArtilleryOutline { get; } = new PvPBuildableOutlineKey("PvPNovaArtilleryOutline");
            public static PvPBuildableOutlineKey PvPCoastguardOutline { get; } = new PvPBuildableOutlineKey("PvPCoastguardOutline");
            public static PvPBuildableOutlineKey PvPFlakTurretOutline { get; } = new PvPBuildableOutlineKey("PvPFlakTurretOutline");
            public static PvPBuildableOutlineKey PvPCIWSOutline { get; } = new PvPBuildableOutlineKey("PvPCIWSOutline");
            public static PvPBuildableOutlineKey PvPUltraCIWSOutline { get; } = new PvPBuildableOutlineKey("PvPUltraCIWSOutline");
            public static PvPBuildableOutlineKey PvPGlobeShieldOutline { get; } = new PvPBuildableOutlineKey("PvPGlobeShieldOutline");
            public static PvPBuildableOutlineKey PvPSledgehammerOutline { get; } = new PvPBuildableOutlineKey("PvPSledgehammerOutline");
            public static PvPBuildableOutlineKey PvPGrapheneBarrierOutline { get; } = new PvPBuildableOutlineKey("PvPGrapheneBarrierOutline");
            public static PvPBuildableOutlineKey PvPBlastVLSOutline { get; } = new PvPBuildableOutlineKey("PvPBlastVLSOutline");
            public static PvPBuildableOutlineKey PvPFirecrackerVLSOutline { get; } = new PvPBuildableOutlineKey("PvPFirecrackerVLSOutline");
            public static PvPBuildableOutlineKey PvPCannonOutline { get; } = new PvPBuildableOutlineKey("PvPCannonOutline");

            public static ReadOnlyCollection<IPrefabKey> AllKeys = new ReadOnlyCollection<IPrefabKey>(new List<IPrefabKey>()
            {
                PvPAirFactoryOutline, PvPAntiAirTurretOutline, PvPAntiShipTurretOutline, PvPArtilleryOutline, PvPBroadsidesOutline,
                PvPControlTowerOutline, PvPDeathstarLauncherOutline, PvPEngineeringBay4Outline, PvPEngineeringBay6Outline, PvPEngineeringBay8Outline,
                PvPEngineeringBayOutline, PvPGatlingMortarOutline, PvPKamikazeSignalOutline, PvPLocalBoosterOutline, PvPMLRSOutline,
                PvPMortarOutline, PvPNavalFactoryOutline, PvPNukeLauncherOutline, PvPRailgunOutline, PvPRailCannonOutline, PvPRocketLauncherOutline,
                PvPSamSiteOutline, PvPShieldGeneratorOutline, PvPStealthGeneratorOutline, PvPTeslaCoilOutline, PvPUltraliskOutline,
                PvPSpySatelliteLauncherOutline, PvPIonCannonOutline, PvPMissilePodOutline, PvPNovaArtilleryOutline, PvPCoastguardOutline,
                PvPFlakTurretOutline, PvPCIWSOutline, PvPUltraCIWSOutline, PvPGlobeShieldOutline, PvPSledgehammerOutline,
                PvPGrapheneBarrierOutline, PvPCannonOutline, PvPBlastVLSOutline, PvPFirecrackerVLSOutline
            });
        }
    }

    public enum PvPProjectileControllerType
    {
        ProjectileController,
        BombController,
        RocketController,
        MissileController,
        NukeController,
        SmartMissileController
    }

    public enum PvPProjectileType
    {
        PvPBullet = 0,
        PvPHighCalibreBullet = 1,
        PvPTinyBullet = 2,
        PvPFlakBullet = 3,
        PvPShellLarge = 4,
        PvPNovaShell = 5,
        PvPFiveShellCluster = 6,
        PvPRocketShell = 7,
        PvPShellSmall = 8,
        PvPBomb = 9,
        PvPStratBomb = 10,
        PvPRocket = 11,
        PvPRocketSmall = 12,
        PvPMissileFirecracker = 13,
        PvPNuke = 14,
        PvPMissileSmall = 15,
        PvPMissileMedium = 16,
        PvPMissileMF = 17,
        // PvPRailSlug = 18,
        PvPMissileLarge = 18,
        PvPMissileSmart = 19
    }

    public enum PvPShipDeathType
    {
        PvPAttackBoat = 0,
        PvPAttackRIB = 1,
        PvPFrigate = 2,
        PvPDestroyer = 3,
        PvPSiegeDestroyer = 4,
        PvPArchon = 5,
        PvPGlassCannoneer = 6,
        PvPGunBoat = 7,
        PvPTurtle = 8
    }

    public enum PvPExplosionType
    {
        PvPBulletImpact = 0,
        PvPHighCalibreBulletImpact = 1,
        PvPTinyBulletImpact = 2,
        PvPNovaShellImpact = 3,
        PvPRocketShellImpact = 4,
        PvPBombExplosion = 5,
        PvPFlakExplosion = 6,
        PvPExplosion75 = 7,
        PvPExplosion100 = 8,
        PvPExplosionMF = 9,
        PvPRailSlugImpact = 10,
        PvPExplosionFirecracker = 11,
        PvPExplosion150 = 12,
        PvPExplosion500 = 13,
        PvPExplosionFiveShellCluster = 14
    }
}
